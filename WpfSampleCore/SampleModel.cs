using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SlimGis.Samples
{
    public class SampleModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility controlPanelVisibility;

        public SampleModel()
        {
            controlPanelVisibility = Visibility.Visible;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public Uri SourceUri { get; set; }

        public UserControl Content => GetContent();

        public Func<UserControl> GetContent { get; set; }

        public Visibility ControlPanelVisibility
        {
            get { return controlPanelVisibility; }
            set
            {
                controlPanelVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ControlPanelVisibility)));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [InheritedExport(typeof(SampleCategoryPlugin))]
    public abstract class SampleCategoryPlugin
    {
        private Collection<SampleModel> samples;

        protected SampleCategoryPlugin()
        { }

        public string Name { get; set; }

        public string SamplesListUri { get; set; }

        protected virtual Collection<SampleModel> GetSamples()
        {
            Collection<SampleModel> samples = new Collection<SampleModel>();
            using (Stream stream = Application.GetResourceStream(new Uri(SamplesListUri, UriKind.RelativeOrAbsolute)).Stream)
            {
                XElement root = XElement.Load(stream);
                foreach (var element in root.Elements("sample"))
                {
                    string name = element.Attribute("name").Value;
                    string type = element.Attribute("type").Value;
                    string uri = element.Attribute("uri").Value;
                    string description = element.Value;
                    SampleModel sampleModel = new SampleModel
                    {
                        Name = name,
                        Description = description,
                        SourceUri = new Uri(uri, UriKind.RelativeOrAbsolute),
                        GetContent = () => (UserControl)Activator.CreateInstance(GetType().Assembly.GetType($"SlimGis.Samples.{type}"))
                    };

                    samples.Add(sampleModel);
                }
            }

            return samples;
        }

        public Collection<SampleModel> Samples
        {
            get
            {
                if (samples == null)
                {
                    samples = new Collection<SampleModel>();
                    foreach (var sample in GetSamples())
                    {
                        samples.Add(sample);
                    }
                }

                return samples;
            }
        }
    }
}
