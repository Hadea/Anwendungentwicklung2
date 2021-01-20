using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public string Filter
        {
            get { return filter; }
            set
            {
                if (filter != value)
                {
                    filter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }
        private string filter;

        public event PropertyChangedEventHandler PropertyChanged;


        public ObservableCollection<UserViewModel> EntryList
        {
            get => entries;
            set
            {
                if (entries != value)
                {
                    entries = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryList)));
                }
            }
        }
        private ObservableCollection<UserViewModel> entries;

        public MainViewModel()
        {
            // verbinden der Commands mit den Properties
            AddUser = new AddUserCommand() {Parent = this };

            EntryList = new ObservableCollection<UserViewModel>();

            // beispieldaten füllen
            EntryList.Add(new UserViewModel { Name = "Hans", Salary = 55000.0 });
            EntryList.Add(new UserViewModel { Name = "Peter", Salary = 58000.0 });
            EntryList.Add(new UserViewModel { Name = "Hildegard", Salary = 62000.0 });

            Filter = "A";
        }

        public ICommand AddUser { get; set; }

    }
}
