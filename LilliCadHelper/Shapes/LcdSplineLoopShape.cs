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
            var param = sr.ReadParameters();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            Points = sr.ReadPoints();
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("SPLINELOOP");
            sw.WriteLine($"\t{LineStyle.ToLcdString()} {FaceColor.ToLcdString()} ");
            WritePoints(sw, Points);
        }
    }
}
