using SlimGis.MapKit.Geometries;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System;
using SlimGis.MapKit.Shared;
using System.Windows.Media.Animation;
using SlimGis.MapKit.Wpf;

namespace SlimGis.Samples
{
    public partial class RotateMapView : UserControl
    {
        public RotateMapView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();
            Map1.ZoomToFullBound();
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            RotateMap(sender, e);
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RotateMap(sender, e);
        }

        private void RotateMap(object sender, MouseEventArgs e)
        {
            Ellipse rotationPanel = (Ellipse)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(rotationPanel);
                HandlerPointTransform.X = currentPoint.X;
                HandlerPointTransform.Y = currentPoint.Y;

                double centerX = rotationPanel.Width * .5;
                double centerY = rotationPanel.Height * .5;
                double rotation = Math.Atan2(currentPoint.Y - centerY, currentPoint.X - centerX);
                double rotationInDegree = rotation * 180 / Math.PI;

                while (rotationInDegree > 360) rotationInDegree -= 360;
                while (rotationInDegree < 0) rotationInDegree += 360;

                Map1.Rotation = rotationInDegree;
            }
        }
    }
}
