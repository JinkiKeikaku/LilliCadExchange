using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 引き出し線
    /// </summary>
    public class LcdLabelShape : LcdShape{
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; }
        /// <summary>
        /// 矢印スタイル
        /// </summary>
        public LcdArrowStyle Arrow { get; set; }
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; }
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
        /// 文字の線からの離れ。実寸（用紙寸ではない）ことに注意してください。
        /// </summary>
        public double TextGap { get; set; }
        /// <summary>
        /// 文字色
        /// </summary>
        public int TextColor { get; set; }
        /// <summary>
        /// 頂点
        /// </summary>
        public List<LcdPoint> Points { get; set; } = new();
        /// <inheritdoc/>
        public override string ToString() {
            return $"Label(Points.Count({Points.Count}) TEXT({Text}) FH({FontHeight}) FN({FontName}";
        }
        /// <inheritdoc/>
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            Arrow = param.GetArrowStyle();
            FaceColor = param.GetFaceColor();
            Text = sr.ReadString();
            FontName = sr.ReadString();    //ReadSingleString()ではない
            param = sr.GetParameters();
            FontHeight = param.GetDouble();
            TextStyle = param.GetInt();
            TextGap = param.GetDouble();
            TextColor = param.GetInt();
            Points = sr.ReadPoints();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("LABEL");
            sw.WriteParamLine(LineStyle,Arrow,FaceColor);
            sw.WriteString(Text);
            sw.WriteString(FontName);
            sw.WriteParamLine(FontHeight,TextStyle,TextGap,TextColor);
            sw.WritePoints(Points);
        }
    }
}
