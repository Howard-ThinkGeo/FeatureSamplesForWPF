using SlimGis.MapKit.Wpf;
using System.ComponentModel;

namespace SlimGis.Samples
{
    public class OverlayViewModel : INotifyPropertyChanged
    {
        private Overlay overlay;

        public OverlayViewModel(Overlay overlay)
        {
            this.overlay = overlay;
        }

        public string Name => overlay.Name;

        public double Opacity
        {
            get { return overlay.Container.Opacity * 10; }
            set
            {
                if (overlay.Container.Opacity * 10 == value) return;
                overlay.Container.Opacity = value / 10d;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity)));
            }
        }

        public bool IsVisible
        {
            get { return overlay.IsVisible; }

            set
            {
                if (overlay.IsVisible == value) return;
                overlay.IsVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisible)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
