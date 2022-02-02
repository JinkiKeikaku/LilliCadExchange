using LilliCadHelper;
using LilliCadHelper.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilliCadViewer
{
    class LilliCadDrawer
    {
        LcdHeader mHeader;
        List<LcdLayer> mLayers;
        CadPoint mOrigin = new(0,0);
        double mScale = 1.0;

        public LilliCadDrawer(LcdHeader header, List<LcdLayer> layers)
        {
            mHeader = header;
            mLayers = layers;
            mOrigin = GetPaperOrigin(header);
            mScale = mHeader.PaperScale;
        }

        public void OnDraw(Graphics g, DrawContext d)
        {
            foreach(var layer in mLayers)
            {
                foreach(var shape in layer.Shapes)
                {
                    OnDrawShape(g, d, shape);
                }
            }

        }

        void OnDrawShape(Graphics g, DrawContext d, LcdShape shape)
        {
            switch (shape)
            {
                case LcdLineShape s:    OnDrawLine(g, d, s);    break;
                case LcdCircleShape s: OnDrawCircle(g, d, s); break;
                case LcdEllipseShape s: OnDrawEllipse(g, d, s); break;
                case LcdArcShape s: OnDrawArc(g, d, s); break;
                case LcdFanShape s: OnDrawFan(g, d, s); break;
                case LcdPolygonShape s: OnDrawPolygon(g, d, s); break;
                case LcdSplineShape s: OnDrawSpline(g, d, s); break;
                case LcdSplineLoopShape s: OnDrawSplineLoop(g, d, s); break;
                case LcdMarkShape s: OnDrawMark(g, d, s); break;
                case LcdRectShape s: OnDrawRect(g, d, s); break;
                case LcdTextShape s: OnDrawText(g, d, s); break;
                case LcdMultiTextShape s: OnDrawMultiText(g, d, s); break;
                case LcdGroupShape s: OnDrawGroup(g, d, s); break;
                case LcdSizeShape s: OnDrawSize(g, d, s); break;
                case LcdRadiusShape s: OnDrawRadius(g, d, s); break;
                case LcdDiameterShape s: OnDrawDiameter(g, d, s); break;
                case LcdAngleShape s: OnDrawAngle(g, d, s); break;
                case LcdBitmapShape s: OnDrawBitmap(g, d, s); break;
                case LcdOle2Shape s: OnDrawOle2(g, d, s); break;
                default:
                    break;
            }
        }

        void OnDrawLine(Graphics g, DrawContext d, LcdLineShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var p1 = d.DocToCanvas(ConvertPoint(shape.P1));
            g.DrawLine(d.Pen, p0, p1);
        }

        void OnDrawCircle(Graphics g, DrawContext d, LcdCircleShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var r = d.DocToCanvas(ConvertLength(shape.Radius));
            g.DrawEllipse(d.Pen, p0.X - r, p0.Y - r, r * 2, r * 2);
        }
        void OnDrawEllipse(Graphics g, DrawContext d, LcdEllipseShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var rx = d.DocToCanvas(ConvertLength(shape.RX));
            var ry = d.DocToCanvas(ConvertLength(shape.RY));
            g.DrawEllipse(d.Pen, p0.X - rx, p0.Y - ry, rx * 2, ry * 2);
        }
        void OnDrawArc(Graphics g, DrawContext d, LcdArcShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var r = d.DocToCanvas(ConvertLength(shape.Radius));
            var sw = RadToDeg(shape.EndAngleRad) - RadToDeg(shape.StartAngleRad);
            if (sw < 0) sw += 360;
            var a1 = d.DocToCanvasAngle(RadToDeg(shape.StartAngleRad));
            var sw1 = d.DocToCanvasAngle(sw);
            g.DrawArc(d.Pen, p0.X - r, p0.Y - r, r * 2, r * 2, a1, sw1);
        }
        void OnDrawFan(Graphics g, DrawContext d, LcdFanShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var r = d.DocToCanvas(ConvertLength(shape.Radius));
            var sw = RadToDeg(shape.EndAngleRad) - RadToDeg(shape.StartAngleRad);
            if (sw < 0) sw += 360;
            var a1 = d.DocToCanvasAngle(RadToDeg(shape.StartAngleRad));
            var sw1 = d.DocToCanvasAngle(sw);
            g.DrawPie(d.Pen, p0.X - r, p0.Y - r, r * 2, r * 2, a1, sw1);
        }
        void OnDrawPolygon(Graphics g, DrawContext d, LcdPolygonShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var pts = new PointF[shape.Points.Count];
            for (var i=0; i < shape.Points.Count; i++)
            {
                pts[i] = d.DocToCanvas(ConvertPoint(shape.Points[i]));
            }
            if ((shape.Flag & 1) != 0)
            {
                g.DrawPolygon(d.Pen, pts);
            }
            else
            {
                g.DrawLines(d.Pen, pts);
            }
        }
        void OnDrawSpline(Graphics g, DrawContext d, LcdSplineShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var pts = new PointF[shape.Points.Count];
            for (var i = 0; i < shape.Points.Count; i++)
            {
                pts[i] = d.DocToCanvas(ConvertPoint(shape.Points[i]));
            }
            g.DrawCurve(d.Pen, pts);
        }
        void OnDrawSplineLoop(Graphics g, DrawContext d, LcdSplineLoopShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var pts = new PointF[shape.Points.Count];
            for (var i = 0; i < shape.Points.Count; i++)
            {
                pts[i] = d.DocToCanvas(ConvertPoint(shape.Points[i]));
            }
            g.DrawClosedCurve(d.Pen, pts);
        }
        void OnDrawMark(Graphics g, DrawContext d, LcdMarkShape shape)
        {
            var r = d.DocToCanvas(shape.Radius);
            d.Pen.Color = Color.Cyan;
            d.Pen.Width = 0;
            d.Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            g.DrawLine(d.Pen, p0.X - r, p0.Y - r, p0.X + r, p0.Y + r);
            g.DrawLine(d.Pen, p0.X - r, p0.Y + r, p0.X + r, p0.Y - r);
        }
        void OnDrawRect(Graphics g, DrawContext d, LcdRectShape shape)
        {
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var w = d.DocToCanvas(ConvertLength(shape.Width));
            var h = -d.DocToCanvas(ConvertLength(shape.Height));
            if(w < 0)
            {
                p0.X += w;
                w = -w;
            }
            if(h < 0)
            {
                p0.Y += h;
                h = -h;

            }
            g.DrawRectangle(d.Pen, p0.X, p0.Y, w, h);
        }
        void OnDrawGroup(Graphics g, DrawContext d, LcdGroupShape shape)
        {
            foreach(var s in shape.Shapes)
            {
                OnDrawShape(g, d, s);
            }
        }

        void OnDrawText(Graphics g, DrawContext d, LcdTextShape shape)
        {
            //文字表示のみで配置基準など省略。
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var angle = d.DocToCanvasAngle(RadToDeg(shape.Angle));
            using var font = new Font(shape.FontName, d.DocToCanvas(ConvertLength(shape.FontHeight)), GraphicsUnit.Pixel);
            using var brush = new SolidBrush(ConvertColor(shape.TextColor));
            var saved = g.Save();
            g.TranslateTransform(p0.X, p0.Y);
            g.RotateTransform(angle);
            g.DrawString(shape.Text, font, brush, 0,0);
            g.Restore(saved);
        }

        void OnDrawMultiText(Graphics g, DrawContext d, LcdMultiTextShape shape)
        {
            //文字表示のみで配置基準など省略。
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var angle = d.DocToCanvasAngle(RadToDeg(shape.Angle));
            using var font = new Font(shape.FontName, d.DocToCanvas(ConvertLength(shape.FontHeight)), GraphicsUnit.Pixel);
            using var brush = new SolidBrush(ConvertColor(shape.TextColor));
            var saved = g.Save();
            g.TranslateTransform(p0.X, p0.Y);
            g.RotateTransform(angle);
            g.DrawString(shape.Text, font, brush, 0,0);
            g.Restore(saved);
        }

        void OnDrawSize(Graphics g, DrawContext d, LcdSizeShape shape)
        {
            //文字の離れとか寸法線のはみ出しとか文字の背景色（FaceColor）、文字の配置などは省略などは省略
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var pts = new CadPoint[5];
            for(var i = 0; i < 5; i++)
            {
                pts[i] = ConvertPoint(shape.Points[i]);
            }
            var dp20 = (pts[2] - pts[0]).UnitPoint();
            var dp31 = (pts[3] - pts[1]).UnitPoint();

            var p0 = d.DocToCanvas(pts[0] - dp20 * shape.SizeStyle.Linegap);
            var p1 = d.DocToCanvas(pts[1] - dp31 * shape.SizeStyle.Linegap);
            var p21 = d.DocToCanvas(pts[2] + dp20 * shape.SizeStyle.Linedrop);
            var p31 = d.DocToCanvas(pts[3] + dp31 * shape.SizeStyle.Linedrop);
            var p2 = d.DocToCanvas(pts[2]);
            var p3 = d.DocToCanvas(pts[3]);
            var p4 = d.DocToCanvas(pts[4]);
            using var font = new Font(shape.FontName, d.DocToCanvas(shape.FontHeight), GraphicsUnit.Pixel);
            using var brush = new SolidBrush(ConvertColor(shape.TextColor));
            g.DrawLine(d.Pen, p0, p21);
            g.DrawLine(d.Pen, p1, p31);
            g.DrawLine(d.Pen, p2, p3);
            var saved = g.Save();
            var angle = d.DocToCanvasAngle(RadToDeg((pts[3]- pts[2]).GetAngle()));
            g.TranslateTransform(p4.X, p4.Y);
            g.RotateTransform(angle);
            var f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Far;
            g.DrawString(shape.Text, font, brush, 0,0, f);
            g.Restore(saved);
        }
        void OnDrawRadius(Graphics g, DrawContext d, LcdRadiusShape shape)
        {
            //文字の離れとか寸法線のはみ出しとか文字の背景色（FaceColor）、文字の配置などは省略などは省略
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = ConvertPoint(shape.P0);
            var r = ConvertLength(shape.Radius);
            var p1 = p0 + CadPoint.CreateFromPolar(r, shape.Angle);
            var p2 = (p1 - p0).UnitPoint() * ConvertLength(shape.TR)+p0;
            var p00 = d.DocToCanvas(p0);
            var p01 = d.DocToCanvas(p1);
            var p02 = d.DocToCanvas(p2);
            using var font = new Font(shape.FontName, d.DocToCanvas(shape.FontHeight), GraphicsUnit.Pixel);
            using var brush = new SolidBrush(ConvertColor(shape.TextColor));
            g.DrawLine(d.Pen, p00, p01);
            var saved = g.Save();
            var angle = d.DocToCanvasAngle(RadToDeg(shape.Angle));
            g.TranslateTransform(p02.X, p02.Y);
            g.RotateTransform(angle);
            var f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Far;
            g.DrawString(shape.Text, font, brush, 0, 0, f);
            g.Restore(saved);
        }

        void OnDrawDiameter(Graphics g, DrawContext d, LcdDiameterShape shape)
        {
            //文字の離れとか寸法線のはみ出しとか文字の背景色（FaceColor）、文字の配置などは省略などは省略
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = ConvertPoint(shape.P0);
            var r = ConvertLength(shape.Radius);
            var p1 = p0 + CadPoint.CreateFromPolar(r, shape.Angle);
            var p2 = p0 - CadPoint.CreateFromPolar(r, shape.Angle);
            var p3 = (p1 - p0).UnitPoint() * ConvertLength(shape.TR) + p0;
            var p00 = d.DocToCanvas(p0);
            var p01 = d.DocToCanvas(p1);
            var p02 = d.DocToCanvas(p2);
            var p03 = d.DocToCanvas(p3);
            using var font = new Font(shape.FontName, d.DocToCanvas(shape.FontHeight), GraphicsUnit.Pixel);
            using var brush = new SolidBrush(ConvertColor(shape.TextColor));
            g.DrawLine(d.Pen, p01, p02);
            var saved = g.Save();
            var angle = d.DocToCanvasAngle(RadToDeg(shape.Angle));
            g.TranslateTransform(p03.X, p03.Y);
            g.RotateTransform(angle);
            var f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Far;
            g.DrawString(shape.Text, font, brush, 0, 0, f);
            g.Restore(saved);
        }

        void OnDrawAngle(Graphics g, DrawContext d, LcdAngleShape shape)
        {
            //文字の離れとか寸法線のはみ出しとか文字の背景色（FaceColor）、文字の配置などは省略
            ApplyLineStyle(d.Pen, shape.LineStyle);
            var p0 = ConvertPoint(shape.P0);
            var r = d.DocToCanvas(ConvertLength(shape.Radius));
            var a0 = RadToDeg(shape.Angles[0]);
            var a1 = RadToDeg(shape.Angles[1]);
            var sw = a1 - a0;
            if (sw < 0) sw += 360;
            var p00 = d.DocToCanvas(p0);
            g.DrawArc(d.Pen, p00.X - r, p00.Y - r, r * 2, r * 2, d.DocToCanvasAngle(a0), d.DocToCanvasAngle(sw));

            using var font = new Font(shape.FontName, d.DocToCanvas(shape.FontHeight), GraphicsUnit.Pixel);
            using var brush = new SolidBrush(ConvertColor(shape.TextColor));
            var saved = g.Save();
            var angle = d.DocToCanvasAngle(RadToDeg(shape.Angles[2]-Math.PI / 2));
            g.TranslateTransform(p00.X, p00.Y);
            g.RotateTransform(angle);
            var f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Far;
            g.DrawString(shape.Text, font, brush, 0, -r, f);
            g.Restore(saved);
        }

        void OnDrawBitmap(Graphics g, DrawContext d, LcdBitmapShape shape)
        {
            var ms = new MemoryStream();
            ms.Write(shape.FileHeader, 0, shape.FileHeader.Length);
            ms.Write(shape.InfoHeader, 0, shape.InfoHeader.Length);
            ms.Write(shape.Datas, 0, shape.Datas.Length);
            ms.Seek(0, SeekOrigin.Begin);
            var image = Bitmap.FromStream(ms);

            var p0 = d.DocToCanvas(ConvertPoint(shape.P0)); //左下
            var w = d.DocToCanvas(ConvertLength(shape.Width));
            var h = d.DocToCanvas(ConvertLength(shape.Height));
            RectangleF r = new RectangleF();
            g.DrawImage(image, p0.X, p0.Y-h, w, h);
        }

        void OnDrawOle2(Graphics g, DrawContext d, LcdOle2Shape shape)
        {
            var p0 = d.DocToCanvas(ConvertPoint(shape.P0));
            var w = d.DocToCanvas(ConvertLength(shape.Width));
            var h = -d.DocToCanvas(ConvertLength(shape.Height));
            if (w < 0)
            {
                p0.X += w;
                w = -w;
            }
            if (h < 0)
            {
                p0.Y += h;
                h = -h;

            }
            d.Pen.Color = Color.Black;
            d.Pen.Width = 1.0f;
            g.DrawRectangle(d.Pen, p0.X, p0.Y, w, h);
            g.DrawLine(d.Pen, p0.X, p0.Y, p0.X + w, p0.Y+h);
            g.DrawLine(d.Pen, p0.X+w, p0.Y, p0.X, p0.Y+h);
        }

        CadPoint ConvertPoint(LcdPoint p)
        {
            return new CadPoint(mScale * p.X + mOrigin.X, mScale * p.Y + mOrigin.Y);
        }
        double ConvertLength(double a)
        {
            return mScale * a;
        }

        double RadToDeg(double rad) => rad * 180 / Math.PI;

        void ApplyLineStyle(Pen pen, LcdLineStyle style)
        {
            var c = ConvertColor(style.LineColor);
            pen.Color = c;
            if (style.LineType == 0)
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            }
            else
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                pen.DashPattern = GetLinePattern(style.LineType);
            }
            pen.Width = style.LineWidth;
        }

        Color ConvertColor(int c)
        {
            if (c == LILLICAD_NULL_COLOR)
            {
                return Color.Transparent;
            }
            return Color.FromArgb(c & 255, (c >> 8) & 255, (c >> 16) & 255);
        }

        CadPoint GetPaperOrigin(LcdHeader header)
        {
            (double width, double height) = header.IsPaperHorizontal ?
                (header.PaperWidth, header.PaperHeight) :
                (header.PaperHeight, header.PaperWidth);
            return header.PaperOriginFlag switch
            {
                0 => new CadPoint(-width / 2, height / 2),
                1 => new CadPoint(0, height / 2),
                2 => new CadPoint(width / 2, height / 2),
                3 => new CadPoint(-width / 2, 0),
                4 => new CadPoint(0, 0),
                5 => new CadPoint(width / 2, 0),
                6 => new CadPoint(-width / 2, -height / 2),
                7 => new CadPoint(0, -height / 2),
                8 => new CadPoint(width / 2, -height / 2),
                _ => new CadPoint(0, 0),
            };
        }

        float[] GetLinePattern(int lineType)
        {
            if ((lineType & 128) != 0)
            {
                return DotPattern[8];
            }
            return DotPattern[lineType];
        }


        static readonly float[][] DotPattern = new float[][] {
            new float[]{0},
            new float[]{1.25f,1.25f},
            new float[]{2.5f,2.5f},
            new float[]{3.75f,1.25f},
            new float[]{3.75f,1.25f, 1.25f,1.25f},
            new float[]{6.25f,2.5f, 2.5f,2.5f},
            new float[]{3.25f,1.25f, 1.25f,1.25f, 1.25f,1.25f},
            new float[]{8.0f,2.5f, 1.25f,2.5f, 1.25f,2.5f},
            new float[]{0.625f, 1.875f},
        };
        const int LILLICAD_NULL_COLOR = 0x1000000;//(RGB(255,255,255) + 1)


    }
}
