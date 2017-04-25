using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class StamenMapLayerView : UserControl
    {
        public StamenMapLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            StamenMapLayer stamenMapLayer = new StamenMapLayer();

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(stamenMapLayer);
            Map1.Overlays.Add(layerOverlay);

            // We can also use the code below to add a stamen maps to our map.
            // Map1.UseStamenMapAsBaseMap(StamenMapType.Toner);

            Map1.ZoomToFullBound();
        }

        private void MapTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StamenMapLayer stamenMapLayer = Map1.FindLayer<StamenMapLayer>("Stamen Map");
            if (stamenMapLayer != null)
            {
                stamenMapLayer.MapType = (StamenMapType)MapTypeComboBox.SelectedItem;
                Map1.Refresh(RefreshType.Redraw);
            }
        }
    }
}
