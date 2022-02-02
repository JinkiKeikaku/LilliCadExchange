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
        public string ToLcdString() => $"{X} {Y}";
        public override string ToString()
        {
            return $"({X:F1}, {Y:F1})";
        }
    }
}
