using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// スプライン
    /// </summary>
    public class LcdSplineShape : LcdShape{
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
        /// <summary>
        /// 頂点リスト
        /// </summary>
        public List<LcdPoint> Points { get; set;  } = new();
        /// <inheritdoc/>
        public override string ToString() {
            return $"Spline(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}  Arrow0{StartArrow} Arrow1{EndArrow}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
            //LilliCad Ver1.4.8zまでは面色を保存していなかったので以下の処理となる。
            if (!param.IsEndOfLine)   FaceColor = param.GetFaceColor();
            Points = sr.ReadPoints();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("SPLINE");
            sw.WriteParamLine(LineStyle, StartArrow, EndArrow, FaceColor);
            sw.WritePoints(Points);
        }
    }
}
