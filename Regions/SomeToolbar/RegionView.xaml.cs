using System.Windows.Controls;

namespace SomeToolbar
{
    /// <summary>
    /// Логика взаимодействия для RegionView.xaml
    /// </summary>
    public partial class RegionView : UserControl
    {
        public RegionView(RegionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
