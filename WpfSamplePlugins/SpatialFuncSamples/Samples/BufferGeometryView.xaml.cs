using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class BufferGeometryView : UserControl
    {
        private Feature highlightFeature;

        public BufferGeometryView()
        {
            InitializeComponent();

            BufferDistanceComboBox.ItemsSource = new[] { 50000d, 100000d, 200000d };
            BufferDistanceComboBox.SelectedIndex = 0;
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileSource dataSource = new ShapefileSource("SampleData/countries-900913.shp");
            dataSource.Open();
            highlightFeature = dataSource.GetFeatureById("1", RequireColumnsType.None);

            MemoryLayer bufferedLayer = new MemoryLayer { Name = "BufferResultLayer" };
            bufferedLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#553F8CB4"), GeoColors.White));
            Map1.AddDynamicLayers("BufferedOverlay", bufferedLayer);

            MemoryLayer highlightLayer = new MemoryLayer();
            highlightLayer.Features.Add(highlightFeature);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(highlightFeature);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer bufferResultLayer = Map1.FindLayer<MemoryLayer>("BufferResultLayer");

            Feature bufferedFeature = new Feature(highlightFeature.Geometry.GetBuffer((double)BufferDistanceComboBox.SelectedValue));
            bufferResultLayer.Features.Clear();
            if (bufferedFeature.Geometry != null)
            {
                bufferResultLayer.Features.Add(bufferedFeature);
                Map1.Refresh("BufferedOverlay");
            }
        }
    }
}
