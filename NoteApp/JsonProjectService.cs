// Services/JsonProjectService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace NoteApp.Services
{
    public class JsonProjectService : IProjectService
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
            "NoteApp.notes"
        );

        public void SaveProject(IEnumerable<Note> notes)
        {
            string jsonString = JsonSerializer.Serialize(notes, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            File.WriteAllText(FilePath, jsonString);
        }

        public IEnumerable<Note> LoadProject()
        {
            if (!File.Exists(FilePath))
                return new List<Note>();

            string jsonString = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Note>>(jsonString) ?? new List<Note>();
        }
    }
}