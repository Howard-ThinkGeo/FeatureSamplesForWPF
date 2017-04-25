using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for FindClosestPointSample.xaml
    /// </summary>
    public partial class GetIntersectionView : UserControl
    {
        private Feature feature1;
        private Feature feature2;

        public GetIntersectionView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            GeoBound bound = new GeoBound(2171997.6512, 8356849.2669, 3515687.9933, 11097616.86);
            GeoPoint center = bound.GetCentroid();
            double x1 = bound.MinX + bound.Width * .25;
            double y = center.Y;
            double x2 = bound.MaxX - bound.Width * .25;
            double radius = bound.Width * 3 / 8;

            feature1 = new Feature(new GeoEllipse(new GeoPoint(x1, y), radius));
            feature2 = new Feature(new GeoEllipse(new GeoPoint(x2, y), radius));

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Features.Add(feature1);
            highlightLayer.Features.Add(feature2);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            MemoryLayer resultLayer = new MemoryLayer { Name = "ResultLayer" };
            resultLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#99FF5722"), GeoColors.White));
            Map1.AddDynamicLayers("ResultOverlay", resultLayer);

            Map1.ZoomTo(bound);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer resultLayer = Map1.FindLayer<MemoryLayer>("ResultLayer");
            if (resultLayer.Features.Count == 0)
            {
                Geometry intersection = feature1.Geometry.GetIntersection(feature2.Geometry);
                resultLayer.Features.Add(new Feature(intersection));

                Map1.Refresh("ResultOverlay");
            }
        }
    }
}
