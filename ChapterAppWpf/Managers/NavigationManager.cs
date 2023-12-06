using CommunityToolkit.Mvvm.ComponentModel;

namespace ChapterAppWpf.Managers;

public class NavigationManager
{
	private ObservableObject _currentViewModel;

	public ObservableObject CurrentViewModel
	{
		get => _currentViewModel;
		set
		{
			_currentViewModel = value;
			OnCurrentViewModelChange();
		}
	}


	private void OnCurrentViewModelChange()
	{
		CurrentViewModelChanged?.Invoke();
	}

	public event Action CurrentViewModelChanged;
}
