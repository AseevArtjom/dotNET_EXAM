using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNET_EXAM.Models
{
    public class Test : INotifyPropertyChanged
    {
        public int _id;
        public string _name;
        public string _description;
        public int _score;
        public ICollection<Question> _questions;

        public int Id { get { return _id; } set { _id = value;OnPropertyChanged("Id"); } }
        public string Name { get { return _name; } set { _name = value;OnPropertyChanged("Name"); } }
        public string Description { get { return _description; } set { _description = value;OnPropertyChanged("Description"); } }
        public int Score { get { return _score; } set { _score = value;OnPropertyChanged("Score"); } }
        public ICollection<Question> Questions { get { return _questions; } set { _questions = value;OnPropertyChanged("Questions"); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propetyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propetyName));
        }
    }
}
