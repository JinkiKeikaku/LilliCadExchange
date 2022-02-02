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
        }
        void WritePaperSection(LcdStreamWriter sw)
        {
            sw.WriteLine("[PAPER]");
            sw.WriteParameterLine(Header.PaperName);
            sw.WriteParameterLine(Header.PaperInfo);
            sw.WriteParameterLine(Header.PaperWidth, Header.PaperHeight);
            sw.WriteParameterLine(Header.PaperScaleName);
            sw.WriteParameterLine(Header.PaperScale);
            sw.WriteParameterLine(Header.IsPaperHorizontal, Header.PaperOriginFlag);
            //sw.WriteLine($"\t{Header.PaperName}");
            //sw.WriteLine($"\t{Header.PaperInfo}");
            //sw.WriteLine($"\t{Header.PaperWidth} {Header.PaperHeight}");
            //sw.WriteLine($"\t{Header.PaperScaleName}");
            //sw.WriteLine($"\t{Header.PaperScale}");
            //sw.WriteLine($"\t{BooleanToInt(Header.IsPaperHorizontal)} {Header.PaperOriginFlag}");
        }
        void WriteOriginSection(StreamWriter sw)
        {
            sw.WriteLine("[ORIGIN]");
            sw.WriteLine($"\t{Header.GridOrigin.ToLcdString()}");
        }
        void WriteGridSection(StreamWriter sw)
        {
            sw.WriteLine("[GRID]");
            sw.WriteLine($"\t{Header.GridSpaceX} {Header.GridSpaceY}");
        }
        void WriteLayersSection(StreamWriter sw)
        {
            sw.WriteLine("[LAYERS]");
            sw.WriteLine($"\t{Header.SelectedLayer}");
            sw.WriteLine($"\t{Layers.Count}");
        }
        void WriteLayers(StreamWriter sw)
        {
            foreach(var layer in Layers)
            {
                sw.WriteLine("[LAYER]");
                sw.WriteLine($"\t{layer.Name}");
                sw.WriteLine($"\t{(int)layer.Flag}");
                sw.WriteLine($"\t{layer.Shapes.Count}");
                foreach(var s in layer.Shapes)
                {
                    s.Write(sw);
                }
            }
        }
        int BooleanToInt(bool flag) => flag ? 1 : 0;

    }
}
