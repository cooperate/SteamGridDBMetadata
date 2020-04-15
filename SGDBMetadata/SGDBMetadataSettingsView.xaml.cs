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
            cmbDimensions.ItemsSource = new List<string>
            {
                "any",
                "460x215",
                "920x430",
                "600x900",
                "342x482",
                "legacy"
            };

            cmbStyles.ItemsSource = new List<string>
            {
                "any",
                "alternate",
                "blurred",
                "white_logo",
                "material",
                "no_logo"
            };
        }
    }
}