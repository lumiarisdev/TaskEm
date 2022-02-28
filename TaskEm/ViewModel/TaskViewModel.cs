using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TaskEm.Model;
using TaskEm.Commands;
using System.Runtime.Serialization.Json;
using System.IO;

namespace TaskEm.ViewModel
{
    public class TaskViewModel
    {

        public TaskCommand DeleteCommand { get; set; }
        public TaskCommand AddCommand { get; set; }

        public TaskCommand SaveCommand { get; set; }
        public TaskCommand LoadCommand { get; set; }

        private Task selectedTask;
        public Task SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                selectedTask = value;
                DeleteCommand.OnCanExecuteChanged();
            }
        }

        private string taskDescInput;
        public string TaskDescInput
        {
            get
            {
                return taskDescInput;
            }
            set
            {
                taskDescInput = value;
                /*if(taskDescInput != null && !taskDescInput.Equals("New Task"))
                {
                    AddCommand.OnCanExecuteChanged();
                }*/
            }
        }

        private string taskTitleInput;
        public string TaskTitleInput
        {
            get
            {
                return taskTitleInput;
            }
            set
            {
                taskTitleInput = value;
                if(taskTitleInput != null && !taskTitleInput.Equals("New Task"))
                {
                    AddCommand.OnCanExecuteChanged();
                }
            }
        }

        public TaskViewModel()
        {
            LoadTasks();
            DeleteCommand = new TaskCommand(OnDelete, CanDelete);
            AddCommand = new TaskCommand(OnAdd, CanAdd);
            SaveCommand = new TaskCommand(OnSave, CanSave);
            LoadCommand = new TaskCommand(OnLoad, CanLoad);
        }

        public ObservableCollection<Task> Tasks
        {
            get; set;
        }

        public void LoadTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>
            {
                new Task { Title = "New Task", Description = "This is an example task. Click on the title and select delete to remove it.", Complete = true},
            };

            LoadTasks(tasks);
        }

        public void LoadTasks(ObservableCollection<Task> tasks)
        {
            Tasks = tasks;
        }

        private void OnDelete()
        {
            Tasks.Remove(SelectedTask);
            if(Tasks.Count == 0)
            {
                LoadCommand.OnCanExecuteChanged();
            }
        }

        private bool CanDelete()
        {
            return SelectedTask != null;
        }

        private void OnAdd()
        {
            Tasks.Add(new Task { Title = taskTitleInput, Description = taskDescInput, Complete = false });
        }

        private bool CanAdd()
        {
            return taskTitleInput != null && !taskTitleInput.Equals("New Task");
        }

        private void OnSave()
        {

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".json";
            dialog.Filter = "(.json)|*.json";

            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                string fileName = dialog.FileName;
                var tasks = Tasks.ToArray<Task>();

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Task[]));
                MemoryStream msObj = new MemoryStream();
                js.WriteObject(msObj, tasks);
                msObj.Position = 0;
                StreamReader sr = new StreamReader(msObj);


                string json = sr.ReadToEnd();
                File.WriteAllText(fileName, json);

                sr.Close();
                msObj.Close();
            }

        }

        private bool CanSave()
        {
            return Tasks.Count > 0;
        }

        private void OnLoad()
        {

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".json";
            dialog.Filter = "(.json)|*.json";

            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                string fileName = dialog.FileName;

                string json = File.ReadAllText(fileName);

                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Task[]));
                    Task[] tasks = (Task[])deserializer.ReadObject(ms);
                    foreach (Task task in tasks)
                    {
                        Tasks.Add(task);
                    }
                }

            }
        }

        private bool CanLoad()
        {
            if(Tasks.Count > 0)
            {
                return false;
            }
            return true;
        }

    }
}
