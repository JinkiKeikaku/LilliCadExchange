using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper.Shapes
{
    public class LcdGroupShape : LcdShape
    {
        /// <summary>
        /// 基準点
        /// </summary>
        public LcdPoint P0 { get; set; }
        /// <summary>
        /// 基準点フラグ　false：基準点無効　true：基準点有効
        /// </summary>
        public bool IsBasisEnable { get; set; }
        /// <summary>
        /// 図形
        /// </summary>
        public List<LcdShape> Shapes { get; } = new();

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            var n = param.GetInt();
            param = sr.ReadParameters();
            IsBasisEnable = param.GetInt() == 1;
            P0 = param.GetPoint();
            for(var i = 0; i < n; i++)
            {
                var s = LcdShapeManager.CreateShape(sr);
                if (s != null) Shapes.Add(s);
            }
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("GROUP");
            sw.WriteLine($"\t{Shapes.Count}");
            var bf = IsBasisEnable ? 1 : 0;
            sw.WriteLine($"\t{bf} {P0.ToLcdString()}");
            foreach(var s in Shapes)
            {
                s.Write(sw);
            }
        }


    }
}
