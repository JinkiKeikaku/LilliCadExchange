using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdSplineShape : LcdShape{
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public LcdArrowStyle StartArrow { get; set; }
        public LcdArrowStyle EndArrow { get; set; }
        public List<LcdPoint> Points { get; set;  } = new();
        public override string ToString() {
            return $"Spline(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}  Arrow0{StartArrow} Arrow1{EndArrow}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
            //LilliCad Ver1.4.8zまでは面色を保存していなかったので以下の処理となる。
            if (!param.IsEndOfLine)   FaceColor = param.GetFaceColor();
            Points = sr.ReadPoints();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("SPLINE");
            sw.WriteParamLine(LineStyle, StartArrow, EndArrow, FaceColor);
            sw.WritePoints(Points);
        }
    }
}
