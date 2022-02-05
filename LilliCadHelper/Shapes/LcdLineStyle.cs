namespace LilliCadHelper.Shapes
{
    public class LcdLineStyle {
        public int LineColor { get; set; }
        public int LineType { get; set; }
        public float LineWidth { get; set; }

        public LcdLineStyle(int lineColor = 0, int lineType =0, float lineWidth=0) {
            LineColor = lineColor;
            LineType = lineType;
            LineWidth = lineWidth;
        }


        public override string ToString() => $"{LineColor} {LineType} {LineWidth}";

    }
}
