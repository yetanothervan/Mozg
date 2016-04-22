using System.Windows.Controls;

namespace OutputScreen.DiagnosticsView
{
    /// <summary>
    /// Interaction logic for DiagnosticsView.xaml
    /// </summary>
    public partial class DiagnosticsView : UserControl
    {
        public DiagnosticsView(DiagnosticsViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
