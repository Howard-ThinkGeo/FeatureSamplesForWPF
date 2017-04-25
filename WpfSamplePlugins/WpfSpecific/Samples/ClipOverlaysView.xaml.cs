using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SlimGis.Samples
{
    public partial class ClipOverlaysView : UserControl
    {
        public ClipOverlaysView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.AddStaticLayers("OpenStreetMap", new OpenStreetMapLayer());
            Map1.AddStaticLayers("StamenMap - Watercolor", new StamenMapLayer(StamenMapType.Terrain));
            Map1.ZoomToFullBound();

            UpdateClips();
            Map1.SizeChanged += Map1_SizeChanged;
        }

        private void Map1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateClips();
        }

        private void UpdateClips()
        {
            Overlay overlay1 = Map1.Overlays["OpenStreetMap"];
            Overlay overlay2 = Map1.Overlays["StamenMap - Watercolor"];

            double halfWidth = Map1.ActualWidth * .5;
            overlay1.Container.Clip = new RectangleGeometry(new Rect(0, 0, halfWidth, Map1.ActualHeight));
            overlay2.Container.Clip = new RectangleGeometry(new Rect(halfWidth, 0, halfWidth, Map1.ActualHeight));
        }
    }
}
