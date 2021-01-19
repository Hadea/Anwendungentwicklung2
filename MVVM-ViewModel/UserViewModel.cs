using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVM_ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        private string name;
        public double Salary
        {
            get => salary;
            set
            {
                if (salary != value)
                {
                    salary = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Salary)));
                }
            }
        }
        private double salary;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
