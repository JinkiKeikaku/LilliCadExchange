using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper
{
    public class LcdWriter
    {
        /// <summary>
        /// lcdヘッダー
        /// </summary>
        public LcdHeader Header { get; } = new();
        /// <summary>
        /// レイヤリスト
        /// </summary>
        public List<LcdLayer> Layers { get; } = new();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LcdWriter()
        {
        }

        public void Write(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var s = new StreamWriter(path, false, Encoding.GetEncoding("shift_jis"));
            var sw = new LcdStreamWriter(s);

            sw.WriteLine("$$LilliCadText$$");
            sw.WriteLine($"{Header.LcdVersion}");
            WritePaperSection(sw);
            WriteOriginSection(sw);
            WriteGridSection(sw);
            WriteLayersSection(sw);
            WriteLayers(sw);
            sw.WriteLine("[EOF]");
            s.Close();
        }
        void WritePaperSection(LcdStreamWriter sw)
        {
            sw.WriteLine("[PAPER]");
            sw.WriteParamLine(Header.PaperName);
            sw.WriteParamLine(Header.PaperInfo);
            sw.WriteParamLine(Header.PaperWidth, Header.PaperHeight);
            sw.WriteParamLine(Header.PaperScaleName);
            sw.WriteParamLine(Header.PaperScale);
            sw.WriteParamLine(Header.IsPaperHorizontal, Header.PaperOriginFlag);
        }
        void WriteOriginSection(LcdStreamWriter sw)
        {
            sw.WriteLine("[ORIGIN]");
            sw.WriteParamLine(Header.GridOrigin);
        }
        void WriteGridSection(LcdStreamWriter sw)
        {
            sw.WriteLine("[GRID]");
            sw.WriteParamLine(Header.GridSpaceX, Header.GridSpaceY);
        }
        void WriteLayersSection(LcdStreamWriter sw)
        {
            sw.WriteLine("[LAYERS]");
            sw.WriteParamLine(Header.SelectedLayer);
            sw.WriteParamLine(Layers.Count);
        }
        void WriteLayers(LcdStreamWriter sw)
        {
            foreach(var layer in Layers)
            {
                sw.WriteLine("[LAYER]");
                sw.WriteParamLine(layer.Name);
                sw.WriteParamLine((int)layer.Flag);
                sw.WriteParamLine(layer.Shapes.Count);
                foreach(var s in layer.Shapes)
                {
                    s.Write(sw);
                }
            }
        }
    }
}
