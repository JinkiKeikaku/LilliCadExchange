using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 四角形
    /// </summary>
    public class LcdRectShape : LcdShape{
        /// <summary>
        /// 頂点（左下）
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 幅
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 高さ
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        /// <inheritdoc/>
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
