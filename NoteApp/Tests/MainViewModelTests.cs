using Moq;
using NoteApp.Services;
using NoteApp.ViewModel;
using Xunit;

namespace NoteApp.Tests;

public class MainViewModelTests
    {
        [Fact]
        public void AddNote_ShouldAddNoteToProject()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            var viewModel = new MainWindowViewModel(mockProjectService.Object);

            // Act
            viewModel.AddNoteCommand.Execute(null);

            // Assert
            Assert.Single(viewModel.Notes);
            mockProjectService.Verify(s => s.SaveProject(It.IsAny<System.Collections.Generic.IEnumerable<Note>>()), Times.Once);
        }

        [Fact]
        public void RemoveNote_ShouldRemoveNoteFromProject()
        {
            // Arrange
            var note = new Note("Test Note", NoteCategory.Work, "This is a test note.");
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(s => s.LoadProject()).Returns(new[] { note });
            var viewModel = new MainWindowViewModel(mockProjectService.Object);
            viewModel.SelectedNote = note;

            // Act
            viewModel.RemoveNoteCommand.Execute(null);

            // Assert
            Assert.Empty(viewModel.Notes);
            mockProjectService.Verify(s => s.SaveProject(It.IsAny<System.Collections.Generic.IEnumerable<Note>>()), Times.Once);
        }

        [Fact]
        public void FilterNotes_ShouldFilterBySelectedCategory()
        {
            // Arrange
            var notes = new[]
            {
                new Note("Note 1", NoteCategory.Work, ""),
                new Note("Note 2", NoteCategory.Home, ""),
                new Note("Note 3", NoteCategory.Work, "")
            };
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(s => s.LoadProject()).Returns(notes);
            var viewModel = new MainWindowViewModel(mockProjectService.Object);
            var goodFilter = notes.Where(n => n.Category == NoteCategory.Work);

            // Act
            viewModel.SelectedCategory = NoteCategory.Work.ToString();

            // Assert
            Assert.Equal(2, viewModel.FilteredNotes.Count());
            Assert.All(viewModel.FilteredNotes, n => Assert.True(n.Category == NoteCategory.Work));
        }
    }