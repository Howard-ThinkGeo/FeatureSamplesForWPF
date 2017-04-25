using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for BufferGeometrySample.xaml
    /// </summary>
    public partial class TransformAGeometryView : UserControl
    {
        private Feature highlightFeature;
        private Feature transformingFeature;

        public TransformAGeometryView()
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
            transformingFeature = highlightFeature.Clone();

            MemoryLayer highlightLayer = new MemoryLayer();
            highlightLayer.Features.Add(transformingFeature);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(highlightFeature);
        }

        private void ScaleUpButton_Click(object sender, RoutedEventArgs e)
        {
            transformingFeature.Geometry.ScaleUp(10);
            Map1.Refresh("HighlightOverlay");
        }

        private void ScaleDownButton_Click(object sender, RoutedEventArgs e)
        {
            transformingFeature.Geometry.ScaleDown(10);
            Map1.Refresh("HighlightOverlay");
        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            transformingFeature.Geometry.Rotate(10);
            Map1.Refresh("HighlightOverlay");
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            transformingFeature.Geometry = highlightFeature.Geometry.Clone();
            Map1.Refresh("HighlightOverlay");
        }
    }
}
