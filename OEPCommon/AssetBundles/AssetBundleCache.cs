using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OEPCommon.AssetBundles
{
    public static class AssetBundleCache
    {
        private static string _bundlesDirectory;
        public static bool enable { get; set; }

        public static void SetCacheDirectory(string cacheDirectory)
        {
            _bundlesDirectory = Path.Combine(cacheDirectory, "AssetBundles");
            
            if (!Directory.Exists(_bundlesDirectory))
            {
                Directory.CreateDirectory(_bundlesDirectory);
            }
        }
        
        public static async void AddCache(string file, string hash, byte[] body, bool async)
        {
            if (!IsCached(file, hash))
            {
                var tmpName = FileName(Guid.NewGuid().ToString(), "tmp");
                var fileName = FileName(file, hash);

                if (async)
                {
                    using (var stream = File.Open(tmpName, FileMode.CreateNew))
                    {
                        await stream.WriteAsync(body, 0, body.Length);
                    }
                }
                else
                {
                    File.WriteAllBytes(tmpName, body);
                }
                
                File.Move(tmpName, fileName);
            }
        }

        public static async Task<byte[]> GetCache(string file, string hash, bool async)
        {
            if (IsCached(file, hash))
            {
                var fileName = FileName(file, hash);
                
                if (async)
                {
                    using (var stream = File.Open(fileName, FileMode.Open))
                    {
                        byte[] buffer = new byte[stream.Length];
                        await stream.ReadAsync(buffer, 0, buffer.Length);
                        return buffer;
                    }
                }
                
                return File.ReadAllBytes(fileName);
            }

            return null;
        }

        public static async Task<bool> CheckCRC(byte[] body, uint crc, bool async)
        {
            var crc32 = new Crc32();
            
            if (async)
            {
                var task = Task<bool>.Factory.StartNew(() => crc32.ComputeChecksum(body) == crc);
                return await task;
            }
            
            return crc32.ComputeChecksum(body) == crc;
        }
        
        public static bool IsCached(string file, string hash)
        {
            return enable && File.Exists(FileName(file, hash));
        }
        
        public static void Delete(string file, string hash)
        {
            File.Delete(FileName(file, hash));
        }
        
        public static void ClearOtherCachedVersions(List<(string fileName, string hash)> list)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in list)
            {
                dict.Add(item.fileName, item.hash);
            }

            foreach (var file in Directory.GetFiles(_bundlesDirectory, "*.*"))
            {
                if (file.StartsWith("tmp_"))
                {
                    File.Delete(file);
                    continue;
                }
                
                var arr = Path.GetFileName(file).Split(new[] {'_'}, 2);
                var hashTest = arr[0];
                var fileName = arr[1];

                if (dict.TryGetValue(fileName, out var hash))
                {
                    if (hash != hashTest)
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                    File.Delete(file);
                }
            }
        }

        private static string FileName(string file, string hash)
        {
            return Path.Combine(_bundlesDirectory, hash + "_" + file);
        }
    }
}