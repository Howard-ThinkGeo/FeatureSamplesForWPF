using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for MemoryLayerView.xaml
    /// </summary>
    public partial class MemoryLayerView : UserControl
    {
        public MemoryLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileSource dataSource = new ShapefileSource("SampleData/countries-900913.shp");
            Feature tempFeature = dataSource.GetFeatureById("1", RequireColumnsType.None);

            MemoryLayer memoryLayer = new MemoryLayer { Name = "ResultLayer" };
            memoryLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));
            memoryLayer.Features.Add(tempFeature);

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(memoryLayer);
            Map1.Overlays.Add(layerOverlay);

            Map1.ZoomTo(tempFeature);
        }
    }
}
