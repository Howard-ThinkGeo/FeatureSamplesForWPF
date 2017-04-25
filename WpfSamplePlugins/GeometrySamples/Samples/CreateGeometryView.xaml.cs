using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class CreateGeometryView : UserControl
    {
        public CreateGeometryView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White, 4));
            highlightLayer.Styles.Add(new LineStyle(GeoColors.White, 8));
            highlightLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#88FAB04D"), 4));
            highlightLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#55FAB04D"), GeoColors.White, 4));
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(new GeoBound(2171997.6512, 8356849.2669, 3515687.9933, 11097616.86));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer highlightLayer = Map1.FindLayer<MemoryLayer>("HighlightLayer");
            highlightLayer.Features.Clear();

            Button button = (Button)sender;
            switch (button.Content.ToString())
            {
                case "Polygon":
                    highlightLayer.Features.Add(new Feature(CreatePolygon()));
                    break;
                case "Polygon with Hole":
                    highlightLayer.Features.Add(new Feature(CreatePolygonWithHole()));
                    break;
                case "Ellipse":
                    highlightLayer.Features.Add(new Feature(CreateEllipse()));
                    break;
                case "Multi-Polygon":
                    highlightLayer.Features.Add(new Feature(CreateMultiPolygon()));
                    break;
                case "Rectangle":
                    highlightLayer.Features.Add(new Feature(CreateRectangle()));
                    break;
                case "Point":
                    highlightLayer.Features.Add(new Feature(CreatePoint()));
                    break;
                case "Multi-Point":
                    highlightLayer.Features.Add(new Feature(CreateMultiPoint()));
                    break;
                case "Line": 
                    highlightLayer.Features.Add(new Feature(CreateLine()));
                    break;
                case "Multi-Line":
                    highlightLayer.Features.Add(new Feature(CreateMultiLine()));
                    break;
                default:
                    break;
            }

            Map1.Refresh("HighlightOverlay");
        }

        #region Create Geometries

        private GeoMultiLine CreateMultiLine()
        {
            GeoLine line1 = new GeoLine();
            GeoLine line2 = new GeoLine();

            double startX = Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .15;
            double endX = Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .85;
            double height = Map1.CurrentBound.Height * .25;
            double centerY1 = Map1.CurrentCenter.Y + height;
            double centerY2 = Map1.CurrentCenter.Y - height;
            int segmentCount = 30;
            double segmentHorizontalLength = (endX - startX) / segmentCount;

            for (int i = 0; i < segmentCount; i++)
            {
                double x1 = startX + segmentHorizontalLength * i;
                double y1 = Math.Sin(Math.PI * 2 * i / segmentCount) * height + centerY1;
                line1.Coordinates.Add(new GeoCoordinate(x1, y1));

                double x2 = startX + segmentHorizontalLength * i;
                double y2 = Math.Cos(Math.PI * 2 * i / segmentCount) * height + centerY2;
                line2.Coordinates.Add(new GeoCoordinate(x2, y2));
            }

            GeoMultiLine multiLine = new GeoMultiLine();
            multiLine.Lines.Add(line1);
            multiLine.Lines.Add(line2);
            return multiLine;
        }

        private GeoLine CreateLine()
        {
            GeoLine line = new GeoLine();

            double startX = Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .15;
            double endX = Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .85;
            double centerY = Map1.CurrentCenter.Y;
            double height = Map1.CurrentBound.Height * .25;
            int segmentCount = 30;
            double segmentHorizontalLength = (endX - startX) / segmentCount;

            for (int i = 0; i < segmentCount; i++)
            {
                double x = startX + segmentHorizontalLength * i;
                double y = Math.Sin(Math.PI * 2 * i / segmentCount) * height  + centerY;
                line.Coordinates.Add(new GeoCoordinate(x, y));
            }

            return line;
        }

        private GeoMultiPoint CreateMultiPoint()
        {
            Random r = new Random();
            GeoMultiPoint multiPoint = new GeoMultiPoint();
            for (int i = 0; i < 30; i++)
            {
                int x = r.Next((int)Map1.CurrentBound.MinX, (int)Map1.CurrentBound.MaxX);
                int y = r.Next((int)Map1.CurrentBound.MinY, (int)Map1.CurrentBound.MaxY);
                multiPoint.Points.Add(new GeoPoint(x, y));
            }

            return multiPoint;
        }

        private GeoPoint CreatePoint()
        {
            double x = Map1.CurrentCenter.X;
            double y = Map1.CurrentCenter.Y;
            return new GeoPoint(x, y);
        }

        private GeoBound CreateRectangle()
        {
            double minX = Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .25;
            double maxX = Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .75;
            double minY = Map1.CurrentBound.MinY + Map1.CurrentBound.Height * .25;
            double maxY = Map1.CurrentBound.MinY + Map1.CurrentBound.Height * .75;

            GeoBound rect = new GeoBound(minX, minY, maxX, maxY);
            return rect;
        }

        private GeoEllipse CreateEllipse()
        {
            GeoBound bound = (GeoBound)Map1.CurrentBound.Clone();
            bound.ScaleDown(40);
            GeoEllipse ellipse = new GeoEllipse(bound);

            //// use this to create ellipse by center, width and height.
            //GeoEllipse ellipse = new GeoEllipse(bound.GetCentroid(), bound.Width, bound.Height);

            return ellipse;
        }

        private GeoMultiPolygon CreateMultiPolygon()
        {
            GeoPoint center1 = new GeoPoint(Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .25, Map1.CurrentCenter.Y);
            GeoPoint center2 = new GeoPoint(Map1.CurrentBound.MinX + Map1.CurrentBound.Width * .75, Map1.CurrentCenter.Y);
            double radius = Map1.CurrentBound.Width * .25;
            int segmentCount = 5;

            GeoPolygon polygon1 = new GeoPolygon(CreateRing(center1, radius, segmentCount));
            GeoPolygon polygon2 = new GeoPolygon(CreateRing(center2, radius, segmentCount));

            GeoMultiPolygon multiPolygon = new GeoMultiPolygon();
            multiPolygon.Polygons.Add(polygon1);
            multiPolygon.Polygons.Add(polygon2);
            return multiPolygon;
        }

        private GeoPolygon CreatePolygon()
        {
            GeoPoint center = Map1.CurrentCenter;
            int segmentCount = 5;
            double radius = Math.Min(Map1.CurrentBound.Width, Map1.CurrentBound.Height) * .4;

            GeoPolygon polygon = new GeoPolygon();
            polygon.OuterRing = CreateRing(center, radius, segmentCount);

            return polygon;
        }

        private GeoPolygon CreatePolygonWithHole()
        {
            GeoPoint center = Map1.CurrentCenter;
            int segmentCount = 5;
            double radius = Math.Min(Map1.CurrentBound.Width, Map1.CurrentBound.Height) * .4;

            GeoPolygon polygon = new GeoPolygon();
            polygon.OuterRing = CreateRing(center, radius, segmentCount);
            polygon.InnerRings.Add(CreateRing(center, radius * .6, segmentCount, true));

            return polygon;
        }

        private static GeoRing CreateRing(GeoPoint center, double radius, int segmentCount, bool reverse = false)
        {
            double angle = Math.PI * 2 / segmentCount;

            GeoRing ring = new GeoRing();
            for (int i = 0; i < segmentCount; i++)
            {
                double x = radius * Math.Cos(angle * i) + center.X;
                double y = radius * Math.Sin(angle * i) + center.Y;
                if (reverse) ring.Coordinates.Insert(0, new GeoCoordinate(x, y));
                else ring.Coordinates.Add(new GeoCoordinate(x, y));
            }

            if (reverse) ring.Coordinates.Insert(0, ring.Coordinates.Last().Clone());
            else ring.Coordinates.Add(ring.Coordinates.First().Clone());

            return ring;
        }
        #endregion
    }
}
