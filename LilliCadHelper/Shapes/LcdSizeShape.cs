
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 寸法図形
    /// </summary>
    public class LcdSizeShape : LcdSizeShapeBase
    {
        /// <summary>
        /// P[0],P[1] 引き出し線位置
        /// P[2],P[3] 寸法線位置
        /// P[4] 文字配置点（文字の中下基準）
        /// </summary>
        public LcdPoint[] Points { get; } = new LcdPoint[5];
        
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Size(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }

        /// <inheritdoc/>
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            for (int i = 0; i < 5; i++)
            {
                Points[i] = param.GetPoint();
            }
            ReadSizeParam(param, sr);
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("SIZE");
            sw.WriteParamLine(Points[0], Points[1], Points[2], Points[3], Points[4], LineStyle, Flag, TextColor, FaceColor);
            sw.WriteParamLine(FontName);
            sw.WriteParamLine(FontHeight, SizeStyle, Arrow);
            sw.WriteString(Text);
        }
    }
}