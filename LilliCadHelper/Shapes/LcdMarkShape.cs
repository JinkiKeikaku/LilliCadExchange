using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdMarkShape : LcdShape{
        public LcdPoint P0 { get; set; }
        public double Radius { get; set; }
        public override string ToString() {
            return $"Mark(P0{P0} R({Radius})";
        }
        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            P0 = param.GetPoint();
            Radius = param.GetDouble();
        }
        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("MARK");
            sw.WriteLine($"\t{P0.ToLcdString()} {Radius}");
        }
    }
}
