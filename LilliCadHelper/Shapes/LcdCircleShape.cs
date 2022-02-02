
using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdCircleShape : LcdShape{
        public LcdPoint P0 { get; set; }
        public double Radius { get; set; }
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Circle(P0{P0} R({Radius}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("CIRCLE");
            sw.WriteParamLine(P0,Radius, LineStyle, FaceColor);
        }
    }
}
