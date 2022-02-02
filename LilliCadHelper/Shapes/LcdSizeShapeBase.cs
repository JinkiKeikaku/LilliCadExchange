
using System.IO;

namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 寸法図形の基本クラス
    /// </summary>
    public abstract class LcdSizeShapeBase : LcdShape{
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; }
        /// <summary>
        /// フラグ　Bit0：1　寸法値を自動記入しない（文字列使用）
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 文字色
        /// </summary>
        public int TextColor { get; set; }
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; }
        /// <summary>
        /// フォント名
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// フォントサイズ
        /// </summary>
        public double FontHeight { get; set; }
        /// <summary>
        /// 寸法値スタイル
        /// </summary>
        public LcdSizeStyle SizeStyle { get; set; }
        /// <summary>
        /// 矢印スタイル
        /// </summary>
        public LcdArrowStyle Arrow { get; set; }
        /// <summary>
        /// 文字列。保存時に図形に表示されていた文字列が入っています。 
        /// FフラグのBit0が0の場合は自動寸法ですが、読み込み時にTEXTの値を表示するか新たに寸法値を表示するかは
        /// アプリの仕様になります。
        /// 自動寸法では少数点以下の桁数などが変わる可能性があるため、読み込み時はTEXTの文字列を利用したほうがいいかもしれません。
        /// </summary>
        public string Text { get; set; }

        internal void ReadSizeParam(LcdStreamReader.Parameters param, LcdStreamReader sr)
        {
            LineStyle = param.GetLineStyle();
            Flag = param.GetInt();
            TextColor = param.GetInt();
            FaceColor = param.GetFaceColor();
            FontName = sr.ReadSingleString();
            param = sr.ReadParameters();
            FontHeight = param.GetDouble();
            SizeStyle = param.GetSizeStyle();
            Arrow = param.GetArrowStyle();
            Text = sr.ReadString();
        }
        internal void WriteSizeParam(StreamWriter sw)
        {
            sw.WriteLine($"{LineStyle.ToLcdString()} {Flag} {TextColor} {FaceColor.ToLcdString()} ");
            sw.WriteLine($"\t{FontName}");
            sw.WriteLine($"\t{FontHeight} {SizeStyle.ToLcdString()} {Arrow.ToLcdString()}");
            WriteString(sw, Text);
        }
    }
}
