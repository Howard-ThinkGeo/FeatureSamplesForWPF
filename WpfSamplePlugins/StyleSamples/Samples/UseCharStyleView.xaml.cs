using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class UseCharStyleView : UserControl
    {
        private CharViewModel charViewModel;

        public UseCharStyleView()
        {
            InitializeComponent();

            SymbolSizeComboBox.ItemsSource = new[] { 20, 30, 40 };
            SymbolSizeComboBox.SelectedIndex = 1;

            CharView.DataContext = charViewModel = new CharViewModel();
            charViewModel.PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == "CharIndex") UpdateStyle();
            };
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

            ShapefileLayer pointLayer = new ShapefileLayer("SampleData/cities-900913.shp");

            MapKit.Symbologies.FontStyle fontStyle = new MapKit.Symbologies.FontStyle();
            fontStyle.Brush = GeoBrushes.Red;
            fontStyle.OutlinePen = new GeoPen(GeoColors.White, 2);
            fontStyle.Margin = 40;
            pointLayer.Styles.Add(fontStyle);
            Map1.AddStaticLayers("PointOverlay", pointLayer);

            SyncFontStyleSettings(pointLayer);

            Map1.ZoomTo(pointLayer.GetBound());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShapefileLayer pointLayer = Map1.FindLayer<ShapefileLayer>("cities-900913");
            SyncFontStyleSettings(pointLayer, true);
            Map1.Refresh("PointOverlay");
        }

        private void SynbolSettingsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStyle();
        }

        private void SyncFontStyleSettings(FeatureLayer featureLayer, bool randomColor = false)
        {
            MapKit.Symbologies.FontStyle fontStyle = (MapKit.Symbologies.FontStyle)featureLayer.Styles[0];
            fontStyle.Font = new GeoFont(charViewModel.FontFamily, (int)SymbolSizeComboBox.SelectedValue);
            fontStyle.CharacterIndex = charViewModel.CharIndex;
            if (randomColor) fontStyle.Brush = new GeoSolidBrush(GeoColor.GetRandom());
         }

        private void UpdateStyle()
        {
            ShapefileLayer pointLayer = Map1.FindLayer<ShapefileLayer>("cities-900913");
            if (pointLayer == null) return;

            SyncFontStyleSettings(pointLayer);
            Map1.Refresh("PointOverlay");
        }
    }
}
