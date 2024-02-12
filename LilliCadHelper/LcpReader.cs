using LilliCadHelper.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper
{
    /// <summary>
    /// lcpファイル読み込みクラス
    /// </summary>
    public class LcpReader
    {
        /// <summary>
        /// パーツファイルバージョン（2のみ）
        /// </summary>
        public int LcpVersion { get; set; } = 2;
        /// <summary>
        /// パーツリスト名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// パーツリスト
        /// </summary>
        public List<LcdParts> PartsList { get; set; } = new List<LcdParts>();

        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        public void Read(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var sr = new StreamReader(path, Encoding.GetEncoding("shift_jis"));
            var sb = new StringBuilder();
            var line = sr.ReadLine();
            if (line != "$$LilliCadPart$$") throw new Exception("Not LilliCad parts format");
            line = sr.ReadLine();
            LcpVersion = int.Parse(line);
            var lcdSr = new LcdStreamReader(sr);
            string sec = "";
            bool isEof = false;
            while (!lcdSr.IsEndOfStream && !isEof)
            {
                line = lcdSr.ReadLine();
                if (line[0] == '[')
                {
                    var i = line.IndexOf(']');
                    if (i < 0) throw new Exception("Section name is not closed ']'");
                    sec = line[1..i];
                }
                switch (sec)
                {
                    case "NAME":
                        {
                            Name = lcdSr.ReadSingleString();
                        }
                        break;
                    case "SIZE":
                        {
                            //not use
                            var size = lcdSr.GetParameters().GetInt();
                        }
                        break;
                    case "PARTS":
                        {
                            var parts = new LcdParts(); 
                            parts.Name= lcdSr.ReadSingleString();
                            var param = lcdSr.GetParameters();
                            parts.IsEnableScale=param.GetInt() == 1;    
                            parts.Scale = param.GetDouble();
                            PartsList.Add(parts);
                            while (!lcdSr.IsEndOfStream)
                            {
                                if (Char.IsLetter(lcdSr.Peek()))
                                {
                                    //行頭が英字なら図形
                                    var shape = LcdShapeManager.CreateShape(lcdSr);
                                    switch (shape){
                                        case LcdGroupShape g:
                                            parts.Group = g;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    if (lcdSr.Peek() == '[') break;    //行頭が[で次のセクション開始
                                    lcdSr.ReadLine();//行読み飛ばし
                                }
                            }
                        }
                        break;
                    case "EOF":
                        isEof = true;
                        break;
                    default:
                        lcdSr.ReadLine();//行読み飛ばし
                        break;
                }
                sec = "";
            }
            sr.Close();
        }
    }
}
