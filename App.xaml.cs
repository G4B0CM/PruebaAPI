using GamingApp.Repositories;
namespace GamingApp
{
    public partial class App : Application
    {
        public static UserRepository PersonRepo { get; private set; }
        public App(UserRepository repo)
        {
            InitializeComponent();

            MainPage = new AppShell();

            PersonRepo = repo;
        }
    }
}
