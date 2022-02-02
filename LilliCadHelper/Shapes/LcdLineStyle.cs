namespace LilliCadHelper.Shapes
{
    public class LcdLineStyle {
        public int LineColor { get; set; }
        public int LineType { get; set; }
        public float LineWidth { get; set; }

        public LcdLineStyle(int lineColor, int lineType, float lineWidth) {
            LineColor = lineColor;
            LineType = lineType;
            LineWidth = lineWidth;
        }


        public string ToLcdString() => $"{LineColor} {LineType} {LineWidth}";

        public override string ToString() {
            var ls = ((LineType & 128) != 0) ? "Construction" : LineType.ToString();
            return $"(Color{LineColor}Type({ls}),Width({LineWidth}))";
        }

    }
}
