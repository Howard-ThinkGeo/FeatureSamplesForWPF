using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for GridLayerView.xaml
    /// </summary>
    public partial class GridLayerView : UserControl
    {
        public GridLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            GridLayer gridLayer = new GridLayer("SampleData/gridfile-900913.grd");
            GridFileSource gridSource = (GridFileSource)gridLayer.Source;
            var cellValues = gridSource.GetDistinctCellValues();
            cellValues.Sort();

            ClassBreakStyle style = ClassBreakStyle.Create("CellValue", cellValues.Last(), cellValues.First(), 8, DimensionType.Area, GeoColorFamily.Hue, GeoColors.Red, 1f);
            style.Margin = 25;
            gridLayer.Styles.Add(style);

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(gridLayer);
            Map1.Overlays.Add(layerOverlay);

            Map1.ZoomTo(gridLayer.GetBound());
        }
    }
}
