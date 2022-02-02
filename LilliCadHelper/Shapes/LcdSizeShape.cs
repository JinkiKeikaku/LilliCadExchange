
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
            var param = sr.ReadParameters();
            for (int i = 0; i < 5; i++)
            {
                Points[i] = param.GetPoint();
            }
            ReadSizeParam(param, sr);
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("SIZE");
            sw.Write($"\t{Points[0].ToLcdString()} {Points[1].ToLcdString()} ");
            sw.Write($"{Points[2].ToLcdString()} {Points[3].ToLcdString()} ");
            sw.Write($"{Points[4].ToLcdString()} ");
            WriteSizeParam(sw);
        }
    }
}