using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MVVM_ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string filter;

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
            EntryList = new ObservableCollection<UserViewModel>();
            EntryList.Add(new UserViewModel { Name = "Hans", Salary = 55000.0 });
            EntryList.Add(new UserViewModel { Name = "Peter", Salary = 58000.0 });
            EntryList.Add(new UserViewModel { Name = "Hildegard", Salary = 62000.0 });

            Filter = "A";
        }

    }
}
