using System.Collections.Generic;

namespace NoteApp.Services
{
    public interface IProjectService
    {
        void SaveProject(IEnumerable<Note> notes);
        IEnumerable<Note> LoadProject();
    }
}