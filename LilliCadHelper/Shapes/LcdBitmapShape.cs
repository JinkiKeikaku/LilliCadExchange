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
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            Width = param.GetDouble();
            Height = param.GetDouble();
            FileHeader = sr.ReadBytes();
            InfoHeader = sr.ReadBytes();
            Datas = sr.ReadBytes();
        }
        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("BITMAP");
            sw.WriteParamLine(P0,Width,Height);
            sw.WriteBytes(FileHeader, false);
            sw.WriteBytes(InfoHeader, false);
            sw.WriteBytes(Datas, true);
        }

    }
}
