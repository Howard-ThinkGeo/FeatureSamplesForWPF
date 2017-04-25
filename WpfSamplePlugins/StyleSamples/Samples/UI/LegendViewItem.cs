using System.Windows.Media;

namespace SlimGis.Samples
{
    public class LegendViewItem
    {
        private string title;
        private ImageSource icon;

        public LegendViewItem()
            : this(string.Empty, null)
        { }

        public LegendViewItem(string title, ImageSource icon)
        {
            this.icon = icon;
            this.title = title;
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public ImageSource Icon
        {
            get { return icon; }
            set { icon = value; }
        }
    }
}
