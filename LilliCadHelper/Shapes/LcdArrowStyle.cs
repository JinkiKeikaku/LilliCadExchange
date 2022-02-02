namespace LilliCadHelper.Shapes
{
    public class LcdArrowStyle {
        public int ArrowType { get; set; }
        public float ArrowSize { get; set; }
        public LcdArrowStyle(int arrowType, float arrowSize) {
            ArrowType = arrowType;
            ArrowSize = arrowSize;
        }
        public string ToLcdString() => $"{ArrowType} {ArrowSize}";
        public override string ToString() {
            return $"(Type({ArrowType}),Size({ArrowSize}))";
        }
    }
}
