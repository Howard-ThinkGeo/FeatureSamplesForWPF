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
    public partial class GetDistanceView : UserControl
    {
        private Feature feature1;
        private Feature feature2;
        private GetDistanceViewModel viewModel;

        public GetDistanceView()
        {
            InitializeComponent();
            ResultPanel.DataContext = viewModel = new GetDistanceViewModel();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileSource dataSource = new ShapefileSource("SampleData/countries-900913.shp");
            dataSource.Open();
            feature1 = dataSource.GetFeatureById("1", RequireColumnsType.None);
            feature2 = dataSource.GetFeatureById("10", RequireColumnsType.None);

            MemoryLayer highlightLayer = new MemoryLayer();
            highlightLayer.Features.Add(feature1);
            highlightLayer.Features.Add(feature2);
            highlightLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D")));
            Map1.AddStaticLayers("HighlightOverlay", highlightLayer);

            MemoryLayer resultLayer = new MemoryLayer { Name = "ResultLayer" };
            resultLayer.Styles.Add(new SymbolStyle(SymbolType.Circle, GeoColor.FromHtml("#99FF5722"), GeoColors.White));
            resultLayer.Styles.Add(new LineStyle(GeoColor.FromHtml("#00BCD4"), 4));
            Map1.AddDynamicLayers("ResultOverlay", resultLayer);

            Map1.ZoomTo(GeoBound.CreateToInclude(new[] { feature1, feature2 }));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsCalculating = true;
            viewModel.DistanceInMeter = await Task.Run(() => feature1.Geometry.GetDistanceTo(feature2.Geometry));
            viewModel.IsCalculating = false;
        }
    }

    public class GetDistanceViewModel : INotifyPropertyChanged
    {
        private List<DistanceUnitModel> units;
        private DistanceUnitModel selectedUnit;
        private double distanceInMeter;
        private bool isCalculating;

        public event PropertyChangedEventHandler PropertyChanged;

        public GetDistanceViewModel()
        {
            distanceInMeter = double.NaN;
            units = new List<DistanceUnitModel>();
            units.Add(new DistanceUnitModel(LengthUnit.Feet, "ft."));
            units.Add(new DistanceUnitModel(LengthUnit.Kilometer, "km."));
            units.Add(new DistanceUnitModel(LengthUnit.Meter, "m"));
            units.Add(new DistanceUnitModel(LengthUnit.Mile, "mi."));
            units.Add(new DistanceUnitModel(LengthUnit.UsSurveyFeet, "us-ft."));
            units.Add(new DistanceUnitModel(LengthUnit.Yard, "yd."));

            selectedUnit = units[2];
        }

        public List<DistanceUnitModel> Units
        {
            get { return units; }
        }

        public DistanceUnitModel SelectedUnit
        {
            get { return selectedUnit; }
            set
            {
                if (selectedUnit == value) return;

                selectedUnit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUnit)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DistanceInSelectedUnit)));
            }
        }

        public double DistanceInMeter
        {
            get { return distanceInMeter; }
            set
            {
                if (distanceInMeter == value) return;

                distanceInMeter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DistanceInMeter)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DistanceInSelectedUnit)));
            }
        }

        public double DistanceInSelectedUnit
        {
            get
            {
                return GeoConversion.ConvertLength(DistanceInMeter, LengthUnit.Meter, SelectedUnit.Unit);
            }
        }

        public bool IsCalculating
        {
            get { return isCalculating; }
            set
            {
                if (isCalculating == value) return;
                isCalculating = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCalculating)));
            }
        }
    }

    public class DistanceUnitModel
    {
        public DistanceUnitModel(LengthUnit unit, string name)
        {
            Name = name;
            Unit = unit;
        }
        public string Name { get; set; }
        public LengthUnit Unit { get; set; }
    }
}
