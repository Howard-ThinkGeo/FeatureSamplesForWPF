using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SlimGis.Samples
{
    public partial class UseMarkersView : UserControl
    {
        public UseMarkersView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.AddStaticLayers("OpenStreetMap", new OpenStreetMapLayer());

            GeoBound currentBound = new GeoBound(1534877.2788, 5720521.3921, 1536797.864, 5721952.8921);
            BitmapImage sourceImage = new BitmapImage(new Uri("pack://application:,,,/SlimGis.Samples.WpfSpecific;component/Images/6x9_icons_50.png", UriKind.RelativeOrAbsolute));

            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                double x = r.Next(1534877, 1536797);
                double y = r.Next(5720521, 5721952);
                AddMarker(x, y, GetRandomIcon(r, sourceImage));
            }

            Map1.ZoomTo(currentBound);
        }

        private void AddMarker(double worldX, double worldY, ImageSource markerImage)
        {
            Marker marker = new Marker();
            marker.Cursor = Cursors.Hand;
            marker.Location = new GeoCoordinate(worldX, worldY);
            marker.ImageSource = markerImage; 
            Map1.Placements.Add(marker);
        }

        private ImageSource GetRandomIcon(Random r, BitmapImage source)
        {
            int index = r.Next(0, 49);
            int row = (int)Math.Floor(index / 6d);
            int colInLastRow = index % 6;

            double cellWidth = source.Width / 6d;
            double cellHeight = source.Height / 9d;
            int top = (int)(row * cellHeight);
            int left = (int)(colInLastRow * cellWidth);
            CroppedBitmap target = new CroppedBitmap(source, new Int32Rect(left, top, (int)cellWidth, (int)cellHeight));
            return target;
        }

        private void ShowShadowCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Map1.Placements.OfType<Marker>().ForEach(marker => marker.DropShadow = checkBox.IsChecked.Value);
        }
    }
}
