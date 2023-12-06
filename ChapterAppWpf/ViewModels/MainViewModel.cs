using ChapterAppWpf.Managers;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace ChapterAppWpf.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		private readonly NavigationManager _navigationManager;
		private readonly ChapterTracker _chapterTracker;

		public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;

		public MainViewModel(NavigationManager navigationManager, ChapterTracker chapterTracker)
		{
			_navigationManager = navigationManager;
			_chapterTracker = chapterTracker;

			_navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
		}

		private void CurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}
	}
}
