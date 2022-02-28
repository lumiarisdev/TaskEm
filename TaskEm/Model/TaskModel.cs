using System.ComponentModel;
using System.Runtime.Serialization;

namespace TaskEm.Model
{
    public class TaskModel
    {
    }

    [DataContract]
    public class Task : INotifyPropertyChanged
    {

        private string title;
        private string description;
        private bool complete;

        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                if(title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        [DataMember]
        public bool Complete
        {
            get { return complete; }
            set
            {
                if(complete != value)
                {
                    complete = value;
                    OnPropertyChanged("Complete");
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }

}
