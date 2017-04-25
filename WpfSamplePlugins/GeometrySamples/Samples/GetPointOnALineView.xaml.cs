using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class GetPointOnALineView : UserControl
    {
        public GetPointOnALineView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();
            GeoBound currentBound = new GeoBound(2171997.6512, 8356849.2669, 3515687.9933, 11097616.86);

            MemoryLayer baselineLayer = new MemoryLayer { Name = "BaseLineLayer" };
            baselineLayer.Styles.Add(new LineStyle(GeoColors.White, 8));
            baselineLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#88FAB04D"), 4));
            baselineLayer.Features.Add(CreateLineFeature(currentBound));
            Map1.AddStaticLayers(baselineLayer);

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#9903A9F4"), GeoColors.White, 4));
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(currentBound);
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
                await MovePoint(highlightLayer, baseline, percentageRatio, i);
            }

            for (int i = times; i > 0; i--)
            {
                await MovePoint(highlightLayer, baseline, percentageRatio, i);
            }
            button.IsEnabled = true;
        }

        private async Task MovePoint(MemoryLayer highlightLayer, GeoLine baseline, double percentageRatio, int i)
        {
            GeoPoint point = await Task.Run(() => baseline.GetPoint((float)percentageRatio * i));
            highlightLayer.Features.Clear();
            highlightLayer.Features.Add(new Feature(point));
            Map1.Refresh("HighlightOverlay");

            await Task.Run(() => Thread.Sleep(50));
        }

        private Feature CreateLineFeature(GeoBound bound)
        {
            GeoLine line = new GeoLine();

            double startX = bound.MinX + bound.Width * .15;
            double endX = bound.MinX + bound.Width * .85;
            double centerY = bound.GetCentroid().Y;
            double height = bound.Height * .25;
            int segmentCount = 30;
            double segmentHorizontalLength = (endX - startX) / segmentCount;

            for (int i = 0; i < segmentCount; i++)
            {
                double x = startX + segmentHorizontalLength * i;
                double y = Math.Sin(Math.PI * 2 * i / segmentCount) * height + centerY;
                line.Coordinates.Add(new GeoCoordinate(x, y));
            }

            return new Feature(line);
        }
    }
}
