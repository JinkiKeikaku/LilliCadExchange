using System;
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 楕円
    /// </summary>
    public class LcdEllipseShape : LcdShape{
        /// <summary>
        /// 中心
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 横直径
        /// </summary>
        public double RX { get; set; }
        /// <summary>
        /// 縦直径
        /// </summary>
        public double RY { get; set; }
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        public override string ToString() {
            return $"Ellipse(P0{P0} RX({RX}) RY({RY}) LineStyle{LineStyle} FaceColor{FaceColor}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            RX = param.GetDouble();
            RY = param.GetDouble();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("ELLIPSE");
            sw.WriteParamLine(P0,RX,RY, LineStyle, FaceColor);
        }
    }
}
