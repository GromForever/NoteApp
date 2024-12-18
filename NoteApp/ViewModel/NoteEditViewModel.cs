using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NoteApp.ViewModel;

public partial class NoteEditViewModel : ObservableObject
{
    [ObservableProperty]
    private Note _note;

    public ObservableCollection<NoteCategory> Categories { get; }

    private bool _isModified;

    public NoteEditViewModel(Note existingNote = null)
    {
        Note = existingNote ?? new Note();
        Categories = new ObservableCollection<NoteCategory>(
            Enum.GetValues(typeof(NoteCategory)).Cast<NoteCategory>()
        );
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Note.Title))
        {
            MessageBox.Show("Название заметки не может быть пустым.", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (Note.Title.Length > 50)
        {
            MessageBox.Show("Название заметки не может превышать 50 символов.", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _isModified = true;
        OnSuccessfulSave?.Invoke(Note);
    }

    [RelayCommand]
    private void Cancel()
    {
        OnCancelled?.Invoke();
    }

    public event Action<Note> OnSuccessfulSave;
    public event Action OnCancelled;
}