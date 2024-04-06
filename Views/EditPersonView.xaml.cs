using System.Windows;
using csharp_final.Models;

namespace csharp_final.Views
{
    public partial class EditPersonView : Window
    {
        public EditPersonView(Person person)
        {
            InitializeComponent();
            DataContext = person;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedPerson = DataContext as Person;
            try
            {
                editedPerson!.FirstName = txtFirstName.Text;
                editedPerson!.LastName = txtLastName.Text;
                editedPerson!.Email = txtEmail.Text;
                editedPerson!.BirthDate = dpBirthDate.SelectedDate.GetValueOrDefault(editedPerson.BirthDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
