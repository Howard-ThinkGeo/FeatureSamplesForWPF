using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class UseGoogleProjectionView : UserControl
    {
        public UseGoogleProjectionView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Degree;

            ShapefileLayer featureLayer = new ShapefileLayer("SampleData/countries-wgs84.shp");
            featureLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#FAB04D"), GeoColors.White));
            Map1.AddStaticLayers("countries-wgs84", featureLayer);

            Map1.ZoomToFullBound();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Map1.MapUnit == GeoUnit.Meter) return;

            Map1.MapUnit = GeoUnit.Meter;
            FeatureLayer featureLayer = Map1.FindLayer<ShapefileLayer>("countries-wgs84");
            SpatialReference srsSource = SpatialReferences.GetWgs84();
            SpatialReference srsTarget = SpatialReferences.GetSphericalMercator();
            featureLayer.Source.Projection = new Proj4Projection(srsSource, srsTarget);

            Map1.Overlays["countries-wgs84"].Invalidate();
            Map1.ZoomToFullBound();
        }
    }
}
