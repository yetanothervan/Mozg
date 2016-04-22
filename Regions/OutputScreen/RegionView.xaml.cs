using System.Windows.Controls;

namespace OutputScreen
{
    /// <summary>
    /// Логика взаимодействия для RegionView.xaml
    /// </summary>
    public partial class RegionView : UserControl
    {
        public RegionView(RegionViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
