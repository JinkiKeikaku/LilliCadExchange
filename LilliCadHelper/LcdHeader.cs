using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper
{
    /// <summary>
    /// Lcdヘッダー
    /// </summary>
    public class LcdHeader
    {
        /// <summary>
        /// lcdファイルのバージョン。現在は1のみ。
        /// </summary>
        public int LcdVersion { get; set; } = 1;
        /// <summary>用紙名。</summary>
        public string PaperName { get; set; } = "A3";
        /// <summary>用紙情報。</summary>
        public string PaperInfo { get; set; } = "420mmX297";
        /// <summary>用紙幅。</summary>
        public double PaperWidth { get; set; } = 420;
        /// <summary>用紙高さ。</summary>
        public double PaperHeight { get; set; } = 297;
        /// <summary>縮尺名。</summary>
        public string PaperScaleName { get; set; } = "1:1";
        /// <summary>縮尺。</summary>
        public double PaperScale { get; set; } = 1.0;
        /// <summary>用紙横向き。</summary>
        public bool IsPaperHorizontal { get; set; } = true;
        /// <summary>
        /// 用紙原点フラグ。
        /// <para>0：左上　1：中上　2：右上　3：左中　4：中央　5：右中　6：左下（標準）7：中下　8：右下</para>
        /// </summary>
        public int PaperOriginFlag { get; set; } = 6;
        /// <summary>グリッド原点</summary>
        public LcdPoint GridOrigin { get; set; } = new LcdPoint(0, 0);
        /// <summary>グリッド間隔（実寸）横</summary>
        public double GridSpaceX { get; set; } = 10.0;
        /// <summary>グリッド間隔（実寸）縦</summary>
        public double GridSpaceY { get; set; } = 10.0;
        /// <summary>選択中レイヤ番号</summary>
        public int SelectedLayer { get; set; } = 0;
        /// <summary>総レイヤ数</summary>
        public int NumLayer { get; set; } = 1;

        public LcdHeader() { }





    }
}
