using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadHelper.Shapes
{
    static class LcdShapeManager
    {
        public static LcdShape CreateShape(LcdStreamReader sr)
        {
            string a;
            a = sr.ReadLine();
            LcdShape shape =  a switch
            {
                "LINE" => new LcdLineShape(),
                "POLYGON" => new LcdPolygonShape(),
                "SPLINE" => new LcdSplineShape(),
                "SPLINELOOP" => new LcdSplineLoopShape(),
                "TEXT" => new LcdTextShape(),
                "MULTITEXT" => new LcdMultiTextShape(),
                "RECT" => new LcdRectShape(),
                "CIRCLE" => new LcdCircleShape(),
                "ELLIPSE" => new LcdEllipseShape(),
                "ARC" => new LcdArcShape(),
                "FAN" => new LcdFanShape(),
                "MARK" => new LcdMarkShape(),
                "SIZE" => new LcdSizeShape(),
                "RADIUS" => new LcdRadiusShape(),
                "DIAMETER" => new LcdDiameterShape(),
                "ANGLE" => new LcdAngleShape(),
                "LABEL" => new LcdLabelShape(),
                "BALLOON" => new LcdBalloonShape(),
                "GROUP" => new LcdGroupShape(),
                "BITMAP" => new LcdBitmapShape(),
                "OLE2" => new LcdOle2Shape(),
                _ => null,
            };
            shape?.Read(sr);
            return shape;
        }
    }
}
