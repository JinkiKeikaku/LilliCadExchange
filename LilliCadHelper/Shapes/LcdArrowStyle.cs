namespace LilliCadHelper.Shapes
{
    public class LcdArrowStyle {
        public int ArrowType { get; set; }
        public float ArrowSize { get; set; }
        public LcdArrowStyle(int arrowType=0, float arrowSize=3) {
            ArrowType = arrowType;
            ArrowSize = arrowSize;
        }
        public override string ToString() => $"{ArrowType} {ArrowSize}";
    }
}
