using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdArcShape : LcdShape{
        public LcdPoint P0 { get; set; }
        public double Radius { get; set; }
        public double StartAngleRad { get; set; }
        public double EndAngleRad { get; set; }
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public LcdArrowStyle StartArrow { get; set; }
        public LcdArrowStyle EndArrow { get; set; }
        public override string ToString() {
            return $"Arc(P0{P0} R({Radius}) Angle({StartAngleRad}, EndAngle)LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            StartAngleRad = param.GetDouble();
            EndAngleRad = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("ARC");
            sw.Write($"\t{P0.ToLcdString()} {Radius} {StartAngleRad} {EndAngleRad} ");
            sw.Write($"{LineStyle.ToLcdString()} ");
            sw.Write($"{FaceColor.ToLcdString()} ");
            sw.Write($"{StartArrow.ToLcdString()} ");
            sw.WriteLine($"{EndArrow.ToLcdString()} ");
        }
    }
}
