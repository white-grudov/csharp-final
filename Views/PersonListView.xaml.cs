using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using csharp_final.ViewModels;

namespace csharp_final.Views
{
    public partial class PersonListView : UserControl
    {
        public PersonListView()
        {
            InitializeComponent();
        }

        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as GridViewColumnHeader;
            string? columnName = columnHeader?.Tag.ToString();
            var viewModel = DataContext as PersonListViewModel;
            viewModel?.SortCommand.Execute(columnName);
        }
    }
}
