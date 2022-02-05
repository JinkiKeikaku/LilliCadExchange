using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// １行のテキスト
    /// </summary>
    public class LcdTextShape : LcdShape{
        /// <summary>
        /// テキスト配置位置
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// テキストの表示領域幅（回転前の文字列の表示領域サイズ）。
        /// LilliCadでは参考に値を出力しているため読み込み時に使用していない。
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// テキストの表示領域高さ（回転前の文字列の表示領域サイズ）。
        /// TextBasisに関係なく左上基準で常にマイナス。
        /// LilliCadでは参考に値を出力しているため読み込み時に使用していない。
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// フォントサイズ
        /// </summary>
        public double FontHeight { get; set; }
        /// <summary>
        /// フォント幅
        /// </summary>
        public double FontWidth { get; set; }
        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }
        /// <summary>
        /// テキストスタイル。以下のフラグのOR。
        /// <para>1：イタリック　2：ボールド　4：下線　8：取り消し線​ 64：枠線　128：縦書き</para>
        /// </summary>
        public int TextStyle { get; set; }
        /// <summary>
        /// テキスト配置基準
        /// <para>0：左上　1：中上　2：右上</para>
        /// <para>4：左中　5：中央　6：右中</para>
        /// <para>8：左下　9：中下　10：右下</para>
        /// </summary>
        public int TextBasis { get; set; }
        /// <summary>
        /// 文字色
        /// </summary>
        public int TextColor { get; set; }
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        /// <summary>
        /// フォント名
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// 文字列
        /// </summary>
        public string Text { get; set; }

        /// <inheritdoc/>
        public override string ToString() {
            return $"Text(P0{P0} TS({TextStyle}) TEXT({Text}) FH({FontHeight}) Angle({Angle}) FN({FontName}) ";
        }

        /// <inheritdoc/>
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Width = param.GetDouble();
            Height = param.GetDouble();
            FontHeight = param.GetDouble();
            FontWidth = param.GetDouble();
            Angle = param.GetDouble();
            TextStyle = param.GetInt();
            TextBasis = param.GetInt();
            TextColor = param.GetInt();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            FontName = sr.ReadSingleString();
            Text = sr.ReadString();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("TEXT");
            sw.WriteParamLine(P0, Width, Height, FontHeight, FontWidth, Angle,
                TextStyle, TextBasis, TextColor, LineStyle, FaceColor
            );
            sw.WriteParamLine(FontName);
            sw.WriteString(Text);
        }

    }
}
