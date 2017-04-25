using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class SpatialQueryView : UserControl
    {
        public SpatialQueryView()
        {
            InitializeComponent();

            QueryOptionComboBox.ItemsSource = new[]
            {
                SpatialQueryMode.Disjointed,
                SpatialQueryMode.Intersecting,
                SpatialQueryMode.Overlapping,
                SpatialQueryMode.Within
            };
            QueryOptionComboBox.SelectedIndex = 0;
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileLayer sectionLayer = new ShapefileLayer("SampleData/sections-900913.shp");
            sectionLayer.Styles.Add(new FillStyle(GeoColors.Transparent, GeoColor.FromHtml("#99FAB04D"), 1));
            Map1.AddStaticLayers("SectionOverlay", sectionLayer);

            GeoBound sectionBound = sectionLayer.GetBound();
            GeoBound queryArea = (GeoBound)sectionBound.Clone();
            queryArea.ScaleDown(60);

            MemoryLayer queryAreaLayer = new MemoryLayer { Name = "QueryAreaLayer" };
            queryAreaLayer.Styles.Add(new FillStyle(GeoColors.Transparent, GeoColor.FromHtml("#9900BCD4"), 4));
            queryAreaLayer.Features.Add(new Feature(queryArea));
            Map1.AddStaticLayers("SectionOverlay", queryAreaLayer);

            MemoryLayer highlightLayer = new MemoryLayer { Name = "HighlightLayer" };
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#66FFFF00"), GeoColors.White));
            Map1.AddDynamicLayers("HighlightOverlay", highlightLayer);

            Map1.ZoomTo(sectionLayer.GetBound());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShapefileLayer sectionLayer = Map1.FindLayer<ShapefileLayer>("sections-900913");
            MemoryLayer highlightLayer = Map1.FindLayer<MemoryLayer>("HighlightLayer");
            MemoryLayer queryAreaLayer = Map1.FindLayer<MemoryLayer>("QueryAreaLayer");
            Geometry queryArea = queryAreaLayer.Features.First().Geometry;
            IEnumerable<Feature> resultFeatures = sectionLayer.Source.SpatialQuery(queryArea, (SpatialQueryMode)QueryOptionComboBox.SelectedValue);
            highlightLayer.Features.Clear();
            foreach (var feature in resultFeatures)
            {
                highlightLayer.Features.Add(feature);
            }

            Map1.Refresh("HighlightOverlay");
        }
    }
}
