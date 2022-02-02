using LilliCadHelper.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper
{
    public class LcdLayer
    {
        /// <summary>
        /// レイヤフラグ
        /// </summary>
        [Flags]
        public enum LayerFlag
        {
            /// <summary>
            /// 表示可。
            /// </summary>
            Visible=1,
            /// <summary>
            /// 印刷可
            /// </summary>
            Printable=2,
            /// <summary>
            /// 他のレイヤが選択中に図形が選択できるか？
            /// </summary>
            Selectable = 4,   
        }
        public string Name { get; set; }
        public LayerFlag Flag { get; set; }
        public List<LcdShape> Shapes { get; } = new();
    }
}
