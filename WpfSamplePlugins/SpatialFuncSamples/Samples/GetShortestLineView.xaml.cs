using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using SlimGis.MapKit.Utilities;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for FindClosestPointSample.xaml
    /// </summary>
    public partial class GetShortestLineView : UserControl
    {
        private Feature feature1;
        private Feature feature2;

        public GetShortestLineView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileSource dataSource = new ShapefileSource("SampleData/countries-900913.shp");
            dataSource.Open();
            feature1 = new Feature(dataSource.GetFeatureById("1", RequireColumnsType.None).GetBound());
            feature2 = new Feature(dataSource.GetFeatureById("10", RequireColumnsType.None).GetBound());

            MemoryLayer highlightLayer = new MemoryLayer();
            highlightLayer.Features.Add(feature1);
            highlightLayer.Features.Add(feature2);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            MemoryLayer resultLayer = new MemoryLayer { Name = "ResultLayer" };
            resultLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#00BCD4"), 4));
            resultLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#99FF5722"), GeoColors.White));
            Map1.AddDynamicLayers("ResultOverlay", resultLayer);

            Map1.ZoomTo(GeoBound.CreateToInclude(new[] { feature1, feature2 }));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryLayer resultLayer = Map1.FindLayer<MemoryLayer>("ResultLayer");
            if (resultLayer.Features.Count == 0)
            {
                GeoMultiLine shortestResult = await Task.Run(() => feature1.Geometry.GetShortestLineTo(feature2.Geometry));
                GeoLine shortestLine = shortestResult.Lines.First();

                resultLayer.Features.Add(new Feature(shortestLine));
                resultLayer.Features.Add(new Feature(new GeoPoint(shortestLine.Coordinates.First())));
                resultLayer.Features.Add(new Feature(new GeoPoint(shortestLine.Coordinates.Last())));
                Map1.Refresh("ResultOverlay");
            }
        }
    }
}
