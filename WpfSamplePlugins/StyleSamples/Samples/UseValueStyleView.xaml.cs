using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for UseValueStyleView.xaml
    /// </summary>
    public partial class UseValueStyleView : UserControl
    {
        private static readonly string columnName = "COLOR_MAP";

        private GeoColorFamily colorFamily;
        private string[] values;

        public UseValueStyleView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileLayer areaLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            Map1.AddStaticLayers("AreaOverlay", areaLayer);
            Map1.CurrentBound = areaLayer.GetBound();
            Refresh();
        }

        private void GeoColorFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colorFamily = (GeoColorFamily)GeoColorFamilyComboBox.SelectedItem;
            Refresh();
        }

        private void Refresh()
        {
            ShapefileLayer areaLayer = Map1.FindLayer<ShapefileLayer>("countries-900913");
            if (areaLayer == null) return;
            if (values == null) values = areaLayer.Source.GetUniqueFieldValues(columnName).Distinct().ToArray();

            areaLayer.Styles.Clear();
            ValueStyle valueStyle = ValueStyle.Create(columnName, values, DimensionType.Area, colorFamily, new GeoColor(255, 108, 144, 150), 0.01f);
            areaLayer.Styles.Add(valueStyle);

            LegendTitleTextBlock.Text = $"Thematic by \"{columnName}\" column";
            LegendView.Update(valueStyle.ValueItems.OrderBy(i => i.Value).SelectMany(i =>
              {
                  foreach (var style in i.Styles) style.Name = $"{columnName} = \"{i.Value}\"";
                  return i.Styles;
              }));
            Map1.Refresh(RefreshType.Redraw);
        }
    }
}
