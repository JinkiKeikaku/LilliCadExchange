
namespace LilliCadHelper.Shapes
{
    public class LcdFaceColor {
        public enum Gradation { None, Line, Rectangle, Circle };
        public Gradation GradationType { get; set; } = Gradation.None;
        /// <summary>
        /// 色の配列。グラデーションの場合は希望する色数の配列を設定すること。
        /// Noneは[0]のみ、中間色無しの場合は開始色[0]と終了色[1]、中間色ありは開始色[0]と中間色[1]と終了色[2]。
        /// </summary>
        public int[] Colors { get; set; } = new int[1] { 0 };
        /// <summary>
        /// 中間色の位置(0から1.0）
        /// </summary>
        public float MP { get; set; }
        /// <summary>
        /// 角度（GradationType:LineとRectangleで使用）
        /// </summary>
        public float Angle { get; set; } = 0.0f;
        /// <summary>
        /// 中心X座標(0から1.0）
        /// </summary>
        public float X { get; set; } = 0.5f;
        /// <summary>
        /// 中心Y座標(0から1.0）
        /// </summary>
        public float Y { get; set; } = 0.5f;
        public override string ToString()
        {
            if (Colors.Length == 0)
            {
                //これは本来無効だが、ミス防止のためいれた。
                return $"0";
            }
            if (Colors.Length == 1)
            {
                return $"{Colors[0]}";
            }
            switch(GradationType)
            {
                case Gradation.Line:
                    if (Colors.Length == 2) {
                        return $"G1 {Angle} 2 {Colors[0]} {Colors[1]}";
                    }
                    else
                    {
                        return $"G1 {Angle} 3 {Colors[0]} {Colors[1]} {Colors[2]} {MP}";
                    }
                case Gradation.Rectangle:
                    if (Colors.Length == 2)
                    {
                        return $"G2 {Angle} {X} {Y} 2 {Colors[0]} {Colors[1]}";
                    }
                    else
                    {
                        return $"G2 {Angle} {X} {Y} 3 {Colors[0]} {Colors[1]} {Colors[2]} {MP}";
                    }
                case Gradation.Circle:
                    if (Colors.Length == 2)
                    {
                        return $"G3 {X} {Y} 2 {Colors[0]} {Colors[1]}";
                    }
                    else
                    {
                        return $"G3 {X} {Y} 3 {Colors[0]} {Colors[1]} {Colors[2]} {MP}";
                    }
                default:
                    return $"{Colors[0]}";
            }
        }
    }
}
