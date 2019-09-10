using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;

namespace common.utils
{
    public static class ZlibUtil
    {
        static readonly UTF8Encoding encoding = new UTF8Encoding();

        public static bool IsZip(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            bool result = ZipFile.IsZipFile(ms, false);
            ms.Dispose();
            return result;
        }

        public static void ExtractAll(byte [] bytes, Action<string, byte[]> onEntityExtract)
        {
            var zipStream = new MemoryStream(bytes);
            using (ZipFile zip = ZipFile.Read(zipStream))
            {
                foreach (var entity in zip.Entries)
                {
                    if (entity.IsDirectory) continue;
                    var ms = new MemoryStream(new byte[entity.UncompressedSize]);
                    entity.Extract(ms);
                    ms.Dispose();
                    onEntityExtract(entity.FileName, ms.ToArray());
                }
            }
            zipStream.Dispose();
        }
        
        public static IEnumerable<KeyValuePair<string, string>> ExtractAll(byte[] compressedArray)
        {
            var stream = new MemoryStream(compressedArray);
            var zipFile = ZipFile.Read(stream);

            foreach (ZipEntry e in zipFile)
            {
                MemoryStream ms = new MemoryStream();
                e.Extract(ms);

                yield return new KeyValuePair<string, string>(e.FileName, encoding.GetString(ms.GetBuffer())); 
                ms.Dispose();
            }

            zipFile.Dispose();
            stream.Dispose();
        }
        
        static void CopyStream(Stream src, Stream dest)
        {
            var buffer = new byte[1024];
            int len = src.Read(buffer, 0, buffer.Length);
            while (len > 0)
            {
                dest.Write(buffer, 0, len);
                len = src.Read(buffer, 0, buffer.Length);
            }
            dest.Flush();
        }

        public static byte[] DecompressStreamToArray(MemoryStream stream)
        {
            var decompressedStream = DecompressStream(stream);
            var bytes = decompressedStream.ToArray();
            decompressedStream.Close();
            return bytes;
        }

        public static byte[] CompressStreamToArray(MemoryStream stream)
        {
            var compressedStream = CompressStream(stream);
            var bytes = compressedStream.ToArray();
            compressedStream.Close();
            return bytes;
        }

        public static byte[] DecompressArray(byte[] compressedArray)
        {
            return DecompressStreamToArray(new MemoryStream(compressedArray));
        }

        public static string DecompressString(byte[] compressedArray)
        {
            return encoding.GetString(DecompressArray(compressedArray));
        }

        public static byte[] CompressArray(byte[] array)
        {
            return CompressStreamToArray(new MemoryStream(array));
        }

        public static MemoryStream DecompressStream(MemoryStream stream)
        {
            stream.Position = 0;
            var decompressedStream = new MemoryStream();
            var zOut = new ZlibStream(decompressedStream, CompressionMode.Decompress, true);
            CopyStream(stream, zOut);
            zOut.Close();
            decompressedStream.Position = 0;
            return decompressedStream;
        }

        public static MemoryStream CompressStream(MemoryStream stream)
        {
            stream.Position = 0;
            var compressedStream = new MemoryStream();
            var zOut = new ZlibStream(compressedStream, CompressionMode.Compress, CompressionLevel.BestCompression, true);
            CopyStream(stream, zOut);
            zOut.Close();
            compressedStream.Position = 0;
            return compressedStream;
        }
    }
}
