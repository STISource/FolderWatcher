using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using FolderWatcher.Models;

namespace FolderWatcher.Services
{
    public class JsonSettingsService : ISettingsService
    {
        public readonly string SettingsPath = @"FolderWatcherSettings.json";

        public FolderWatcherSettings GetSettings()
        {
            if (File.Exists(this.SettingsPath))
            {
                FolderWatcherSettings settings = null;
                var serializer = new DataContractJsonSerializer(typeof(FolderWatcherSettings));

                using (var fileStream = new FileStream(this.SettingsPath, FileMode.Open))
                {
                    try
                    {
                        using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(fileStream, Encoding.GetEncoding("utf-8"), XmlDictionaryReaderQuotas.Max, null))
                        {
                            settings = (FolderWatcherSettings)serializer.ReadObject(jsonReader);
                        }
                    }
                    catch (Exception)
                    {
                        settings = null;                        
                    }
                }

                return settings ?? new FolderWatcherSettings
                {
                    Folders = new ObservableCollection<FolderDetails>()
                };
            }

            var blankSettings = new FolderWatcherSettings
            {
                Folders = new ObservableCollection<FolderDetails>()
            };

            this.SaveSettings(blankSettings);
            
            return blankSettings;
        }

        public void SaveSettings(FolderWatcherSettings settings)
        {
            var serializer = new DataContractJsonSerializer(typeof(FolderWatcherSettings));

            using (var fileStream = new FileStream(this.SettingsPath, FileMode.Create))
            {
                using (var jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(fileStream, Encoding.GetEncoding("utf-8")))
                {
                    serializer.WriteObject(jsonWriter, settings);
                    jsonWriter.Flush();
                }
            }
        }
    }
}
