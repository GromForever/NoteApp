using Xunit;

namespace NoteApp.Tests;

public class NoteTests
{
    [Fact]
    public void Note_TitleMaxLength_ShouldThrowException()
    {
        // Arrange
        var longTitle = new string('x', 51);

        // Act and assert
        Assert.Throws<ArgumentException>(() => new Note(longTitle));
    }

    [Fact]
    public void Note_Clone_ShouldCreateIdenticalCopy()
    {
        // Arrange
        var note = new Note("Test Note", NoteCategory.Work, "This is a test note.");

        // Act
        var clonedNote = (Note)note.Clone();

        // Assert
        Assert.Equal(note.Title, clonedNote.Title);
        Assert.Equal(note.Category, clonedNote.Category);
        Assert.Equal(note.Text, clonedNote.Text);
        Assert.Equal(note.CreationTime, clonedNote.CreationTime);
        Assert.Equal(note.LastModifiedTime, clonedNote.LastModifiedTime);
    }
}