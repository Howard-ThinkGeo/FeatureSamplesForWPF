using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class GetWellKnownDataView : UserControl
    {
        private ShapefileLayer dataLayer;

        public GetWellKnownDataView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            dataLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            dataLayer.Open();

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White, 4));
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(new GeoBound(2171997.6512, 8356849.2669, 3515687.9933, 11097616.86));
        }

        private void Map1_MapSingleClick(object sender, MapClickEventArgs e)
        {
            Feature identifiedFeature = IdentifyHelper.Identify(dataLayer, e.WorldCoordinate, Map1.CurrentScale, Map1.MapUnit).FirstOrDefault();
            if (identifiedFeature != null)
            {
                MemoryLayer highlightLayer = Map1.FindLayer<MemoryLayer>("HighlightLayer");

                highlightLayer.Features.Clear();
                highlightLayer.Features.Add(identifiedFeature);

                WktTextBox.Text = identifiedFeature.ToWkt();
                WkbTextBox.Text = Convert.ToBase64String(identifiedFeature.ToWkb());

                Map1.Refresh("HighlightOverlay");
            }
        }
    }
}
