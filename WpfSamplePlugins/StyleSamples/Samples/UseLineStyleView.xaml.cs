using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class UseLineStyleView : UserControl
    {
        public UseLineStyleView()
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

            ShapefileLayer lineLayer = new ShapefileLayer("SampleData/streets-900913.shp");
            lineLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#55FAB04D"), 1));
            Map1.AddStaticLayers("LineOverlay", lineLayer);

            GeoBound bound = lineLayer.GetBound();
            bound.ScaleDown(90);
            Map1.ZoomTo(bound);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShapefileLayer lineLayer = Map1.FindLayer<ShapefileLayer>("streets-900913");
            float[] dashPattern = ((LineStyle)lineLayer.Styles[0]).Pen.DashPattern;
            lineLayer.UseRandomStyle();
            ((LineStyle)lineLayer.Styles[0]).Pen.DashPattern = dashPattern;

            Map1.Refresh("LineOverlay");
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool useDash = ((CheckBox)sender).IsChecked.Value;

            ShapefileLayer lineLayer = Map1.FindLayer<ShapefileLayer>("streets-900913");
            ((LineStyle)lineLayer.Styles[0]).Pen.DashPattern = useDash ? new[] { 4f, 4f } : null;
            Map1.Refresh("LineOverlay");
        }
    }
}
