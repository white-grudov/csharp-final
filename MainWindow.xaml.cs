using csharp_final.Models;
using System.Windows;

namespace csharp_final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Do you want to save current list?", "Save Data", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var viewModel = DataContext as ViewModels.PersonListViewModel;
                await PersonSerializer.SavePersonListAsync(viewModel!.People);
            }
        }
    }
}