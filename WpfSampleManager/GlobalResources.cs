using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SlimGis.Samples
{
    public class GlobalResources : INotifyPropertyChanged
    {
        private static GlobalResources instance;
        private ObservableCollection<SampleCategoryPlugin> sampleCategories;
        private SampleModel selectedSample;
        private bool isLoaded;
        private Visibility loadSamplesFailedVisibility;
        private Visibility listPanelVisibility;

        public event PropertyChangedEventHandler PropertyChanged;

        public GlobalResources()
        {
            sampleCategories = new ObservableCollection<SampleCategoryPlugin>();
            loadSamplesFailedVisibility = Visibility.Collapsed;
            listPanelVisibility = Visibility.Visible;
        }

        public static GlobalResources Instance
        {
            get
            {
                return instance ?? (instance = new GlobalResources());
            }
        }

        public async Task LoadAsync()
        {
            Collection<SampleCategoryPlugin> samplePlugins = await LoadSamplePluginsAsync();
            foreach (SampleCategoryPlugin samplePlugin in samplePlugins)
            {
                try
                {
                    //SampleModel sampleModel = new SampleModel();
                    //sampleModel.Name = samplePlugin.Name;
                    //sampleModel.Description = string.Empty;
                    //foreach (var sample in samplePlugin.Samples)
                    //{
                    //    sampleModel.Samples.Add(sample);
                    //}

                    Samples.Add(samplePlugin);
                }
                catch { }
            }

            IsLoaded = true;
        }

        private async Task<Collection<SampleCategoryPlugin>> LoadSamplePluginsAsync()
        {
            return await Task.Run(() =>
            {
                string entryDllPath = Assembly.GetEntryAssembly().Location;
                string entryDir = Directory.GetParent(Path.GetDirectoryName(entryDllPath)).FullName;
                string[] dllPaths = Directory.GetFiles(entryDir, "*.dll", SearchOption.AllDirectories).Where(f => !f.Equals(entryDllPath)).ToArray();

                AggregateCatalog catalog = new AggregateCatalog();
                foreach (var dllPath in dllPaths)
                {
                    if (dllPath.Contains(@"\obj\") || !Path.GetFileName(dllPath).StartsWith("SlimGis.Samples.")) continue;
                    try
                    {
                        AssemblyCatalog assemblyCatalog = new AssemblyCatalog(Assembly.LoadFile(dllPath));
                        catalog.Catalogs.Add(assemblyCatalog);
                    }
                    catch { }
                }

                CompositionContainer container = new CompositionContainer(catalog);
                Collection<SampleCategoryPlugin> plugins = new Collection<SampleCategoryPlugin>();
                foreach (var samplePluginExport in container.GetExports<SampleCategoryPlugin>())
                {
                    try
                    {
                        SampleCategoryPlugin sampleCategoryPlugin = samplePluginExport.Value;
                        plugins.Add(sampleCategoryPlugin);
                    }
                    catch { }
                }

                return plugins;
            });
        }

        public ObservableCollection<SampleCategoryPlugin> Samples => sampleCategories;

        public bool IsLoaded
        {
            get { return isLoaded; }
            set
            {
                if (isLoaded == value) return;
                isLoaded = value;

                CurrentSample = Samples.SelectMany(category => category.Samples).OfType<SampleModel>().FirstOrDefault();
                if (CurrentSample == null) LoadSamplesFailedVisibility = Visibility.Visible;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoaded)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Samples)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadingBarVisibility)));
            }
        }

        public Visibility LoadingBarVisibility { get { return IsLoaded ? Visibility.Collapsed : Visibility.Visible; } }

        public Visibility LoadSamplesFailedVisibility
        {
            get { return loadSamplesFailedVisibility; }
            set
            {
                if (loadSamplesFailedVisibility == value) return;
                loadSamplesFailedVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadSamplesFailedVisibility)));
            }
        }

        public Visibility ListPanelVisibility
        {
            get { return listPanelVisibility; }
            set
            {
                listPanelVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListPanelVisibility)));
            }
        }

        public SampleModel CurrentSample
        {
            get
            {
                return selectedSample;
            }

            set
            {
                if (selectedSample == value) return;

                selectedSample = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSample)));
            }
        }
    }
}
