using System.Windows.Controls;

namespace OutputScreen.OutputView
{
    /// <summary>
    /// Логика взаимодействия для RegionView.xaml
    /// </summary>
    public partial class OutputView : UserControl
    {
        public OutputView(OutputViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
