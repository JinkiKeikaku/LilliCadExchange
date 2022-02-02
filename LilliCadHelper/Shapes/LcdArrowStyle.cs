namespace LilliCadHelper.Shapes
{
    public class LcdArrowStyle {
        public int ArrowType { get; set; }
        public float ArrowSize { get; set; }
        public LcdArrowStyle(int arrowType, float arrowSize) {
            ArrowType = arrowType;
            ArrowSize = arrowSize;
        }
        public override string ToString() => $"{ArrowType} {ArrowSize}";
    }
}
