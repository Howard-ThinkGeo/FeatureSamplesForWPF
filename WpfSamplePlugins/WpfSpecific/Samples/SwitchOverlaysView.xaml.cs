using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for UseOverlaysView.xaml
    /// </summary>
    public partial class SwitchOverlaysView : UserControl
    {
        public SwitchOverlaysView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            Map1.AddStaticLayers("OpenStreetMap", new OpenStreetMapLayer());
            Map1.AddStaticLayers("StamenMap - Toner", new StamenMapLayer(StamenMapType.Toner));
            Map1.AddStaticLayers("StamenMap - Watercolor", new StamenMapLayer(StamenMapType.Watercolor));

            ShapefileLayer countriesLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            countriesLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#FAB04D"), GeoColors.White));
            Map1.AddStaticLayers("Countries", countriesLayer);

            Map1.Overlays.Skip(1).ForEach(o => o.IsVisible = false);
            Map1.ZoomToFullBound();

            List<OverlayViewModel> viewModel = new List<OverlayViewModel>();
            viewModel.AddRange(Map1.Overlays.Select(o => new OverlayViewModel(o)));
            OverlayListGrid.DataContext = viewModel;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            List<OverlayViewModel> overlays = (List<OverlayViewModel>)OverlayListGrid.DataContext;
            RadioButton radioButton = (RadioButton)sender;
            OverlayViewModel overlayViewModel = (OverlayViewModel)radioButton.DataContext;
            if (overlayViewModel.IsVisible)
            {
                foreach (OverlayViewModel overlay in overlays)
                {
                    overlay.IsVisible = overlayViewModel == overlay;
                }
            }
        }
    }
}
