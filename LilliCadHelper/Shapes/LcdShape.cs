
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace LilliCadHelper.Shapes
{
    public abstract class LcdShape
    {
        internal abstract void Read(LcdStreamReader sr);
        internal abstract void Write(LcdStreamWriter sw);

        /// <summary>
        /// 破線のパターン。0は実線なのでこのパターンは使わないこと。
        /// また、8は仮線用のパターン。仮線はLineTypeのBit7をたてるため、コードは8+128=136となる。
        /// </summary>
        public static float[][] DotPattern { get; } = new float[][] {
            new float[]{0},
            new float[]{1.25f,1.25f},
            new float[]{2.5f,2.5f},
            new float[]{3.75f,1.25f},
            new float[]{3.75f,1.25f, 1.25f,1.25f},
            new float[]{6.25f,2.5f, 2.5f,2.5f},
            new float[]{3.25f,1.25f, 1.25f,1.25f, 1.25f,1.25f},
            new float[]{8.0f,2.5f, 1.25f,2.5f, 1.25f,2.5f},
            new float[]{0.625f, 1.875f},
        };

        /// <summary>
        /// 透明色
        /// </summary>
        public const int LILLICAD_NULL_COLOR = 0x1000000;//(RGB(255,255,255) + 1)

        ///// <summary>
        ///// 文字スタイルフラグ
        ///// </summary>
        //[Flags]
        //public enum TextStyle
        //{
        //    /// <summary>
        //    /// なし
        //    /// </summary>
        //    None = 0,
        //    /// <summary>
        //    /// イタリック
        //    /// </summary>
        //    Italic = 1,
        //    /// <summary>
        //    /// ボールド
        //    /// </summary>
        //    Bold = 2,
        //    /// <summary>
        //    /// 下線
        //    /// </summary>
        //    Underline = 4,
        //    /// <summary>
        //    /// 取り消し線
        //    /// </summary>
        //    Strikeout,
        //    /// <summary>
        //    /// 枠線
        //    /// </summary>
        //    Frameline = 64,
        //    /// <summary>
        //    /// 縦書き
        //    /// </summary>
        //    Vertical = 128,
        //}
        /*
            /// <summary>
            /// 文字スタイル　イタリック
            /// </summary>
            public const int TextStyle_Italic = 1;
                /// <summary>
                /// 文字スタイル　ボールド
                /// </summary>
                public const int Bold = 2;
                /// <summary>
                /// 文字スタイル　下線
                /// </summary>
                public const int Underline = 4;
                /// <summary>
                /// 文字スタイル　取り消し線
                /// </summary>
                public const int Strikeout = 8;
                /// <summary>
                /// 文字スタイル　枠線
                /// </summary>
                public const int Frameline = 64;
                /// <summary>
                /// 文字スタイル　縦書き
                /// </summary>
                public const int Vertical = 128;
        */
        ///// <summary>
        ///// 文字配置基準
        ///// </summary>
        //public enum TextBasis
        //{
        //    /// <summary>
        //    /// 文字基準左上
        //    /// </summary>
        //    TopLeft = 0,
        //    /// <summary>
        //    /// 文字基準中上
        //    /// </summary>
        //    TopCenter = 1,
        //    /// <summary>
        //    /// 文字基準右上
        //    /// </summary>
        //    TopRight = 2,
        //    /// <summary>
        //    /// 文字基準左中
        //    /// </summary>
        //    CenterLeft = 4,
        //    /// <summary>
        //    /// 文字基準中央
        //    /// </summary>
        //    Center = 5,
        //    /// <summary>
        //    /// 文字基準右中
        //    /// </summary>
        //    CenterRight = 6,
        //    /// <summary>
        //    /// 文字基準左下
        //    /// </summary>
        //    BottomLeft = 8,
        //    /// <summary>
        //    /// 文字基準中下
        //    /// </summary>
        //    BottomCenter = 9,
        //    /// <summary>
        //    /// 文字基準右下
        //    /// </summary>
        //    BottomRight = 10,
        //}
        ///// <summary>
        ///// 文字揃え
        ///// </summary>
        //public enum TextAlign
        //{
        //    /// <summary>
        //    /// 文字基準左上
        //    /// </summary>
        //    TopLeft = 0,
        //    /// <summary>
        //    /// 文字基準中上
        //    /// </summary>
        //    TopCenter = 1,
        //    /// <summary>
        //    /// 文字基準右上
        //    /// </summary>
        //    TopRight = 2,
        //    /// <summary>
        //    /// 文字基準左中
        //    /// </summary>
        //    CenterLeft = 4,
        //    /// <summary>
        //    /// 文字基準中央
        //    /// </summary>
        //    Center = 5,
        //    /// <summary>
        //    /// 文字基準右中
        //    /// </summary>
        //    CenterRight = 6,
        //    /// <summary>
        //    /// 文字基準左下
        //    /// </summary>
        //    BottomLeft = 8,
        //    /// <summary>
        //    /// 文字基準中下
        //    /// </summary>
        //    BottomCenter = 9,
        //    /// <summary>
        //    /// 文字基準右下
        //    /// </summary>
        //    BottomRight = 10,
        //}

                ///// <summary>
                ///// 文字基準左上
                ///// </summary>
                //public const int TextBasis_TopLeft = 0;
                ///// <summary>
                ///// 文字基準中上
                ///// </summary>
                //public const int TextBasis_TopCenter = 1;
                ///// <summary>
                ///// 文字基準右上
                ///// </summary>
                //public const int TextBasis_TopRight = 2;
                ///// <summary>
                ///// 文字基準左中
                ///// </summary>
                //public const int TextBasis_CenterLeft = 4;
                ///// <summary>
                ///// 文字基準中央
                ///// </summary>
                //public const int TextBasis_Center = 5;
                ///// <summary>
                ///// 文字基準右中
                ///// </summary>
                //public const int TextBasis_CenterRight = 6;
                ///// <summary>
                ///// 文字基準左下
                ///// </summary>
                //public const int TextBasis_BottomLeft = 8;
                ///// <summary>
                ///// 文字基準中下
                ///// </summary>
                //public const int TextBasis_BottomCenter = 9;
                ///// <summary>
                ///// 文字基準右下
                ///// </summary>
                //public const int TextBasis_BottomRight = 10;
    }
}
