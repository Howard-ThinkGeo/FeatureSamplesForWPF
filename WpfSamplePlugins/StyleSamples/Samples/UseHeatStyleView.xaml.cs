using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for UseHeatStyleView.xaml
    /// </summary>
    public partial class UseHeatStyleView : UserControl
    {
        private static readonly string columnName = "MED_AGE";
        private ColorPaletteType colorPaletteType;

        public UseHeatStyleView()
        {
            InitializeComponent();
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Default);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Gray);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Rainbow);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.RedWhiteBlue);
            ColorPaletteTypeComboBox.Items.Add(ColorPaletteType.Thermal);
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileLayer pointLayer = new ShapefileLayer("SampleData/cities-900913.shp");
            HeatStyle heatStyle = new HeatStyle();
            heatStyle.DataColumn = columnName;
            heatStyle.MaxIntensity = 37.7;
            heatStyle.MinIntensity = 30;
            heatStyle.Alpha = 150;
            heatStyle.Radius = (2 - HeatPointSizeComboBox.SelectedIndex) * 30 + 20;
            heatStyle.ColorPaletteType = colorPaletteType;
            pointLayer.Styles.Add(heatStyle);
            Map1.AddStaticLayers("PointOverlay", pointLayer);

            Map1.ZoomTo(pointLayer.GetBound());
        }

        private void HeatPointSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void ColorPaletteTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorPaletteType = (ColorPaletteType)ColorPaletteTypeComboBox.SelectedItem;
            Refresh();
        }

        private void Refresh()
        {
            ShapefileLayer pointLayer = Map1.FindLayer<ShapefileLayer>("cities-900913");
            if (pointLayer == null) return;
            HeatStyle heatStyle = (HeatStyle)pointLayer.Styles[0];
            heatStyle.Radius = (2 - HeatPointSizeComboBox.SelectedIndex) * 30 + 20;
            heatStyle.ColorPaletteType = colorPaletteType;

            Map1.Refresh(RefreshType.Redraw);
        }

    }
}
