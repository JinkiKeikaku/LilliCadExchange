using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// バルーン
    /// </summary>
    public class LcdBalloonShape : LcdShape{

        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 矢印スタイル
        /// </summary>
        public LcdArrowStyle Arrow { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        /// <summary>
        /// 円の半径の最小値。実寸（用紙寸ではない）ことに注意してください。0の場合自動設定（文字列のサイズから決める）
        /// </summary>
        public double RMin { get; set; }
        /// <summary>
        /// 円の半径の最大値。実寸（用紙寸ではない）ことに注意してください。0の場合自動設定（文字列のサイズから決める）
        /// </summary>
        public double RMax { get; set; }
        /// <summary>
        /// 文字
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// フォント名
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// フォントサイズ。実寸（用紙寸ではない）ことに注意してください。
        /// </summary>
        public double FontHeight { get; set; }
        /// <summary>
        /// テキストスタイル。以下のフラグのOR。
        /// <para>1：イタリック　2：ボールド　4：下線　8：取り消し線​ 64：枠線　128：縦書き</para>
        /// </summary>
        public int TextStyle { get; set; }
        /// <summary>
        /// 文字色
        /// </summary>
        public int TextColor { get; set; }
        /// <summary>
        /// 頂点
        /// </summary>
        public List<LcdPoint> Points { get; set; } = new();
        public override string ToString() {
            return $"Balloon(Points.Count({Points.Count}) TEXT({Text}) FH({FontHeight}) FN({FontName}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            Arrow = param.GetArrowStyle();
            FaceColor = param.GetFaceColor();
            param = sr.GetParameters();
            RMin = param.GetDouble();
            RMax = param.GetDouble();
            Text = sr.ReadString();
            FontName = sr.ReadString();    //ReadSingleString()ではない
            param = sr.GetParameters();
            FontHeight = param.GetDouble();
            TextStyle = param.GetInt();
            TextColor = param.GetInt();
            Points = sr.ReadPoints();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("BALLOON");
            sw.WriteParamLine(LineStyle,Arrow,FaceColor);
            sw.WriteParamLine(RMin,RMax);
            sw.WriteString(Text);
            sw.WriteString(FontName);
            sw.WriteParamLine(FontHeight,TextStyle,TextColor);
            sw.WritePoints(Points);
        }

    }
}
