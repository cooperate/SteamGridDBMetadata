using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGDBMetadata
{
    public class SGDBMetadataSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string CoverStyle { get; set; } = "any";
        public string CoverDimension { get; set; } = "600x900";
        public string CoverNsfw { get; set; } = "false";
        public string CoverHumor { get; set; } = "false";
        public string BackgroundStyle { get; set; } = "any";
        public string BackgroundDimension { get; set; } = "any";
        public string BackgroundNsfw { get; set; } = "false";
        public string BackgroundHumor { get; set; } = "false";
        public string IconAssetSelection { get; set; } = "icons";
        public string IconNsfw { get; set; } = "false";
        public string IconHumor { get; set; } = "false";

        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonDontSerialize` ignore attribute.
        [DontSerialize]
        public bool OptionThatWontBeSaved { get; set; } = false;
    }

    public class SGDBMetadataSettingsViewModel : ObservableObject, ISettings
    {
        private readonly SGDBMetadata plugin;
        private SGDBMetadataSettings editingClone { get; set; }

        private SGDBMetadataSettings settings;
        public SGDBMetadataSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }

        public SGDBMetadataSettingsViewModel(SGDBMetadata plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<SGDBMetadataSettings>();

            // LoadPluginSettings returns null if not saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new SGDBMetadataSettings();
            }
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.
            plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.
            errors = new List<string>();
            return true;
        }
    }
}
