using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapterAppWpf.Managers;
using CommunityToolkit.Mvvm.ComponentModel;
using Services;

namespace ChapterAppWpf.ViewModels
{
	public class StartViewModel : ObservableObject
	{
		private readonly ChapterTracker _chapterTracker;
		private readonly NavigationManager _navigationManager;

		public StartViewModel(ChapterTracker chapterTracker, NavigationManager navigationManager)
		{
			_chapterTracker = chapterTracker;
			_navigationManager = navigationManager;

			//Här lägga till olika commands vad som ska kunna hända.
		}
	}
}
