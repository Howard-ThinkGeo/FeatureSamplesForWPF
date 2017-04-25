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
    public partial class GetAreaView : UserControl
    {
        private ShapefileLayer dataLayer;

        public GetAreaView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            dataLayer = new ShapefileLayer("SampleData/sections-900913.shp");
            dataLayer.Styles.Add(new FillStyle(GeoColors.Transparent, GeoColor.FromHtml("#99FAB04D"), 1));
            Map1.AddStaticLayers(dataLayer);

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Columns.Add(new FeatureColumn("Area", UnifiedColumnType.Text));
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#66FFFF00"), GeoColors.White));

            LabelStyle areaLabelStyle = new LabelStyle("Area", new GeoFont(), GeoColor.FromHtml("#111111"), GeoColors.White);
            highlightLayer.Styles.Add(areaLabelStyle);
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(new GeoBound(-10111509.577, 4582117.8006, -10099064.6735, 4598828.7496));
        }

        private void Map1_MapSingleClick(object sender, MapClickEventArgs e)
        {
            Feature identifiedFeature = IdentifyHelper.Identify(dataLayer, e.WorldCoordinate, Map1.CurrentScale, Map1.MapUnit).FirstOrDefault();
            if (identifiedFeature != null)
            {
                double areaInSquareKilometer = ((GeoAreaBase)identifiedFeature.Geometry).GetArea(GeoUnit.Meter, AreaUnit.SquareKilometers);
                identifiedFeature.FieldValues.Add("Area", $"{areaInSquareKilometer:N2} sq. km");

                MemoryLayer highlightLayer = Map1.FindLayer<MemoryLayer>("HighlightLayer");
                highlightLayer.Features.Clear();
                highlightLayer.Features.Add(identifiedFeature);

                Map1.Refresh("HighlightOverlay");
            }
        }
    }
}
