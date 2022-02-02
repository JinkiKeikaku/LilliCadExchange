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
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            StartAngleRad = param.GetDouble();
            EndAngleRad = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("ARC");
            sw.WriteParamLine(P0, Radius, StartAngleRad, EndAngleRad, LineStyle, FaceColor, StartArrow, EndArrow);
        }
    }
}
