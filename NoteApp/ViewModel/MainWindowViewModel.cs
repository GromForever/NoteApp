using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoteApp.Services;

namespace NoteApp.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IProjectService _projectService;

    [ObservableProperty] private ObservableCollection<Note> _notes;

    [ObservableProperty] private Note _selectedNote;
    
    [ObservableProperty]
    private string _selectedCategory = "All";

    private ObservableCollection<Note> _filteredNotes;
    public ObservableCollection<Note> FilteredNotes
    {
        get => _filteredNotes;
        private set
        {
            SetProperty(ref _filteredNotes, value);
            OnPropertyChanged(nameof(FilteredNotes));
        }
    }

    public IEnumerable<string> Categories => 
        new[] { "All" }.Concat(Enum.GetNames(typeof(NoteCategory)));

    public MainWindowViewModel(IProjectService projectService = null)
    {
        _projectService = projectService ?? new JsonProjectService();
        Notes = new ObservableCollection<Note>(_projectService.LoadProject());
        FilteredNotes = new ObservableCollection<Note>(Notes);
    }
    
    partial void OnSelectedCategoryChanged(string value)
    {
        UpdateFilteredNotes();
    }

    private void UpdateFilteredNotes()
    {
        if (SelectedCategory == "All")
        {
            FilteredNotes = new ObservableCollection<Note>(Notes);
        }
        else
        {
            FilteredNotes = new ObservableCollection<Note>(Notes.Where(n => n.Category.ToString() == SelectedCategory));
        }
    }

        [RelayCommand]
        private void AddNote()
        {
            var editViewModel = new NoteEditViewModel();
            var editWindow = new NoteEditView { DataContext = editViewModel };

            editViewModel.OnSuccessfulSave += (newNote) =>
            {
                Notes.Add(newNote);
                SelectedNote = newNote;
                _projectService.SaveProject(Notes);
                editWindow.DialogResult = true;
                editWindow.Close();
            };

            editViewModel.OnCancelled += () =>
            {
                editWindow.DialogResult = false;
                editWindow.Close();
            };

            editWindow.ShowDialog();
        }

        [RelayCommand]
        private void EditNote()
        {
            if (SelectedNote == null) return;

            var editViewModel = new NoteEditViewModel((Note)SelectedNote.Clone());
            var editWindow = new NoteEditView { DataContext = editViewModel };

            editViewModel.OnSuccessfulSave += (updatedNote) =>
            {
                int index = Notes.IndexOf(SelectedNote);
                Notes[index] = updatedNote;
                SelectedNote = updatedNote;
                _projectService.SaveProject(Notes);
                editWindow.DialogResult = true;
                editWindow.Close();
            };

            editViewModel.OnCancelled += () =>
            {
                editWindow.DialogResult = false;
                editWindow.Close();
            };

            editWindow.ShowDialog();
        }

        [RelayCommand]
        private void RemoveNote()
        {
            if (SelectedNote == null) return;

            var result = MessageBox.Show(
                $"Вы действительно хотите удалить заметку: {SelectedNote.Title}?", 
                "Подтверждение удаления", 
                MessageBoxButton.OKCancel, 
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.OK)
            {
                Notes.Remove(SelectedNote);
                FilteredNotes.Remove(SelectedNote);
                _projectService.SaveProject(Notes);
                SelectedNote = null;
            }
        }
        
        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void ShowAbout()
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }