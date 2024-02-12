namespace LilliCadHelper
{
    /// <summary>
    /// 点クラス
    /// </summary>
    public class LcdPoint
    {
        /// <summary>
        /// X座標
        /// </summary>
        public double X;
        /// <summary>
        /// Y座標
        /// </summary>
        public double Y;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LcdPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        /// <inheritdoc/>
        public override string ToString() => $"{X} {Y}";
    }
}
