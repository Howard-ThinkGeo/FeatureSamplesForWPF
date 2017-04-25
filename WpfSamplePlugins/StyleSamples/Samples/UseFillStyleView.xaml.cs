using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class UseFillStyleView : UserControl
    {
        public UseFillStyleView()
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
            areaLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White, 1));
            Map1.AddStaticLayers("AreaOverlay", areaLayer);

            Map1.ZoomTo(areaLayer.GetBound());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShapefileLayer areaLayer = Map1.FindLayer<ShapefileLayer>("countries-900913");
            areaLayer.UseRandomStyle(120);
            Map1.Refresh("AreaOverlay");
        }
    }
}
