using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper.Shapes
{
    public class LcdFanShape : LcdShape {
        public LcdPoint P0 { get; set; }
        public double Radius { get; set; }
        public double StartAngleRad { get; set; }
        public double EndAngleRad { get; set; }
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Fan(P0{P0} R({Radius}) Angle({StartAngleRad}, {EndAngleRad})LineStyle{LineStyle} FaceColor{FaceColor}";
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
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("FAN");
            sw.WriteParamLine(P0, Radius, StartAngleRad, EndAngleRad, LineStyle, FaceColor);
        }
    }
}
