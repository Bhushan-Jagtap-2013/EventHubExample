using CoffeeMachine.EventHub.Sender;
using CoffeeMachine.UI.ViewModel;
using System.Configuration;
using System.Windows;

namespace CoffeeMachine.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            DataContext = new MainViewModel(new MachineDataSender(connectionString));
        }
    }
}
