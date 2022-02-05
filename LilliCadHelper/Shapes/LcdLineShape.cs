using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 直線
    /// </summary>
    public class LcdLineShape : LcdShape
    {
        /// <summary>
        /// 始点
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 終点
        /// </summary>
        public LcdPoint P1 { get; set; }
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 始点矢印
        /// </summary>
        public LcdArrowStyle StartArrow { get; set; } = new();
        /// <summary>
        /// 終点矢印
        /// </summary>
        public LcdArrowStyle EndArrow { get; set; } = new();
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Line(P0{P0} P1{P1} LineStyle{LineStyle} Arrow0{StartArrow} Arrow1{EndArrow}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            P1 = param.GetPoint();
            LineStyle = param.GetLineStyle();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
        }
        internal override void Write(LcdStreamWriter sw) 
        {
            sw.WriteLine("LINE");
            sw.WriteParamLine(P0,P1, LineStyle, StartArrow, EndArrow);
        }

    }
}
