using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdLineShape : LcdShape
    {
        public LcdPoint P0 { get; set; }
        public LcdPoint P1 { get; set; }
        public LcdLineStyle LineStyle { get; set; }
        public LcdArrowStyle StartArrow { get; set; }
        public LcdArrowStyle EndArrow { get; set; }


        public override string ToString()
        {
            return $"Line(P0{P0} P1{P1} LineStyle{LineStyle} Arrow0{StartArrow} Arrow1{EndArrow}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            P0 = param.GetPoint();
            P1 = param.GetPoint();
            LineStyle = param.GetLineStyle();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
        }
        internal override void Write(LcdStreamWriter sw) 
        {
            sw.WriteLine("LINE");
            sw.WriteParamLine(P0,P1, LineStyle, StartArrow, EndArrow);
        }

    }
}
