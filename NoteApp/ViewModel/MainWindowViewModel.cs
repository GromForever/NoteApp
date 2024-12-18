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
    
    [ObservableProperty] private ObservableCollection<Note> _filteredNotes;

    [ObservableProperty] private Note _selectedNote;

    [ObservableProperty] private NoteCategory _selectedCategory;

    [ObservableProperty] private ObservableCollection<NoteCategory> _categories;

    public MainWindowViewModel(IProjectService projectService = null)
    {
        _projectService = projectService ?? new JsonProjectService();
        _categories = new ObservableCollection<NoteCategory>(
            Enum.GetValues(typeof(NoteCategory)).Cast<NoteCategory>());
        Notes = new ObservableCollection<Note>(_projectService.LoadProject());
        _filteredNotes = new ObservableCollection<Note>(_notes);
    }

    partial void OnSelectedCategoryChanged(NoteCategory value)
    {
        if (value != NoteCategory.All)
        {
            _filteredNotes.Clear();
            foreach (var category in _notes.Where(n => n.Category == value))
            {
                _filteredNotes.Add(category);
            }
        }
        else
        {
            _filteredNotes.Clear();
            foreach (var category in _notes)
            {
                _filteredNotes.Add(category);
            }
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