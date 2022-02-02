using LilliCadHelper.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LilliCadHelper
{
    class LcdStreamReader
    {
        public class Parameters
        {
            public int mPos = 0;
            public string[] Tokens;
            public Parameters(string[] tokens)
            {
                Tokens = tokens;
            }
            public bool IsEndOfLine => Tokens.Length <= mPos;

            public int GetInt() => int.Parse(Tokens[mPos++]);
            public double GetDouble() => double.Parse(Tokens[mPos++]);
            public float GetFloat() => float.Parse(Tokens[mPos++]);
            public LcdPoint GetPoint()
            {
                //評価準が左から右なのでmPos++はOK
                return new LcdPoint(double.Parse(Tokens[mPos++]), double.Parse(Tokens[mPos++]));
            }
            //read LC LS LW
            public LcdLineStyle GetLineStyle() => new LcdLineStyle(GetInt(), GetInt(), GetFloat());
            //read AT AS
            public LcdArrowStyle GetArrowStyle() => new LcdArrowStyle(GetInt(), GetFloat());
            public LcdFaceColor GetFaceColor()
            {
                var fc = new LcdFaceColor();
                if (Tokens[mPos][0] == 'G' || Tokens[mPos][0] == 'g')
                {
                    //グラデーション
                    switch (Tokens[mPos++][1])
                    {
                        case '1':
                            {
                                //Line
                                fc.GradationType = LcdFaceColor.Gradation.Line;
                                fc.Angle = GetFloat();
                            }
                            break;
                        case '2':
                            {
                                //Rect
                                fc.GradationType = LcdFaceColor.Gradation.Rectangle;
                                fc.Angle = GetFloat();
                                fc.X = GetFloat();
                                fc.Y = GetFloat();
                            }
                            break;
                        case '3':
                            {
                                //Circle
                                fc.X = GetFloat();
                                fc.Y = GetFloat();
                            }
                            break;
                        default:
                            {
                                //解析不能
                                throw new Exception("Gradient color format error");
                            }
                    }
                    int n = GetInt();
                    if (n == 2)
                    {
                        fc.Colors = new int[2];
                        fc.Colors[0] = GetInt();   //開始色
                        fc.Colors[1] = GetInt();   //終了色
                    }
                    else
                    {
                        fc.Colors = new int[3];
                        //グラデーションは３色までしかない
                        fc.Colors[0] = GetInt();   //開始色
                        fc.Colors[1] = GetInt();   //中間色
                        fc.Colors[2] = GetInt();   //終了色
                        fc.MP = GetFloat(); //中間色の位置
                    }
                }
                else
                {
                    fc.GradationType = LcdFaceColor.Gradation.None;
                    fc.Colors = new int[1] { GetInt() };
                }
                return fc;
            }

            public LcdSizeStyle GetSizeStyle()
            {
                var s = new LcdSizeStyle
                {
                    Linegap = GetFloat(),
                    Linejut = GetFloat(),
                    Linedrop = GetFloat(),
                    Textgap = GetFloat(),
                };
                return s;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LcdStreamReader(StreamReader sr)
        {
            mSr = sr;
        }

        public Char Peek() => (Char)mSr.Peek();

        public Parameters GetParameters()
        {
            string a = ReadLineForParameters();
            return new Parameters(a.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public bool IsEndOfStream => mSr.EndOfStream;

        public List<LcdPoint> ReadPoints()
        {
            var points = new List<LcdPoint>();
            var param = GetParameters();
            var n = param.GetInt();
            for (int i = 0; i < n; i++)
            {
                param = GetParameters();
                points.Add(param.GetPoint());
            }
            return points;
        }


        public string ReadSingleString()
        {
            if (mSr.Read() >= 0)
            {   //最初の空白文字除去
                var line = mSr.ReadLine();
                if (line != null) return line;
            }
            throw new Exception("Unexpectedly reached the end of the file");
        }

        public string ReadString()
        {
            string s = "";
            var param = GetParameters();
            int n = param.GetInt();
            if (n <= 0) return "";
            for (int i = 0; i < n - 1; i++)
            {
                s = s + ReadSingleString() + "\n";
            }
            s += ReadSingleString();
            return s;
        }

        public string ReadLine()
        {
            while (!mSr.EndOfStream)
            {
                var line = mSr.ReadLine();
                line = line.TrimEnd();
                if (!string.IsNullOrEmpty(line)) return line;
            }
            throw new Exception("Unexpectedly reached the end of the file");
        }

        public string ReadTrimmedLine()
        {
            while (!mSr.EndOfStream)
            {
                var line = mSr.ReadLine();
                line = line.Trim();
                if (!string.IsNullOrEmpty(line)) return line;
            }
            throw new Exception("Unexpectedly reached the end of the file");
        }

        public string ReadLineForParameters()
        {
            while (!mSr.EndOfStream)
            {
                var line = mSr.ReadLine();
                line = line.Trim();
                if (!string.IsNullOrEmpty(line)) return line;
            }
            throw new Exception("Unexpectedly reached the end of the file");
        }

        public byte[] ReadBytes()
        {
            var isCompressd = false;
            var param = GetParameters();
            var len = param.GetInt();
            if (len <= 0) return null;
            if (param.Tokens.Length == 1) return null;    //old version.Dont use base64. So old that not support.
            if (param.Tokens[1] != "BASE64") return null;
            if (param.Tokens.Length >= 3) isCompressd = int.Parse(param.Tokens[2]) != 0;
            int i = 0;
            if (isCompressd)
            {
                var sb = new StringBuilder();
                while (Char.IsWhiteSpace(Peek()) && !IsEndOfStream)
                {
                    sb.Append(ReadTrimmedLine());
                }
                var m = Convert.FromBase64String(sb.ToString());
                using var bs = new MemoryStream(m);
                //bs.Position = 2;  //DeflateStreamを使うときは最初の２バイトをスキップする。
                //using var ds = new DeflateStream(bs, CompressionMode.Decompress);
                using var ds = new ZLibStream(bs, CompressionMode.Decompress);
                var buf = new byte[len];
                ds.Read(buf, 0, len);
                ds.Close();
                return buf;
            }
            else
            {
                var buf = new List<Byte>(len);
                while (i < len)
                {
                    var b = Convert.FromBase64String(ReadTrimmedLine());
                    i += b.Length;
                    buf.AddRange(b);
                }
                return buf.ToArray();
            }
        }
        StreamReader mSr;
    }
}
