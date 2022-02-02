using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LilliCadHelper
{
    class LcdStreamWriter
    {
        StreamWriter mSw;
        public LcdStreamWriter(StreamWriter sw)
        {
            mSw = sw;
        }

        public void WriteLine(string s)
        {
            mSw.WriteLine(s);
        }

        public void WriteParamLine(params object[] args)
        {
            int a;
            mSw.Write("\t");
            for (var i=0; i < args.Length-1; i++)
            {
                mSw.Write($"{ConvertArg(args[i])} ");
            }
            mSw.WriteLine($"{ConvertArg(args[^1])}");
            string ConvertArg(object arg)
            {
                if (arg is bool b)
                {
                    return $"{BooleanToInt(b)}";
                }
                else
                {
                    return arg.ToString();
                }
            }

        }
        public void WriteString(string text)
        {
            var lines = Regex.Split(text, @"\r\n|\r|\n");
            if (lines.Length == 0)
            {
                WriteParamLine(1);
                WriteParamLine("");
                return;
            }
            WriteParamLine(lines.Length);
            foreach (var s in lines)
            {
                WriteParamLine(s);
            }
        }
        public void WritePoints(IReadOnlyList<LcdPoint> points)
        {
            WriteParamLine(points.Count);
            foreach (var p in points)
            {
                WriteParamLine(p);
            }
        }
        public void WriteBytes(byte[] bytes, bool isCompressed)
        {
            WriteParamLine(bytes.Length, "BASE64", isCompressed);
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
                    WriteParamLine(s);
                }
            }
            else
            {
                for (var i = 0; i < bytes.Length; i += 54)
                {
                    var size = (bytes.Length - i >= 54) ? 54 : bytes.Length - i;
                    var s = Convert.ToBase64String(bytes, i, size);
                    WriteParamLine(s);
                }
            }
        }


        int BooleanToInt(bool flag) => flag ? 1 : 0;
    }
}
