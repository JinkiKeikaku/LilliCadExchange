using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdBitmapShape : LcdShape
    {
        public LcdPoint P0 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public byte[] FileHeader { get; set; }
        public byte[] InfoHeader { get; set; }
        public byte[] Datas { get; set; }

        public override string ToString()
        {
            return $"Bitmap(P0{P0} W:{Width} H:{Height})";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            P0 = param.GetPoint();
            Width = param.GetDouble();
            Height = param.GetDouble();
            FileHeader = sr.ReadBytes();
            InfoHeader = sr.ReadBytes();
            Datas = sr.ReadBytes();
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("BITMAP");
            sw.WriteLine($"\t{P0.ToLcdString()} {Width} {Height}");
            WriteBytes(sw, FileHeader, false);
            WriteBytes(sw, InfoHeader, false);
            WriteBytes(sw, Datas, true);
        }

    }
}
