using csharp_final.Models;
using System.Windows;

namespace csharp_final.Views
{
    public partial class AddNewPersonView : Window
    {
        public Person? NewPerson { get; private set; }

        public AddNewPersonView()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = PersonGenerator.GenerateFirstName();
            txtLastName.Text = PersonGenerator.GenerateLastName();
            txtEmail.Text = PersonGenerator.GenerateEmail(txtFirstName.Text, txtLastName.Text);
            dpBirthDate.SelectedDate = PersonGenerator.GenerateBirthDate();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewPerson = new
                (
                    txtFirstName.Text,
                    txtLastName.Text,
                    txtEmail.Text,
                    dpBirthDate.SelectedDate.GetValueOrDefault(DateTime.Today)
                );
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
