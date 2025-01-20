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

namespace GamingApp.ViewModels
{
    public class GamerViewModel : ObservableObject, IQueryAttributable
    {
        private ObservableCollection<Models.Gamer> _peopleList;
        private string _statusMessage;
        private readonly GamerRepository _gamerRepository;
        public ICommand SaveCommand { get; }
        public ICommand GetAllPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }


        private Models.Gamer _gamer;

        public Models.Gamer Gamer
        {
            get => _gamer;
            set
            {
                if (SetProperty(ref _gamer, value)) //Profe estoy usando este método porque me instale el CommunityToolkit.MVVM
                {
                    OnPropertyChanged(nameof(name));
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public ObservableCollection<Models.Gamer> PeopleList
        {
            get => _peopleList;
            set => SetProperty(ref _peopleList, value);
        }

        public string name
        {
            get => _gamer.name;
            set
            {
                if (_gamer.name != value)
                {
                    _gamer.name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string description
        {
            get => _gamer.description;
            set
            {
                if (_gamer.description != value)
                {
                    _gamer.description = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id => _gamer.Id;


        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }



        public GamerViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3");
            _gamerRepository = new GamerRepository(dbPath);

            _gamer = new Models.Gamer();
            PeopleList = new ObservableCollection<Models.Gamer>();
            SaveCommand = new AsyncRelayCommand(Save);
            GetAllPeopleCommand = new AsyncRelayCommand(LoadPeople);
            DeletePersonCommand = new AsyncRelayCommand<Models.Gamer>((person) => Eliminar(person));
        }

        private async Task Save()
        {
            try
            {
                if (string.IsNullOrEmpty(_gamer.name))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(_gamer.description))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                _gamerRepository.agregarGamer(_gamer.name,_gamer.description);

                StatusMessage = $"Persona {_gamer.name} guardada exitosamente.";
                await Shell.Current.GoToAsync($"..?saved={_gamer.name}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar la persona: {ex.Message}";
            }
        }

        private async Task Eliminar(Models.Gamer personaAEliminar)
        {
            try
            {
                if (personaAEliminar == null)
                {
                    throw new Exception("Persona no válida.");
                }

                _gamerRepository.EliminarPersona(personaAEliminar.name);
                PeopleList.Remove(personaAEliminar);
                StatusMessage = $"Se eliminó a {personaAEliminar.name}.";

                await Shell.Current.DisplayAlert("Aviso!", $"Gabriel Calderón acaba de eliminar a {personaAEliminar.name}", "Aceptar");
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
                var people = _gamerRepository.GetAllPeople();
                PeopleList.Clear();
                foreach (var person in people)
                {
                    PeopleList.Add(person);
                }

                StatusMessage = $"Se cargaron {PeopleList.Count} personas.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener personas: {ex.Message}";
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("person") && query["person"] is Models.Gamer person)
            {
                Gamer = person;
            }
            else if (query.ContainsKey("deleted"))
            {
                string nombre = query["deleted"].ToString();
                Models.Gamer matchedPerson = PeopleList.FirstOrDefault(p => p.name == nombre);

                if (matchedPerson != null)
                    PeopleList.Remove(matchedPerson);
            }
        }
    }
}
