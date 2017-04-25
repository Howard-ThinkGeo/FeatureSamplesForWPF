using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for LegendView.xaml
    /// </summary>
    public partial class LegendView : UserControl
    {
        private ObservableCollection<LegendViewItem> legendItems;

        public LegendView()
        {
            InitializeComponent();
            legendItems = new ObservableCollection<LegendViewItem>();
            LegendListView.ItemsSource = legendItems;
        }

        public void Update(IEnumerable<Style> styles)
        {
            legendItems.Clear();
            foreach (var style in styles)
            {
                LegendViewItem legendItem = new LegendViewItem();
                legendItem.Title = style.Name;
                legendItem.Icon = style.GetThumbnail(new GeoSize(26, 26)).GetImage();
                legendItems.Add(legendItem);
            }
        }
    }
}
