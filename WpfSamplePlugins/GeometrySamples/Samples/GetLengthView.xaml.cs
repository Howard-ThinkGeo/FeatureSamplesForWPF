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
    public partial class GetLengthView : UserControl
    {
        private ShapefileLayer dataLayer;

        public GetLengthView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            dataLayer = new ShapefileLayer("SampleData/streets-900913.shp");
            dataLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#55FAB04D"), 4));
            Map1.AddStaticLayers(dataLayer);

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Columns.Add(new FeatureColumn("Distance", UnifiedColumnType.Text));
            highlightLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#8800BCD4"), 8));

            LabelStyle distanceLabelStyle = new LabelStyle("Distance", new GeoFont(), GeoColor.FromHtml("#111111"), GeoColors.White);
            distanceLabelStyle.LineSegmentRatio = 100;
            highlightLayer.Styles.Add(distanceLabelStyle);
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(new GeoBound(-10880446.205, 3540612.6137, -10879623.9531, 3541519.945));
        }

        private void Map1_MapSingleClick(object sender, MapClickEventArgs e)
        {
            Feature identifiedFeature = IdentifyHelper.Identify(dataLayer, e.WorldCoordinate, Map1.CurrentScale, Map1.MapUnit).FirstOrDefault();
            if (identifiedFeature != null)
            {
                double distanceInMeter = ((GeoLinearBase)identifiedFeature.Geometry).GetLength();
                identifiedFeature.FieldValues.Add("Distance", $"{distanceInMeter:N2} m");

                MemoryLayer highlightLayer = Map1.FindLayer<MemoryLayer>("HighlightLayer");

                highlightLayer.Features.Clear();
                highlightLayer.Features.Add(identifiedFeature);

                Map1.Refresh("HighlightOverlay");
            }
        }
    }
}
