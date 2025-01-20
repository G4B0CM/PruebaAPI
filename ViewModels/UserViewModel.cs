using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GamingApp.Repositories;
using GamingApp.Models;
using GamingApp.Services;

namespace GamingApp.ViewModels
{
    public class UserViewModel : ObservableObject, IQueryAttributable
    {
        private readonly UserService _userService;
        private string _statusMessage;
        public ObservableCollection<User> Users { get; set; }
        
        private Models.User _user;
        private readonly UserRepository _userRepository;

        public ICommand SaveCommand { get; }
        public ICommand GetAllPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public Models.User User
        {
            get => _user;
            set
            {
                if (SetProperty(ref _user, value)) //Profe estoy usando este método porque me instale el CommunityToolkit.MVVM
                {
                    OnPropertyChanged(nameof(_user.Name));
                    OnPropertyChanged(nameof(_user.Username));
                    OnPropertyChanged(nameof(_user.Email));
                    OnPropertyChanged(nameof(_user.Id));
                }
            }
        }
        public string Name
        {
            get => _user.Name;
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => _user.Username;
            set
            {
                if (_user.Username != value)
                {
                    _user.Username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _user.Email;
            set
            {
                if (_user.Email != value)
                {
                    _user.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id => _user.Id;
        public UserViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3");
            _userRepository = new UserRepository(dbPath);

            _user = new Models.User();
            _userService = new UserService();
            Users = new ObservableCollection<Models.User>();
            SaveCommand = new AsyncRelayCommand(Save);
            GetAllPeopleCommand = new AsyncRelayCommand(LoadPeople);
            DeletePersonCommand = new AsyncRelayCommand<Models.User>((person) => Eliminar(person));
        }
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadUsersAsync()
        {
            
            var users = await _userService.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
                Console.WriteLine(user.ToString);
            }
        }
        private async Task Save()
        {
            try
            {
                if (string.IsNullOrEmpty(_user.Name))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(_user.Username))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(_user.Email))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                _userRepository.agregarUsuario(_user.Name, _user.Username,_user.Email);

                StatusMessage = $"Persona {_user.Name} guardada exitosamente.";
                await Shell.Current.GoToAsync($"..?saved={_user.Name}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar la persona: {ex.Message}";
            }
        }
        private async Task Eliminar(Models.User personaAEliminar)
        {
            try
            {
                if (personaAEliminar == null)
                {
                    throw new Exception("Persona no válida.");
                }

                _userRepository.EliminarPersona(personaAEliminar.Name);
                Users.Remove(personaAEliminar);
                StatusMessage = $"Se eliminó a {personaAEliminar.Name}.";

                await Shell.Current.DisplayAlert("Aviso!", $"Gabriel Calderón acaba de eliminar a {personaAEliminar.Name}", "Aceptar");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar a la persona: {ex.Message}";
            }
        }
        private async Task LoadPeople()
        {
            try
            {
                var people = _userRepository.GetAllPeople();
                Users.Clear();
                foreach (var person in people)
                {
                    Users.Add(person);
                }

                StatusMessage = $"Se cargaron {Users.Count} personas.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener personas: {ex.Message}";
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("person") && query["person"] is Models.User person)
            {
                User = person;
            }
            else if (query.ContainsKey("deleted"))
            {
                string nombre = query["deleted"].ToString();
                Models.User matchedPerson = Users.FirstOrDefault(p => p.Name == nombre);

                if (matchedPerson != null)
                    Users.Remove(matchedPerson);
            }
        }
    }
}
