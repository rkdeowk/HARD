using SuperChat.Core;
using SuperChat.MVVM.Model;
using System;
using System.Collections.ObjectModel;

namespace SuperChat.MVVM.ViewModel
{
    class MainViewModel : ObsavableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }

        public RelayCommand SendCommand { get; set; }

        private ContactModel _selectedContact;
        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    FirstMessage = false
                });

                Message = "";
            });

            Messages.Add(new MessageModel
            {
                Username = "Allison 0",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/yMWvlXd.png",
                Message = "Test",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true
            });

            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Allison 0",
                    UsernameColor = "#409aff",
                    ImageSource = "https://i.imgur.com/yMWvlXd.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = false
                });
            }

            for (int i = 0; i < 4; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Allison 4",
                    UsernameColor = "#409aff",
                    ImageSource = "https://i.imgur.com/yMWvlXd.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = true,
                    FirstMessage = true,
                });
            }

            Messages.Add(new MessageModel
            {
                Username = "Allison 4",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/yMWvlXd.png",
                Message = "Last",
                Time = DateTime.Now,
                IsNativeOrigin = true,
            });

            for (int i = 0; i < 5; i++)
            {
                Contacts.Add(new ContactModel
                {
                    Username = $"Allison {i}",
                    ImageSource = "https://i.imgur.com/i2szTsp.png",
                    Messages = Messages
                });
            }
        }
    }
}
