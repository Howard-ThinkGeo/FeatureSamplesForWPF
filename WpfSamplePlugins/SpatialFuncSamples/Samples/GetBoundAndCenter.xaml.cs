using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class GetBoundAndCenter : UserControl
    {
        private ShapefileLayer dataLayer;

        public GetBoundAndCenter()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            dataLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            dataLayer.Open();

            GeoPen outlinePen = new GeoPen(GeoColor.FromHtml("#00BCD4"), 2) { DashPattern = new float[] { 4, 4 } };

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            highlightLayer.Styles.Add(new LineStyle(outlinePen));
            highlightLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#99FF5722"), GeoColors.White));
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

                GeoBound identifiedBound = identifiedFeature.GetBound();
                highlightLayer.Features.Add(new Feature(new GeoLine(identifiedBound.GetVertices())));

                GeoPoint identifiedCenter = identifiedBound.GetCentroid();
                highlightLayer.Features.Add(new Feature(identifiedCenter));

                Map1.Refresh("HighlightOverlay");
            }
        }
    }
}
