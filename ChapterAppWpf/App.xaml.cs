using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Xaml;
using ChapterAppWpf.Managers;
using ChapterAppWpf.ViewModels;
using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace ChapterAppWpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly NavigationManager _navigationManager;
		private readonly ApplicationDbContext _context;
		private readonly ChapterTracker _chapterTracker;

		public App()
		{
			//Services = ConfigureServices();
			_navigationManager = new NavigationManager();
			_context = new ApplicationDbContext();
			_chapterTracker = new ChapterTracker(_context);
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			_navigationManager.CurrentViewModel = new StartViewModel(_chapterTracker, _navigationManager);
			
			var mainWindow = new MainWindow(){DataContext = new MainViewModel(_navigationManager, _chapterTracker)};

			mainWindow.Show();
			base.OnStartup(e);
		}

		//public new static App Current => (App)Application.Current;

		//IServiceProvider Services { get; }

		//private IServiceProvider ConfigureServices()
		//{
		//	//var services = new ServiceCollection();

		//	//services.AddScoped();
		//}


	}

}
