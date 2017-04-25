using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public class SampleTemplate : Control
    {
        public static readonly DependencyProperty MapContentProperty =
            DependencyProperty.Register(nameof(MapContent), typeof(object), typeof(SampleTemplate), new PropertyMetadata(null));


        public static readonly DependencyProperty ControlContentProperty =
            DependencyProperty.Register("ControlContent", typeof(object), typeof(SampleTemplate), new PropertyMetadata(null));

        static SampleTemplate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SampleTemplate), new FrameworkPropertyMetadata(typeof(SampleTemplate)));
        }

        public object MapContent
        {
            get { return (object)GetValue(MapContentProperty); }
            set { SetValue(MapContentProperty, value); }
        }

        public object ControlContent
        {
            get { return (object)GetValue(ControlContentProperty); }
            set { SetValue(ControlContentProperty, value); }
        }
    }
}
