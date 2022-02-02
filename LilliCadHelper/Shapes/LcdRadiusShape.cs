using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 半径寸法図形
    /// </summary>
    public class LcdRadiusShape : LcdSizeShapeBase {
        /// <summary>
        /// 寸法線始点（円の中心）
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }
        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }
        /// <summary>
        /// 文字位置（P0からの距離）
        /// </summary>
        public double TR { get; set; }

        /// <inheritdoc/>
        public override string ToString() {
            return $"Radius(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }

        /// <inheritdoc/>
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            Angle = param.GetDouble();
            TR = param.GetDouble();
            ReadSizeParam(param, sr);
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("RADIUS");
            sw.WriteParamLine(P0, Radius, Angle, TR, LineStyle, Flag, TextColor, FaceColor);
            sw.WriteParamLine(FontName);
            sw.WriteParamLine(FontHeight, SizeStyle, Arrow);
            sw.WriteString(Text);
        }
    }
}
