using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for UseOverlaysView.xaml
    /// </summary>
    public partial class UseOverlaysView : UserControl
    {
        public UseOverlaysView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            LayerOverlay staticOverlay = new LayerOverlay { Name = "Static Overlay" };
            staticOverlay.Layers.Add(new OpenStreetMapLayer());
            Map1.Overlays.Add(staticOverlay);

            ShapefileLayer dynamicLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            dynamicLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));

            LayerOverlay dynamicOverlay = new LayerOverlay { Name = "Dynamic Overlay" };
            dynamicOverlay.Layers.Add(dynamicLayer);
            Map1.Overlays.Add(dynamicOverlay);

            Map1.ZoomToFullBound();

            Collection<OverlayViewModel> viewModel = new Collection<OverlayViewModel>();
            viewModel.Add(new OverlayViewModel(staticOverlay));
            viewModel.Add(new OverlayViewModel(dynamicOverlay));
            OverlayListGrid.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FeatureLayer featureLayer = Map1.FindLayer<FeatureLayer>("countries-900913");
            featureLayer.UseRandomStyle();
            Map1.Refresh("Dynamic Overlay");
        }
    }
}
