using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class NotesCenterServiceTests
{
    [Fact]
    public void FireOnNotesCenterChange_InvokesEventWithCorrectListOfNotes()
    {
        // Arrange
        var service = new NotesCenterService();
        List<MemberEntity>? receivedNotes = null;
        service.OnNotesCenterChange += (notes) => receivedNotes = notes;
        var expectedNotes = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };

        // Act
        service.FireOnNotesCenterChange(expectedNotes);

        // Assert
        Assert.NotNull(receivedNotes);
        Assert.Equal(expectedNotes, receivedNotes);
    }
}