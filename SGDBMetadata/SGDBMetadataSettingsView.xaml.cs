using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Playnite.SDK;

namespace SGDBMetadata
{
    public partial class SGDBMetadataSettingsView : UserControl
    {
         public SGDBMetadataSettingsView()
        {
            InitializeComponent();

            // Generic
            var cmbGenericNsfw = new Dictionary<string, string>
            {
                { "any", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericAny") },
                { "false", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericNotAdult") },
                { "true", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericOnlyAdult") },
            };

            var cmbGenericHumor = new Dictionary<string, string>
            {
                { "any", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericAny") },
                { "false", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericNotHumor") },
                { "true", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericOnlyHumor") },
            };


            // Cover items sources
            cmbCoverStyles.ItemsSource = new Dictionary<string, string>
            {
                { "any", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericAny") },
                { "alternate", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleAlternate") },
                { "blurred", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleBlurred") },
                { "white_logo", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleWhiteLogo") },
                { "material", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleMaterial") },
                { "no_logo", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleNoLogo") }
            };

            cmbCoverDimensions.ItemsSource = new Dictionary<string, string>
            {
                { "any", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericAny") },
                { "460x215", "460x215" },
                { "920x430",  "920x430" },
                { "460x215,920x430", "460x215 & 920x430" },
                { "600x900", "600x900" },
                { "342x482", "342x482" },
                { "660x930", "660x930" },
                { "512x512", "512x512" },
                { "1024x1024", "1024x1024" },
                { "512x512,1024x1024", "512x512 & 1024x1024" }
            };

            cmbCoverNsfw.ItemsSource = cmbGenericNsfw;
            cmbCoverHumor.ItemsSource = cmbGenericHumor;

            // Background Image items sources
            cmbBackgroundStyles.ItemsSource = new Dictionary<string, string>
            {
                { "any", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericAny") },
                { "alternate", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleAlternate") },
                { "blurred", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleBlurred") },
                { "material", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingStyleMaterial") },
            };

            cmbBackgroundDimensions.ItemsSource = new Dictionary<string, string>
            {
                { "any", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingGenericAny") },
                { "1920x620", "1920x620" },
                { "3840x1240",  "3840x1240" },
                { "1600x650", "1600x650" }
            };

            cmbBackgroundNsfw.ItemsSource = cmbGenericNsfw;
            cmbBackgroundHumor.ItemsSource = cmbGenericHumor;

            // Icon items sources
            cmbIconAssetSelection.ItemsSource = new Dictionary<string, string>
            {
                { "icons", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingIconAssetIcon") },
                { "logos", ResourceProvider.GetString("LOCSteamGridDBMetadata_SettingIconAssetLogo") },
            };

            cmbIconNsfw.ItemsSource = cmbGenericNsfw;
            cmbIconHumor.ItemsSource = cmbGenericHumor;
        }
    }
}
