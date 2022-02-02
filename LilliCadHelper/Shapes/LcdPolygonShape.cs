﻿using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdPolygonShape : LcdShape{
        public LcdLineStyle LineStyle { get; set; }
        public LcdFaceColor FaceColor { get; set; }
        /// <summary>
        /// Flag 2:開放(Polyline) 3:閉じる(Polygon)
        /// </summary>
        public int Flag { get; set; }
        public LcdArrowStyle StartArrow { get; set; }
        public LcdArrowStyle EndArrow { get; set; }
        public List<LcdPoint> Points { get; set; } = new();
        public override string ToString() {
            return $"Polygon(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}  Arrow0{StartArrow} Arrow1{EndArrow}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.ReadParameters();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            Flag = param.GetInt();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
            Points = sr.ReadPoints();
        }

        internal override void Write(StreamWriter sw)
        {
            sw.WriteLine("POLYGON");
            sw.Write($"\t{LineStyle.ToLcdString()} {FaceColor.ToLcdString()} ");
            sw.Write($"{Flag} ");
            sw.Write($"{StartArrow.ToLcdString()} ");
            sw.WriteLine($"{EndArrow.ToLcdString()}");
            WritePoints(sw, Points);
        }

    }
}