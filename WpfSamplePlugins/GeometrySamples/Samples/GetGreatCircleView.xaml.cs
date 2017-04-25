using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class GetGreatCircleView : UserControl
    {
        public GetGreatCircleView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            GeoLine greatCircle = CreateGreatCircle();

            MemoryLayer baselineLayer = new MemoryLayer { Name = "BaseLineLayer" };
            baselineLayer.Styles.Add(new LineStyle(GeoColors.White, 8));
            baselineLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#88FAB04D"), 4));
            baselineLayer.Features.Add(new Feature(greatCircle));
            Map1.AddStaticLayers(baselineLayer);

            Stream airplaneIconStream = Application.GetResourceStream(new Uri("/SlimGis.Samples.Geometries;component/Images/airplane.png", UriKind.RelativeOrAbsolute)).Stream;

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#9903A9F4"), 4));
            highlightLayer.Styles.Add(new IconStyle(new GeoImage(airplaneIconStream)) { AspectRatio = 0.6667 });
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            GeoBound bound = baselineLayer.GetBound();
            bound.ScaleUp(25);
            Map1.ZoomTo(bound);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MemoryLayer baselineLayer = Map1.FindLayer<MemoryLayer>("BaseLineLayer");
            MemoryLayer highlightLayer = Map1.FindLayer<MemoryLayer>("HighlightLayer");
            GeoLine baseline = (GeoLine)baselineLayer.Features.First().Geometry;

            int times = 100;
            double percentageRatio = 100d / times;

            button.IsEnabled = false;
            for (int i = 0; i <= times; i++)
            {
                await MoveLine(highlightLayer, baseline, percentageRatio, i);
            }

            for (int i = times; i >= 0; i--)
            {
                await MoveLine(highlightLayer, baseline, percentageRatio, i, true);
            }
            button.IsEnabled = true;
        }

        private async Task MoveLine(MemoryLayer highlightLayer, GeoLine baseline, double percentageRatio, int i, bool reverse = false)
        {
            GeoLine line = await Task.Run(() => (GeoLine)baseline.GetSegmentation((float)percentageRatio * i));
            highlightLayer.Features.Clear();

            if (line != null)
            {
                highlightLayer.Features.Add(new Feature(line));

                GeoCoordinate currentPoint = line.Coordinates.Last();
                highlightLayer.Features.Add(new Feature(new GeoPoint(currentPoint)));
                if (line.Coordinates.Count > 1)
                {
                    GeoCoordinate previousPoint = line.Coordinates.ElementAt(line.Coordinates.Count - 2);
                    double angle = Math.Atan2(currentPoint.Y - previousPoint.Y, currentPoint.X - previousPoint.X);
                    float degree = (float)(angle * 180 / Math.PI);
                    if (reverse) degree = degree + 180;
                    highlightLayer.Styles.OfType<IconStyle>().First().Rotation = degree;
                }
            }
            Map1.Refresh("HighlightOverlay");

            await Task.Run(() => Thread.Sleep(50));
        }

        private GeoLine CreateGreatCircle()
        {
            GeoPoint point1 = new GeoPoint(-73.935242, 40.730610);
            GeoPoint point2 = new GeoPoint(2.294694, 48.858093);
            GeoMultiLine greatCircleInWgs84 = point1.GetGreatCircle(point2, 30);

            Projection projection = new Proj4Projection(SpatialReferences.GetWgs84(), SpatialReferences.GetSphericalMercator());
            GeoMultiLine greatCircleIn900913 = (GeoMultiLine)projection.ConvertToTarget(greatCircleInWgs84);
            return greatCircleIn900913.Lines[0];
        }
    }
}
