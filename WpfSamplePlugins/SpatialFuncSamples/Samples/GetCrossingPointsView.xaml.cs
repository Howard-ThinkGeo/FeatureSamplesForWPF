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
    public partial class GetCrossingPointsView : UserControl
    {
        private Feature highlightFeature;
        private Feature crossLineFeature;

        public GetCrossingPointsView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileSource dataSource = new ShapefileSource("SampleData/countries-900913.shp");
            dataSource.Open();
            highlightFeature = dataSource.GetFeatureById("1", RequireColumnsType.None);

            GeoBound tempBound = highlightFeature.GetBound();
            tempBound.ScaleDown(20);
            crossLineFeature = new Feature(new GeoLine(tempBound.GetVertices().Skip(1)));

            MemoryLayer highlightLayer = new MemoryLayer();
            highlightLayer.Features.Add(highlightFeature);
            highlightLayer.Features.Add(crossLineFeature);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            highlightLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#00BCD4"), 4));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            MemoryLayer resultLayer = new MemoryLayer { Name = "ResultLayer" };
            resultLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#99FF5722"), GeoColors.White));
            Map1.AddDynamicLayers("ResultOverlay", resultLayer);

            Map1.ZoomTo(highlightFeature);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer resultLayer = Map1.FindLayer<MemoryLayer>("ResultLayer");

            if (resultLayer.Features.Count == 0)
            {
                Collection<GeoPoint> crossingPoints = highlightFeature.Geometry.GetCrossing(crossLineFeature.Geometry);
                resultLayer.Features.AddRange(crossingPoints.Select(p => new Feature(p)));
                Map1.Refresh("ResultOverlay");
            }
        }
    }
}
