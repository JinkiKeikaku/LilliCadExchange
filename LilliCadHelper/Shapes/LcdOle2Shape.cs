using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdOle2Shape : LcdShape
    {
        public LcdPoint P0 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public byte[] Datas { get; set; }

        public override string ToString()
        {
            return $"Ole2(P0{P0} W:{Width} H:{Height})";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            P0 = param.GetPoint();
            Width = param.GetDouble();
            Height = param.GetDouble();
            Datas = sr.ReadBytes();
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("OLE2");
            sw.WriteLine($"\t{P0.ToLcdString()} {Width} {Height}");
            WriteBytes(sw, Datas, true);
        }
    }
}
