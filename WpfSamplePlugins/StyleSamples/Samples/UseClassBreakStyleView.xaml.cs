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
    /// Interaction logic for UseClassBreakStyleView.xaml
    /// </summary>
    public partial class UseClassBreakStyleView : UserControl
    {
        private static readonly string columnName = "POP_CNTRY";
        private GeoColorFamily colorFamily;

        public UseClassBreakStyleView()
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
            areaLayer.Styles.Clear();
            ClassBreakStyle classBreakStyle = ClassBreakStyle.Create(columnName, 281396894, 5000000, 11, DimensionType.Area, colorFamily, new GeoColor(50, 100, 200, 150), 0.01f);
            classBreakStyle.ClassBreaks[0].Value = 0;
            areaLayer.Styles.Add(classBreakStyle);

            LegendTitleTextBlock.Text = "The population of countries";
            ClassBreak[] classBreaks = classBreakStyle.ClassBreaks.OrderBy(i => i.Value).ToArray();
            for (int i = 0; i < classBreaks.Length; i++)
            {
                ClassBreak fromClassBreak = classBreaks[i];
                ClassBreak toClassBreak = i + 1 < classBreaks.Length ? classBreaks[i + 1] : null;
                foreach (var style in fromClassBreak.Styles)
                {
                    if (toClassBreak != null) style.Name = $"{(fromClassBreak.Value / 1000000).ToString("N0")} ~ {(toClassBreak.Value / 1000000).ToString("N0")} million";
                    else style.Name = $">= {(fromClassBreak.Value / 1000000).ToString("N0")} million";
                }
            }
            LegendView.Update(classBreaks.SelectMany(i => i.Styles));
            Map1.Refresh(RefreshType.Redraw);
        }
    }
}
