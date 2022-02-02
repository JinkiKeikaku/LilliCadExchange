namespace LilliCadHelper
{
    public class LcdPoint
    {
        public double X;
        public double Y;
        public LcdPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override string ToString() => $"{X} {Y}";
    }
}
