
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace LilliCadHelper.Shapes
{
    public abstract class LcdShape
    {
        internal abstract void Read(LcdStreamReader sr);
        internal abstract void Write(StreamWriter sw);

        internal static void WriteString(StreamWriter sw, string text)
        {
            var lines = Regex.Split(text, @"\r\n|\r|\n");
            if (lines.Length == 0)
            {
                sw.WriteLine("\t1");
                sw.WriteLine("\t");
                return;
            }
            sw.WriteLine($"\t{lines.Length}");
            foreach (var s in lines)
            {
                sw.WriteLine($"\t{s}");
            }
        }
        internal static void WritePoints(StreamWriter sw, IReadOnlyList<LcdPoint> points)
        {
            sw.WriteLine($"\t{points.Count}");
            foreach (var p in points)
            {
                sw.WriteLine($"\t{p.ToLcdString()}");
            }
        }
        internal static void WriteBytes(StreamWriter sw, byte[] bytes, bool isCompressed)
        {
            int c = isCompressed ? 1 : 0;
            sw.WriteLine($"\t{bytes.Length} BASE64 {c}");
            if (isCompressed)
            {
                using var bs = new MemoryStream();
                using var ds = new ZLibStream(bs, CompressionMode.Compress);
                ds.Write(bytes, 0, bytes.Length);
                ds.Close();
                var buf = bs.ToArray();

                for (var i = 0; i < buf.Length; i += 54)
                {
                    var size = (buf.Length - i >= 54) ? 54 : buf.Length - i;
                    var s = Convert.ToBase64String(buf, i, size);
                    sw.WriteLine($"\t{s}");
                }
            }
            else
            {
                for (var i = 0; i < bytes.Length; i += 54)
                {
                    var size = (bytes.Length - i >= 54) ? 54 : bytes.Length - i;
                    var s = Convert.ToBase64String(bytes, i, size);
                    sw.WriteLine($"\t{s}");
                }
            }
        }
    }
}
