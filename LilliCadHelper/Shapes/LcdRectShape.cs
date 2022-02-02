using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdRectShape : LcdShape{
        public LcdPoint P0 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        public override string ToString() {
            return $"Rect(P0{P0} Size({Width}, {Height}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Width = param.GetDouble();
            Height = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("RECT");
            sw.WriteParamLine(P0,Width,Height,LineStyle,FaceColor);
        }
    }
}
