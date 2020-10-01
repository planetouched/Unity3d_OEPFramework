using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Basement.OEPFramework.Futures;

namespace OEPCommon.AssetBundles.Futures
{
    public class AssetBundleWebRequestFuture : ThreadSafeFuture
    {
        private static readonly Crc32 _assetBundleCrc32; 
        private readonly string _storageUrl;
        private readonly string _assetBundleFile;
        private readonly uint _crc32;
        private readonly int _resendAttemptsCount;
        private int _resendCounter;
        private readonly bool _async;

        public byte[] result { get; private set; }

        static AssetBundleWebRequestFuture()
        {
            ServicePointManager.ServerCertificateValidationCallback += (p1, p2, p3, p4) => true;
            _assetBundleCrc32 = new Crc32();
        }

        public AssetBundleWebRequestFuture(string storageUrl, string assetBundleFile, bool async, uint crc32 = 0, int resendAttemptsCount = 0)
        {
            _async = async;
            _storageUrl = storageUrl;
            _assetBundleFile = assetBundleFile;
            _crc32 = crc32;
            _resendAttemptsCount = resendAttemptsCount;
        }
        
        protected override void OnRun()
        {
            SendRequest();
        }
        
        private void SendRequest()
        {
            try
            {
                CreateRequestTask().ContinueWith(task => { CompleteAsync(task.Result); }, TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception)
            {
                if (_resendCounter == _resendAttemptsCount)
                {
                    throw new Exception("Can't load: " + _storageUrl);
                }
                
                _resendCounter++;
                SendRequest();
            }
        }

        private void CompleteAsync(byte [] bytes)
        {
            result = bytes;
            
            if (_crc32 != 0 && _crc32 != _assetBundleCrc32.ComputeChecksum(bytes))
            {
                if (_resendCounter == _resendAttemptsCount)
                {
                    throw new Exception("Can't load: " + _storageUrl);
                }
                
                _resendCounter++;
                SendRequest();
            }
            else
            {
                Complete();
            }
        }

        private async Task<byte[]> CreateRequestTask()
        {
            var path = Path.Combine(_storageUrl, _assetBundleFile);

            if (path.ToLower().StartsWith("http"))
            {
                using (var client = new HttpClient())
                {
                    return await client.GetByteArrayAsync(path);
                }
            }
            
            if (_async)
            {
                using (var stream = File.Open(path, FileMode.Open))
                {
                    byte[] buffer = new byte[stream.Length];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
            
            return File.ReadAllBytes(path);
        }

        protected override void OnComplete()
        {
        }
    }
}