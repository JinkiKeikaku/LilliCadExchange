using System.Collections.Generic;
using System.IO;

namespace LilliCadHelper.Shapes
{
    public class LcdPolygonShape : LcdShape
    {
        /// <summary>
        /// Flag 2:開放(Polyline) 3:閉じる(Polygon)
        /// </summary>
        int mFlag { get; set; } = 2;
        /// <summary>
        /// 始点と終点を閉じるか？
        /// </summary>
        public bool IsClosed
        {
            get => (mFlag & 1) != 0;
            set
            {
                mFlag = value ? 3 : 2;
            }
        }
        /// <summary>
        /// 線スタイル
        /// </summary>
        public LcdLineStyle LineStyle { get; set; } = new();
        /// <summary>
        /// 面色
        /// </summary>
        public LcdFaceColor FaceColor { get; set; } = new();
        /// <summary>
        /// 始点矢印
        /// </summary>
        public LcdArrowStyle StartArrow { get; set; } = new();
        /// <summary>
        /// 終点矢印
        /// </summary>
        public LcdArrowStyle EndArrow { get; set; } = new();
        /// <summary>
        /// 頂点リスト
        /// </summary>
        public List<LcdPoint> Points { get; set; } = new();
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Polygon(Points.Count({Points.Count}) LineStyle{LineStyle} FaceColor{FaceColor}  Arrow0{StartArrow} Arrow1{EndArrow}";
        }

        internal override void Read(LcdStreamReader sr)
        {
            var param = sr.GetParameters();
            LineStyle = param.GetLineStyle();
            FaceColor = param.GetFaceColor();
            mFlag = param.GetInt();
            StartArrow = param.GetArrowStyle();
            EndArrow = param.GetArrowStyle();
            Points = sr.ReadPoints();
        }

        internal override void Write(LcdStreamWriter sw)
        {
            sw.WriteLine("POLYGON");
            sw.WriteParamLine(LineStyle, FaceColor, mFlag, StartArrow, EndArrow);
            sw.WritePoints(Points);
        }

    }
}
