using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using csharp_final.Models;
using csharp_final.Utilities;
using csharp_final.Views;

namespace csharp_final.ViewModels
{
    internal class PersonListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }

        private string _filterText = string.Empty;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                Task.Run(Filter);
            }
        }

        private string _filterInfo = string.Empty;
        public string FilterInfo
        {
            get { return _filterInfo; }
            set
            {
                _filterInfo = value;
                OnPropertyChanged(nameof(FilterInfo));
            }
        }

        private ObservableCollection<Person> _people = [];

        public ObservableCollection<Person> People
        {
            get { return _people; }
            set
            {
                _people = value;
                OnPropertyChanged(nameof(People));
                Task.Run(Filter);
            }
        }

        private ObservableCollection<Person> _filteredPeople = [];

        public ObservableCollection<Person> FilteredPeople
        {
            get { return _filteredPeople; }
            set
            {
                _filteredPeople = value;
                OnPropertyChanged(nameof(FilteredPeople));
            }
        }

        private Person? _selectedPerson;
        public Person? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        private enum SortOrder
        {
            Ascending,
            Descending,
            Default
        }

        private SortOrder _sortOrder = SortOrder.Default;
        private string? _currentColumn;

        private readonly Task _initLoadData;

        public PersonListViewModel()
        {
            _initLoadData = Task.Run(async () =>
            {
                People = await PersonSerializer.LoadPersonListAsync();
            });

            AddCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), (object parameter) => Add());
            EditCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), (object parameter) => Edit());
            DeleteCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), (object parameter) => Delete());
            ClearCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), (object parameter) => Clear());
            SaveCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), async (object parameter) => await Save());
            LoadCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), async (object parameter) => await Load());
            SortCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), async (object parameter) => await Sort(parameter));
            ResetCommand = new RelayCommand((object parameter) => IsInitTaskCompleted(), async (object parameter) => await Reset());
        }

        private bool IsInitTaskCompleted()
        {
            return _initLoadData.Status == TaskStatus.RanToCompletion;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Add()
        {
            var window = new AddNewPersonView();
            if (window.ShowDialog() == true)
            {
                People.Add(window.NewPerson!);
                Task.Run(Filter);
            }
        }

        private void Edit()
        {
            if (!CheckIfSelected())
            {
                return;
            }

            var window = new EditPersonView(SelectedPerson!);
            if (window.ShowDialog() == true)
            {
                OnPropertyChanged(nameof(People));
                Task.Run(Filter);
            }
        }

        private void Delete()
        {
            if (!CheckIfSelected())
            {
                return;
            }

            string title = "Delete person";
            string text = "Are you sure you want to delete this person?";
            if (SpawnMessageBox(title, text))
            {
                People.Remove(SelectedPerson!);
                Task.Run(Filter);
            }
        }

        private void Clear()
        {
            string title = "Clear list";
            string text = "Are you sure you want to clear the current list?";
            if (SpawnMessageBox(title, text))
            {
                People = [];
                Task.Run(Reset);
            }
        }

        private async Task Save()
        {
            string title = "Save to file";
            string text = "Are you sure you want to overwrite the file with current list?";
            if (SpawnMessageBox(title, text))
            {
                await PersonSerializer.SavePersonListAsync(People);
            }
        }

        private async Task Load()
        {
            string title = "Load from file";
            string text = "Are you sure you want to overwrite the current list?";
            if (SpawnMessageBox(title, text))
            {
                People = await PersonSerializer.LoadPersonListAsync();
            }
        }

        private async Task Filter()
        {
            await Task.Run(() =>
            {
                var filtered = People.Where(p => p.ToString().Contains(FilterText, StringComparison.CurrentCultureIgnoreCase));

                Func<Person, object>? sortingCallback = _currentColumn switch
                {
                    "FirstName" => p => p.FirstName,
                    "LastName" => p => p.LastName,
                    "Email" => p => p.Email,
                    "BirthDate" => p => p.BirthDate,
                    "Age" => p => p.Age,
                    "WesternZodiac" => p => p.WesternZodiac,
                    "ChineseZodiac" => p => p.ChineseZodiac,
                    "IsAdult" => p => p.IsAdult,
                    "IsBirthday" => p => p.IsBirthday,
                    _ => null
                };

                if (_currentColumn != null)
                {
                    if (_sortOrder == SortOrder.Ascending)
                    {
                        filtered = filtered.OrderBy(sortingCallback!);
                    }
                    else if (_sortOrder == SortOrder.Descending)
                    {
                        filtered = filtered.OrderByDescending(sortingCallback!);
                    }
                }
                FilteredPeople = new ObservableCollection<Person>(filtered);
            });
        }

        private async Task Sort(object parameter)
        {
            if (_currentColumn == (string)parameter)
            {
                _sortOrder = _sortOrder switch
                {
                    SortOrder.Ascending => _sortOrder = SortOrder.Descending,
                    SortOrder.Descending => _sortOrder = SortOrder.Default,
                    SortOrder.Default => _sortOrder = SortOrder.Ascending,
                    _ => _sortOrder
                };
            }
            else
            {
                _currentColumn = (string)parameter;
                _sortOrder = SortOrder.Ascending;
            }
            FilterInfo = _sortOrder != SortOrder.Default ? $"Sorted by {_currentColumn}, order: {_sortOrder}" : string.Empty;
            await Filter();
        }

        private async Task Reset()
        {
            _currentColumn = null;
            _sortOrder = SortOrder.Default;
            FilterText = string.Empty;
            FilterInfo = string.Empty;

            await Filter();
        }

        private bool CheckIfSelected()
        {
            if (SelectedPerson == null)
            {
                MessageBox.Show("Select a person first!", "Person not selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private static bool SpawnMessageBox(string title, string text)
        {
            return MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
