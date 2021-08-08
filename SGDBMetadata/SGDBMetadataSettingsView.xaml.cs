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
                { "any", "Any" },
                { "false", "Only get assets not tagged as adult content" },
                { "true", "Only get assets tagged as adult content" },
            };

            var cmbGenericHumor = new Dictionary<string, string>
            {
                { "any", "Any" },
                { "false", "Only get assets not tagged as humor" },
                { "true", "Only get assets tagged as humor" },
            };


            // Cover items sources
            cmbCoverStyles.ItemsSource = new Dictionary<string, string>
            {
                { "any", "Any" },
                { "alternate", "Alternate" },
                { "blurred", "Blurred" },
                { "white_logo", "White Logo" },
                { "material", "Material" },
                { "no_logo", "No Logo" }
            };

            cmbCoverDimensions.ItemsSource = new Dictionary<string, string>
            {
                { "any", "Any" },
                { "460x215", "460x215" },
                { "920x430",  "920x430" },
                { "600x900", "600x900" },
                { "342x482", "342x482" },
                { "660x930", "660x930" },
                { "512x512", "512x512" },
                { "1024x1024", "1024x1024" }
            };

            cmbCoverNsfw.ItemsSource = cmbGenericNsfw;
            cmbCoverHumor.ItemsSource = cmbGenericHumor;

            // Background Image items sources
            cmbBackgroundStyles.ItemsSource = new Dictionary<string, string>
            {
                { "any", "Any" },
                { "alternate", "Alternate" },
                { "blurred", "Blurred" },
                { "white_logo", "White Logo" },
                { "material", "Material" },
                { "no_logo", "No Logo" }
            };
            cmbBackgroundDimensions.ItemsSource = new Dictionary<string, string>
            {
                { "any", "Any" },
                { "1920x620", "1920x620" },
                { "3840x1240",  "3840x1240" },
                { "1600x650", "1600x650" }
            };

            cmbBackgroundNsfw.ItemsSource = cmbGenericNsfw;
            cmbBackgroundHumor.ItemsSource = cmbGenericHumor;

            // Icon items sources
            cmbIconAssetSelection.ItemsSource = new Dictionary<string, string>
            {
                { "icons", "Icons" },
                { "logos", "Logos" },
            };

            cmbIconNsfw.ItemsSource = cmbGenericNsfw;
            cmbIconHumor.ItemsSource = cmbGenericHumor;
        }
    }
}
