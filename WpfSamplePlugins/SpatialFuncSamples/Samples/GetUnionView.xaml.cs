using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class GetUnionView : UserControl
    {
        private Feature feature1;
        private Feature feature2;

        public GetUnionView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            GeoBound bound = new GeoBound(2171997.6512, 8356849.2669, 3515687.9933, 11097616.86);
            GeoPoint center = bound.GetCentroid();
            double x1 = bound.MinX + bound.Width * .25;
            double y = center.Y;
            double x2 = bound.MaxX - bound.Width * .25;
            double radius = bound.Width * 3 / 8;

            feature1 = new Feature(new GeoEllipse(new GeoPoint(x1, y), radius));
            feature2 = new Feature(new GeoEllipse(new GeoPoint(x2, y), radius));

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Features.Add(feature1);
            highlightLayer.Features.Add(feature2);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            MemoryLayer resultLayer = new MemoryLayer { Name = "ResultLayer" };
            resultLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));
            Map1.AddDynamicLayers("ResultOverlay", resultLayer);

            Map1.ZoomTo(bound);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer resultLayer = Map1.FindLayer<MemoryLayer>("ResultLayer");

            if (resultLayer.Features.Count == 0)
            {
                Map1.FindLayer<MemoryLayer>("HighlightLayer").Features.Clear();

                GeoAreaBase unionResult = ((GeoAreaBase)feature1.Geometry).Union((GeoAreaBase)feature2.Geometry);
                resultLayer.Features.Add(new Feature(unionResult));
                Map1.Refresh("HighlightOverlay", "ResultOverlay");
            }
        }
    }
}
