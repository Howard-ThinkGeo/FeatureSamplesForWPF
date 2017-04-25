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
    public partial class GetDifferenceView : UserControl
    {
        private Feature highlightFeature;
        private Feature highlightBoundFeature;

        public GetDifferenceView()
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

            GeoBound highlightBound = highlightFeature.GetBound();
            highlightBoundFeature = new Feature(highlightBound);

            MemoryLayer highlightLayer = new MemoryLayer() { Name = "HighlightLayer" };
            highlightLayer.Features.Add(highlightFeature);
            highlightLayer.Features.Add(highlightBoundFeature);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            highlightLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#00BCD4"), 4));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            MemoryLayer resultLayer = new MemoryLayer { Name = "ResultLayer" };
            resultLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));
            Map1.AddDynamicLayers("ResultOverlay", resultLayer);

            Map1.ZoomTo(highlightFeature);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer resultLayer = Map1.FindLayer<MemoryLayer>("ResultLayer");

            if (resultLayer.Features.Count == 0)
            {
                Map1.FindLayer<MemoryLayer>("HighlightLayer").Features.Clear();

                Geometry difference = highlightBoundFeature.Geometry.GetDifference(highlightFeature.Geometry);
                resultLayer.Features.Add(new Feature(difference));
                Map1.Refresh("HighlightOverlay", "ResultOverlay");
            }
        }
    }
}
