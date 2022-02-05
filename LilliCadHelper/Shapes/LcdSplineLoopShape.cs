using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 閉じたスプライン
    /// </summary>
    public class LcdSplineLoopShape  : LcdShape{
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        /// <summary>
        /// 頂点リスト
        /// </summary>
        public List<LcdPoint> Points { get; set; } = new();
        /// <inheritdoc/>
        public override string ToString() {
            return $"SplineLoop(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            Points = sr.ReadPoints();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("SPLINELOOP");
            sw.WriteParamLine(LineStyle, FaceColor);
            sw.WritePoints(Points);
        }
    }
}
