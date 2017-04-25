using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class UsePointStyleView : UserControl
    {
        public UsePointStyleView()
        {
            InitializeComponent();

            SymbolTypeComboBox.ItemsSource = Enum.GetValues(typeof(SymbolType));
            SymbolTypeComboBox.SelectedIndex = 0;

            SymbolSizeComboBox.ItemsSource = new[] { 20f, 30f, 40f };
            SymbolSizeComboBox.SelectedIndex = 1;
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

            SymbolStyle symbolStyle = new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#55FAB04D"), GeoColors.White, 2);
            symbolStyle.SymbolType = (SymbolType)SymbolTypeComboBox.SelectedValue;
            symbolStyle.Size = (float)SymbolSizeComboBox.SelectedValue;
            symbolStyle.Margin = 40;
            pointLayer.Styles.Add(symbolStyle);
            Map1.AddStaticLayers("PointOverlay", pointLayer);

            Map1.ZoomTo(pointLayer.GetBound());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShapefileLayer pointLayer = Map1.FindLayer<ShapefileLayer>("cities-900913");
            pointLayer.UseRandomStyle(120);

            SyncSymbolSettings(pointLayer);
        }

        private void SynbolSettingsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShapefileLayer pointLayer = Map1.FindLayer<ShapefileLayer>("cities-900913");
            if (pointLayer == null) return;

            SyncSymbolSettings(pointLayer);
        }

        private void SyncSymbolSettings(ShapefileLayer pointLayer)
        {
            SymbolStyle symbolStyle = (SymbolStyle)pointLayer.Styles[0];
            symbolStyle.SymbolType = (SymbolType)SymbolTypeComboBox.SelectedValue;
            symbolStyle.Size = (float)SymbolSizeComboBox.SelectedValue;
            symbolStyle.Margin = 40;

            Map1.Refresh("PointOverlay");
        }
    }
}
