using SlimGis.MapKit.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for CharView.xaml
    /// </summary>
    public partial class CharView : UserControl
    {
        private static List<int> characters;
        private static List<string> fontFamilies;

        public CharView()
        {
            InitializeComponent();
        }

        public static List<int> Characters
        {
            get
            {
                if (characters != null) return characters;

                characters = new List<int>();
                Enumerable.Range(33, 222).ForEach(i => characters.Add(i));
                return characters;
            }
        }

        public static List<string> FontFamilies
        {
            get
            {
                if (fontFamilies != null) return fontFamilies;

                fontFamilies = System.Drawing.FontFamily.Families.Select(f => f.Name).ToList();
                return fontFamilies;
            }
        }
    }

    public class CharViewModel : INotifyPropertyChanged
    {
        private string fontFamily;
        private int charIndex;

        public CharViewModel()
        {
            fontFamily = "Symbol";
            charIndex = 169;
        }

        public string FontFamily
        {
            get
            {
                return fontFamily;
            }

            set
            {
                fontFamily = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontFamily)));
            }
        }

        public int CharIndex
        {
            get
            {
                return charIndex;
            }

            set
            {
                charIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CharIndex)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
