using CommunityToolkit.Mvvm.ComponentModel;

namespace NoteApp;

public partial class Note : ObservableObject, ICloneable
{
    [ObservableProperty]
    private string _title = "Без названия";

    [ObservableProperty]
    private string _text = string.Empty;

    [ObservableProperty]
    private NoteCategory _category = NoteCategory.Miscellaneous;

    public DateTime CreationTime { get; set; }
    public DateTime LastModifiedTime { get; set; }

    public Note()
    {
        CreationTime = DateTime.Now;
        LastModifiedTime = CreationTime;
    }

    partial void OnTitleChanged(string value)
    {
        if (value.Length > 50)
            throw new ArgumentException("Название не может превышать 50 символов");

        LastModifiedTime = DateTime.Now;
    }

    partial void OnTextChanged(string value)
    {
        LastModifiedTime = DateTime.Now;
    }

    partial void OnCategoryChanged(NoteCategory value)
    {
        LastModifiedTime = DateTime.Now;
    }

    public object Clone() => MemberwiseClone();
}