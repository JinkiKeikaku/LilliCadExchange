using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 円弧
    /// </summary>
    public class LcdArcShape : LcdShape{
        /// <summary>
        /// 中心
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
        /// <summary>
        /// 始点矢印
        /// </summary>
        public double StartAngleRad { get; set; }
        /// <summary>
        /// 終点矢印
        /// </summary>
        public double EndAngleRad { get; set; }
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        /// <summary>
        /// 始点矢印
        /// </summary>
        public LcdArrowStyle StartArrow { get; set; } = new();
        /// <summary>
        /// 終点矢印
        /// </summary>
        public LcdArrowStyle EndArrow { get; set; } = new();
        /// <inheritdoc/>
        public override string ToString() {
            return $"Arc(P0{P0} R({Radius}) Angle({StartAngleRad}, EndAngle)LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            StartAngleRad = param.GetDouble();
            EndAngleRad = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("ARC");
            sw.WriteParamLine(P0, Radius, StartAngleRad, EndAngleRad, LineStyle, FaceColor, StartArrow, EndArrow);
        }
    }
}
