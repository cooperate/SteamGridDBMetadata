using System.Collections.Generic;
using System.Windows.Controls;

namespace SGDBMetadata
{
    public partial class SGDBMetadataSettingsView : UserControl
    {
        public SGDBMetadataSettingsView()
        {
            InitializeComponent();
            
            cmbDimensions.ItemsSource = new List<string>
            {
                "Any",
                "460x215",
                "920x430",
                "600x900",
                "342x482",
                "Legacy"
            };
            cmbDimensions.SelectedIndex = 0;

            cmbStyles.ItemsSource = new List<string>
            {
                "Any",
                "Alternate",
                "Blurred",
                "White Logo",
                "Material",
                "No Logo"
            };
            cmbStyles.SelectedIndex = 0;
        }
    }
}