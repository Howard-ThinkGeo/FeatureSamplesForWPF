using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class UseLabelStyleView : UserControl
    {
        public UseLabelStyleView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the Map1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileLayer areaLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            var feature = areaLayer.Source.GetFeatureById("1", RequireColumnsType.All);

            areaLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White, 1));

            LabelStyle labelStyle = new LabelStyle("CNTRY_NAME", new GeoFont { FontSize = 8 }, GeoColors.Black, GeoColors.White, 2);
            labelStyle.Margin = 100;
            areaLayer.Styles.Add(labelStyle);
            Map1.AddStaticLayers("AreaOverlay", areaLayer);

            Map1.ZoomTo(areaLayer.GetBound());
        }
    }
}
