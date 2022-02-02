
namespace LilliCadHelper.Shapes
{
    /// <summary>
    /// 寸法値図形のスタイル。図形によっては使用しないメンバもあるので注意。
    /// </summary>
    public class LcdSizeStyle {
        /// <summary>
        /// 引出位置の離れ
        /// </summary>
        public float Linegap { get; set; }
        /// <summary>
        /// 寸法線のはみ出し
        /// </summary>
        public float Linejut { get; set; }
        /// <summary>
        /// 引き出し線のはみ出し
        /// </summary>
        public float Linedrop { get; set; }
        /// <summary>
        /// 寸法値文字の寸法線からの離れ
        /// </summary>
        public float Textgap { get; set; }

        override public string ToString() => $"{Linegap} {Linejut} {Linedrop} {Textgap}";

    }
}
