using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadViewer
{
    class LcdToShapeConverter
    {
        const int LILLICAD_NULL_COLOR = 0x1000000;//(RGB(255,255,255) + 1)
        CadPoint mOrigin;
        double mScale;
        public LcdToShapeConverter(CadPoint origin, double scale)
        {
            mOrigin = origin;
            mScale = scale;
        }

        CadPoint ConvertPoint(LilliCadHelper.LcdPoint p)
        {
            return new CadPoint(mScale * p.X + mOrigin.X, mScale * p.Y + mOrigin.Y);
        }
        double ConvertLength(double a)
        {
            return mScale * a;
        }
        public Color ConvertColor(int c)
        {
            if (c == LILLICAD_NULL_COLOR)
            {
                return Color.Transparent;
            }
            return Color.FromArgb(c & 255, (c >> 8) & 255, (c >> 16) & 255);
        }


    }
}
