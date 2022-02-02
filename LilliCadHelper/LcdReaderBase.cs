using LilliCadHelper.Shapes;
using System;
using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper
{
    public class LcdReaderBase
    {
        //public const int LILLICAD_NULL_COLOR = 0x1000000;//(RGB(255,255,255) + 1)

        //public static readonly float[][] mDotPattern = new float[][] {
        //    new float[]{0},
        //    new float[]{1.25f,1.25f},
        //    new float[]{2.5f,2.5f},
        //    new float[]{3.75f,1.25f},
        //    new float[]{3.75f,1.25f, 1.25f,1.25f},
        //    new float[]{6.25f,2.5f, 2.5f,2.5f},
        //    new float[]{3.25f,1.25f, 1.25f,1.25f, 1.25f,1.25f},
        //    new float[]{8.0f,2.5f, 1.25f,2.5f, 1.25f,2.5f},
        //    new float[]{0.625f, 1.875f},
        //};



        public LcdReaderBase()
        {
        }

        //internal LcdShape ReadShape(LcdStreamReader sr)
        //{
        //    return LcdShapeManager.CreateShape(sr);
        //}

        //protected LcdPoint ConvertPoint(double x, double y) => new(x, y);
        //protected double ConvertLength(double x) => x;
        //protected string ReadLine(StreamReader sr)
        //{
        //    while (!sr.EndOfStream)
        //    {
        //        var line = sr.ReadLine();
        //        line = line.Trim();
        //        if (!string.IsNullOrEmpty(line)) return line;
        //    }
        //    throw new Exception("Unexpectedly reached the end of the file");
        //}
        //protected string ReadSingleString(StreamReader sr)
        //{
        //    if (sr.Read() >= 0)
        //    {   //最初の空白文字除去
        //        var line = sr.ReadLine();
        //        if (line != null) return line;
        //    }
        //    throw new Exception("Unexpectedly reached the end of the file");
        //}
        //protected string ReadString(StreamReader sr)
        //{
        //    string s = "";
        //    var c = ReadTokens(sr);
        //    int n = ConvertInt(c, 0);
        //    if (n <= 0) return "";
        //    for (int i = 0; i < n - 1; i++)
        //    {
        //        s = s + ReadSingleString(sr) + "\n";
        //    }
        //    s += ReadSingleString(sr);
        //    return s;
        //}

        //protected string[] ReadTokens(StreamReader sr)
        //{
        //    string a = ReadLine(sr);
        //    return a.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        //}
        //protected int ConvertInt(string[] tokens, int pos)
        //{
        //    return int.Parse(tokens[pos]);
        //}
        //protected double ConvertDouble(string[] tokens, int pos)
        //{
        //    return double.Parse(tokens[pos]);
        //}
        //protected float ConvertFloat(string[] tokens, int pos)
        //{
        //    return float.Parse(tokens[pos]);
        //}

        //protected (LcdPoint p, int pos) ConvertPoint(string[] tokens, int pos)
        //{
        //    return (new LcdPoint(ConvertDouble(tokens, pos), ConvertDouble(tokens, pos + 1)), pos + 2);
        //}
        //void ConvertPoints(StreamReader sr, IList<LcdPoint> points)
        //{
        //    string[] tokens;
        //    tokens = ReadTokens(sr);
        //    var n = ConvertInt(tokens, 0);
        //    for (int i = 0; i < n; i++)
        //    {
        //        tokens = ReadTokens(sr);
        //        (var p, int _) = ConvertPoint(tokens, 0);
        //        points.Add(p);
        //    }
        //}
        //(LcdLineStyle style, int pos) ConvertLineStyle(string[] tokens, int pos)
        //{
        //    int lc = ConvertInt(tokens, pos++);
        //    int ls = ConvertInt(tokens, pos++);
        //    float lw = ConvertFloat(tokens, pos++);
        //    return (new LcdLineStyle(lc, ls, lw), pos);
        //}
        //(LcdArrowStyle style, int pos) ConvertArrowStyle(string[] tokens, int pos)
        //{
        //    int arrowType = ConvertInt(tokens, pos++);
        //    float arrowSize = ConvertFloat(tokens, pos++);
        //    return (new LcdArrowStyle(arrowType, arrowSize), pos);
        //}

        //(LcdFaceColor fc, int pos) ConvertFaceColor(string[] tokens, int pos)
        //{
        //    var fc = new LcdFaceColor();
        //    if (tokens[pos][0] == 'G' || tokens[pos][0] == 'g')
        //    {
        //        //グラデーション
        //        switch (tokens[pos++][1])
        //        {
        //            case '1':
        //                {
        //                    //Line
        //                    fc.GradationType = LcdFaceColor.Gradation.Line;
        //                    fc.Angle = ConvertFloat(tokens, pos++);
        //                }
        //                break;
        //            case '2':
        //                {
        //                    //Rect
        //                    fc.GradationType = LcdFaceColor.Gradation.Rectangle;
        //                    fc.Angle = ConvertFloat(tokens, pos++);
        //                    fc.X = ConvertFloat(tokens, pos++);
        //                    fc.Y = ConvertFloat(tokens, pos++);
        //                }
        //                break;
        //            case '3':
        //                {
        //                    //Circle
        //                    fc.X = ConvertFloat(tokens, pos++);
        //                    fc.Y = ConvertFloat(tokens, pos++);
        //                }
        //                break;
        //            default:
        //                {
        //                    //解析不能
        //                    throw new Exception("Gradient color format error");
        //                }
        //        }
        //        int n = ConvertInt(tokens, pos++);
        //        if (n == 2)
        //        {
        //            fc.Colors[0] = ConvertInt(tokens, pos++);   //開始色
        //            fc.Colors[1] = ConvertInt(tokens, pos++);   //終了色
        //        }
        //        else
        //        {
        //            //グラデーションは３色までしかないのでn==2でなければ中間色１色
        //            fc.Colors[0] = ConvertInt(tokens, pos++);   //開始色
        //            fc.Colors[1] = ConvertInt(tokens, pos++);   //中間色
        //            fc.Colors[2] = ConvertInt(tokens, pos++);   //終了色
        //            fc.MP = ConvertFloat(tokens, pos++); //中間色の位置
        //        }
        //    }
        //    else
        //    {
        //        fc.GradationType = LcdFaceColor.Gradation.Solid;
        //        fc.Colors[0] = ConvertInt(tokens, pos++);
        //    }
        //    return (fc, pos);
        //}
        //(LcdSizeStyle, int) ConvertSizeStyle(string[] tokens, int pos)
        //{
        //    var s = new LcdSizeStyle
        //    {
        //        Linegap = ConvertFloat(tokens, pos++),
        //        Linejut = ConvertFloat(tokens, pos++),
        //        Linedrop = ConvertFloat(tokens, pos++),
        //        Textgap = ConvertFloat(tokens, pos++)
        //    };
        //    return (s, pos);
        //}

    }
}
