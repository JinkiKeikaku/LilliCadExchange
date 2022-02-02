using LilliCadHelper.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LilliCadHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class LcdReader
    {
        /// <summary>
        /// 読み込み完了時に呼ばれるコールバック
        /// </summary>
        /// <param name="reader"></param>
        public delegate void CompletedCallback(LcdReader reader);
        /// <summary>
        /// lcdヘッダー
        /// </summary>
        public LcdHeader Header { get; set; } = new();
        /// <summary>
        /// レイヤリスト
        /// </summary>
        public List<LcdLayer> Layers { get; } = new();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LcdReader()
        {
        }
        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <param name="callback">読み込み完了時のコールバック</param>
        public void Read(string path, CompletedCallback callback)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var sr = new StreamReader(path, Encoding.GetEncoding("shift_jis"));
            var sb = new StringBuilder();
            var line = sr.ReadLine();
            if (line != "$$LilliCadText$$") throw new Exception("Not LilliCad format");
            line = sr.ReadLine();
            Header.LcdVersion = int.Parse(line);
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
                    case "PAPER":
                        {
                            ParsePaperInfo(lcdSr);
                        }
                        break;
                    case "ORIGIN":
                        {
                            ParseOrigin(lcdSr);
                        }
                        break;
                    case "GRID":
                        {
                            ParseGrid(lcdSr);
                        }
                        break;
                    case "TOOL":
                        {
                            //TOOLセクション。ツールの共通設定です。LilliCad以外では使いません。読み飛ばします。
                        }
                        break;
                    case "TOOLS":
                        //TOOLSセクション。各ツールの設定値が入っています。LilliCad以外では使いません。読み飛ばします。
                        break;
                    case "LAYERS":
                        {
                            Header.SelectedLayer = lcdSr.GetParameters().GetInt();// 選択されていたレイヤー
                            Header.NumLayer = lcdSr.GetParameters().GetInt();//レイヤーがいくつあるか
                        }
                        break;
                    case "LAYER":
                        {
                            var layer = new LcdLayer();
                            layer.Name = lcdSr.ReadSingleString();
                            int flag = lcdSr.GetParameters().GetInt();
                            layer.Flag = (LcdLayer.LayerFlag)flag;
                            int numShape = lcdSr.GetParameters().GetInt();
                            Layers.Add(layer);
                            while (!lcdSr.IsEndOfStream)
                            {
                                if (Char.IsLetter(lcdSr.Peek()))
                                {
                                    //行頭が英字なら図形
                                    var shape = LcdShapeManager.CreateShape(lcdSr);
                                    if (shape != null) layer.Shapes.Add(shape);
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
            callback?.Invoke(this);
            sr.Close();
        }


        //internal LcdShape ReadShape(LcdStreamReader sr)
        //{
        //    return LcdShapeManager.CreateShape(sr);
        //}
        void ParsePaperInfo(LcdStreamReader lcdSr)
        {
            Header.PaperName = lcdSr.ReadSingleString();
            Header.PaperInfo = lcdSr.ReadSingleString();
            var param = lcdSr.GetParameters();
            Header.PaperWidth = param.GetDouble();
            Header.PaperHeight = param.GetDouble();
            Header.PaperScaleName = lcdSr.ReadSingleString();
            param = lcdSr.GetParameters();
            Header.PaperScale = param.GetDouble();
            param = lcdSr.GetParameters();
            Header.IsPaperHorizontal = param.GetInt() != 0;
            Header.PaperOriginFlag = param.GetInt();
        }
        void ParseOrigin(LcdStreamReader lcdSr)
        {
            Header.GridOrigin = lcdSr.GetParameters().GetPoint();
        }

        void ParseGrid(LcdStreamReader lcdSr)
        {
            var param = lcdSr.GetParameters();
            Header.GridSpaceX = param.GetDouble();
            Header.GridSpaceY = param.GetDouble();
        }
    }
}
