using System;
using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdEllipseShape : LcdShape{
        public LcdPoint P0 { get; set; }
        public double RX { get; set; }
        public double RY { get; set; }
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Ellipse(P0{P0} RX({RX}) RY({RY}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            P0 = param.GetPoint();
            RX = param.GetDouble();
            RY = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("ELLIPSE");
            sw.Write($"\t{P0.ToLcdString()} {RX} {RY} ");
            sw.WriteLine($"{LineStyle.ToLcdString()} {FaceColor.ToLcdString()} ");
        }
    }
}
