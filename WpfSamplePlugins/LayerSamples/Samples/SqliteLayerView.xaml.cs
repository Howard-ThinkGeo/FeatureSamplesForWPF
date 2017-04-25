using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for SqliteLayerView.xaml
    /// </summary>
    public partial class SqliteLayerView : UserControl
    {
        public SqliteLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            string connectionString = SqliteSource.CreateConnectionString("SampleData/usstates-900913.sqlite");
            SqliteLayer sqliteLayer = new SqliteLayer(connectionString, "table_name", "id", "geometry");
            sqliteLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(sqliteLayer);
            Map1.Overlays.Add(layerOverlay);

            Map1.ZoomTo(sqliteLayer.GetBound());
        }
    }
}
