using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class FindClosestPointView : UserControl
    {
        private Feature highlightFeature;

        public FindClosestPointView()
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

            MemoryLayer highlightLayer = new MemoryLayer();
            highlightLayer.Features.Add(highlightFeature);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(highlightFeature);
        }

        private void Map1_MapMouseMove(object sender, MapKit.Wpf.MapMouseMoveEventArgs e)
        {
            FindClosestPoint(e);
        }

        private void FindClosestPoint(MapMouseMoveEventArgs e)
        {
            MemoryLayer closetPointLayer = Map1.FindLayer<MemoryLayer>("ClosetPointLayer");
            if (closetPointLayer == null)
            {
                closetPointLayer = new MemoryLayer { Name = "ClosetPointLayer" };
                closetPointLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#AA3F8CB4"), GeoColors.White));
                Map1.AddDynamicLayers("ClosestPointOverlay", closetPointLayer);
            }

            GeoPoint closestPoint = highlightFeature.Geometry.GetClosestPointTo(new GeoPoint(e.WorldCoordinate));
            closetPointLayer.Features.Clear();
            if (closestPoint != null)
            {
                closetPointLayer.Features.Add(new Feature(closestPoint));
                Map1.Refresh("ClosestPointOverlay");
            }
        }
    }
}
