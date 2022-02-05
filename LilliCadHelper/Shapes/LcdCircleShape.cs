
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 円
    /// </summary>
    public class LcdCircleShape : LcdShape{
        /// <summary>
        /// 中心
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
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
            return $"Circle(P0{P0} R({Radius}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("CIRCLE");
            sw.WriteParamLine(P0,Radius, LineStyle, FaceColor);
        }
    }
}
