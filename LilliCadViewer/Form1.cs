using LilliCadViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LilliCadViewer
{
    public partial class Form1 : Form
    {
        DrawContext DrawContext;
        //        List<ICadShape> mShapes = new();
        LilliCadHelper.LcdHeader mHeader;
        List<LilliCadHelper.LcdLayer> mLayers;


        public Form1()
        {
            InitializeComponent();
            panel1.AutoScroll = true;
            this.panel1.MouseWheel += panel1_MouseWheel;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog();
            d.Filter = "lcd Files|*.lcd|All Files|*.*";
            if (d.ShowDialog() != DialogResult.OK) return;
            OpenFile(d.FileName);
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog();
            d.Filter = "lcd Files|*.lcd|All Files|*.*";
            if (d.ShowDialog() != DialogResult.OK) return;
            SaveFile(d.FileName);
        }

        private void OpenFile(String path)
        {
            try
            {
                if (Path.GetExtension(path) == ".lcd")
                {
                    //JwwReaderが読み込み用のクラス。
                    var reader = new LilliCadHelper.LcdReader();
                    //Completedは読み込み完了時に実行される関数。
                    reader.Read(path, Completed);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error");
                DrawContext = null;
                panel1.Invalidate();
            }
        }



        //dllでjwwファイル読み込み完了後に呼ばれます。
        private void Completed(LilliCadHelper.LcdReader reader)
        {
            mLayers = reader.Layers;
            mHeader = reader.Header;
            LayerNameToTextBox(reader);
            //var origin = GetPaperOrigin(reader.Header);
            //var cv = new LcdToShapeConverter(origin, reader.Header.PaperScale);
            //foreach (var layer in reader.Layers)
            //{
            //    foreach (var lcdShape in layer.Shapes)
            //    {
            //        var s = cv.Convert(lcdShape);
            //        if (s != null) mShapes.Add(s);
            //    }
            //}
            //DrawContextは表示する時に使う情報保持オブジェクト。
            DrawContext = new DrawContext(reader.Header);
            //スクロールバーなんかの設定。
            CalcSize();
            //panel1を無効化してpanel1のpaintが呼ばれる。
            panel1.Invalidate();
        }


        /// <summary>
        /// レイヤ名をテキストボックスに入れる。
        /// </summary>
        private void LayerNameToTextBox(LilliCadHelper.LcdReader reader)
        {
            var sb = new StringBuilder();
            foreach (var layer in reader.Layers)
            {
                sb.AppendLine($"{layer.Name}");
            }
            textBox1.Text = sb.ToString();
        }

        /// <summary>
        /// 図形の表示原点。Viewerの使用で用紙中央が原点のため、ちょっとややこしい。
        /// 例えば、原点フラグが6で左下の場合は(-width/2, -height/2)になる。
        /// 現状、LilliCadは6のみ使用で他はチェックできていない。
        /// </summary>
        private CadPoint GetPaperOrigin(LilliCadHelper.LcdHeader header)
        {
            (double width, double height) = header.IsPaperHorizontal ?
                (header.PaperWidth, header.PaperHeight) :
                (header.PaperHeight, header.PaperWidth);
            return header.PaperOriginFlag switch
            {
                0 => new CadPoint(-width / 2, height / 2),
                1 => new CadPoint(0, height / 2),
                2 => new CadPoint(width / 2, height / 2),
                3 => new CadPoint(-width / 2, 0),
                4 => new CadPoint(0, 0),
                5 => new CadPoint(width / 2, 0),
                6 => new CadPoint(-width / 2, -height / 2),
                7 => new CadPoint(0, -height / 2),
                8 => new CadPoint(width / 2, -height / 2),
                _ => new CadPoint(0, 0),
            };
        }


        private void SaveFile(String path)
        {
            try
            {
                var writer = new LilliCadHelper.LcdWriter();
                if(mHeader!= null && mLayers != null)
                {
                    //テストなので単純にコピーする。
                    writer.Header.PaperName = mHeader.PaperName;
                    writer.Header.PaperInfo = mHeader.PaperInfo;
                    writer.Header.PaperWidth = mHeader.PaperWidth;
                    writer.Header.PaperHeight = mHeader.PaperHeight;
                    writer.Header.PaperScaleName = mHeader.PaperScaleName;
                    writer.Header.PaperScale = mHeader.PaperScale;
                    writer.Header.IsPaperHorizontal = mHeader.IsPaperHorizontal;
                    writer.Header.PaperOriginFlag = mHeader.PaperOriginFlag;
                    writer.Header.GridOrigin = mHeader.GridOrigin;
                    writer.Header.GridSpaceX = mHeader.GridSpaceX;
                    writer.Header.GridSpaceY = mHeader.GridSpaceY;
                    writer.Header.NumLayer = mLayers.Count;
                    foreach(var srcLay in mLayers)
                    {
                        var dstLay = new LilliCadHelper.LcdLayer();
                        dstLay.Name = srcLay.Name;
                        dstLay.Flag = srcLay.Flag;
                        foreach(var s in srcLay.Shapes)
                        {
                            dstLay.Shapes.Add(s);
                        }
                        writer.Layers.Add(dstLay);
                    }
                }
                writer.Write(path);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }


        /// <summary>
        /// スクロールの設定
        /// </summary>
        private void CalcSize()
        {
            if (DrawContext == null) return;
            var ps = new Size((int)(DrawContext.PaperSize.Width * DrawContext.Scale), (int)(DrawContext.PaperSize.Height * DrawContext.Scale));
            panel1.AutoScrollMinSize = new Size((int)ps.Width, (int)ps.Height);
            panel1.AutoScrollPosition = new Point(
                Math.Max(0, (int)ps.Width / 2 - Width / 2),
                Math.Max(0, (int)ps.Height / 2 - Height / 2)
            );
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            if (DrawContext == null) return;
            var saved = g.Save();
            g.TranslateTransform(
                DrawContext.Scale * DrawContext.PaperSize.Width / 2 + panel1.AutoScrollPosition.X,
                DrawContext.Scale * DrawContext.PaperSize.Height / 2 + panel1.AutoScrollPosition.Y
            );
            g.ScaleTransform(DrawContext.Scale, DrawContext.Scale);
            var drawer = new LilliCadDrawer(mHeader, mLayers);
            drawer.OnDraw(g, DrawContext);
            g.Restore(saved);
        }

        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (DrawContext == null) return;
            /// マウスホイールでは拡大縮小のみ行う。
            if (e.Delta < 0)
            {
                DrawContext.Scale *= 2 / 3.0f;
            }
            else
            {
                DrawContext.Scale *= 1.5f;
            }
            CalcSize();
            panel1.Invalidate();
        }

        private void panel1_ClientSizeChanged(object sender, EventArgs e)
        {
            CalcSize();
        }

    }
}
