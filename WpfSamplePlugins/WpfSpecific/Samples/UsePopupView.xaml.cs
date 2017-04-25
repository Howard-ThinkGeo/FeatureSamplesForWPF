using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SlimGis.Samples
{
    public partial class UsePopupView : UserControl
    {
        public UsePopupView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.AddStaticLayers("OpenStreetMap", new OpenStreetMapLayer());

            GeoBound currentBound = new GeoBound(1534877.2788, 5720521.3921, 1536797.864, 5721952.8921);
            Map1.ZoomTo(currentBound);
        }

        private void Map1_MapSingleClick(object sender, MapClickEventArgs e)
        {
            Window window = new Window
            {
                Title = "Mark A Place",
                Content = new PopupPromptUserControl(),
                SizeToContent = SizeToContent.Height,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 280
            };

            PopupPromptViewModel promptVM;
            window.DataContext = promptVM = new PopupPromptViewModel();
            promptVM.Confirm = () => window.DialogResult = true;
            if (window.ShowDialog().Value)
            {
                PopupContentView popupContent = new PopupContentView();
                popupContent.DataContext = promptVM;

                Popup popup = new Popup();
                popup.Content = popupContent;
                popup.Width = 260;
                popup.Height = 200;
                popup.Location = e.WorldCoordinate;
                Map1.Placements.Add(popup);
            }
        }
    }

    public class PopupPromptViewModel : INotifyPropertyChanged
    {
        private POIType poiType;
        private string name = "New place";
        private string description = "Demo: The museum is at the foot of the Bridge, where the first Selma march was stopped on “Bloody Sunday,” March 7, 1965, by law enforcement officers who unleashed tear gas and billy clubs on protesters.";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public POIType POIType
        {
            get
            {
                if (poiType == null) poiType = POITypes.First();
                return poiType;
            }
            set
            {
                poiType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(POIType)));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public List<POIType> POITypes => SlimGis.Samples.POITypes.AllTypes;

        public Action Confirm { get; set; }
    }

    public class POIType
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }
    }

    public static class POITypes
    {
        private static List<POIType> all;
        public static List<POIType> AllTypes
        {
            get
            {
                if (all == null)
                {
                    all = new List<POIType>();
                    all.Add(new POIType { Name = "Park", Image = new BitmapImage(new Uri("pack://application:,,,/SlimGis.Samples.WpfSpecific;component/Images/Park.png", UriKind.RelativeOrAbsolute)) });
                    all.Add(new POIType { Name = "Sports", Image = new BitmapImage(new Uri("pack://application:,,,/SlimGis.Samples.WpfSpecific;component/Images/Sports.png", UriKind.RelativeOrAbsolute)) });
                    all.Add(new POIType { Name = "Bank", Image = new BitmapImage(new Uri("pack://application:,,,/SlimGis.Samples.WpfSpecific;component/Images/Bank.png", UriKind.RelativeOrAbsolute)) });
                }
                return all;
            }
        }
    }
}
