using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdSplineLoopShape  : LcdShape{
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public List<LcdPoint> Points { set;  get; } = new();
        public override string ToString() {
            return $"SplineLoop(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            Points = sr.ReadPoints();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("SPLINELOOP");
            sw.WriteParamLine(LineStyle, FaceColor);
            sw.WritePoints(Points);
        }
    }
}
