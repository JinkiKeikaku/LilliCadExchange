using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 角度寸法図形
    /// </summary>
    public class LcdAngleShape :  LcdSizeShapeBase{

        /// <summary>
        /// 中心
        /// </summary>
        public LcdPoint P0 { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// 開始角度、終了角度、文字位置の角度
        /// </summary>
        public double[] Angles { get; } = new double[3];

        /// <inheritdoc/>
        public override string ToString() {
            return $"Angle(TEXT({Text}) FH({FontHeight}) FN({FontName}) ";
        }

        /// <inheritdoc/>
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
            for (int i = 0; i < 3; i++)
            {
                Angles[i] = param.GetDouble();
            }
            ReadSizeParam(param, sr);
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("ANGLE");
            sw.Write($"\t{P0.ToLcdString()} {Radius} {Angles[0]} {Angles[1]} {Angles[2]} ");
            WriteSizeParam(sw);
        }


    }
}
