using DBExplorerBlazor.Pages;

namespace MenuItemPages
{
    public class DuplicateKeyComparerTests
    {
        [Fact]
        public void Compare_SameKeys_ReturnsOne()
        {
            // Arrange
            var comparer = new DuplicateKeyComparer<int>();
            int key1 = 5;
            int key2 = 5;

            // Act
            int result = comparer.Compare(key1, key2);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_DifferentKeys_ReturnsComparisonResult()
        {
            // Arrange
            var comparer = new DuplicateKeyComparer<int>();
            int smallerKey = 3;
            int largerKey = 7;

            // Act
            int result1 = comparer.Compare(smallerKey, largerKey);
            int result2 = comparer.Compare(largerKey, smallerKey);

            // Assert
            Assert.True(result1 < 0);
            Assert.True(result2 > 0);
        }

        //[Fact]
        //public void Compare_NullKeys_ThrowsArgumentNullException()
        //{
        //    // Arrange
        //    var comparer = new DuplicateKeyComparer<string>();

        //    // Act & Assert
        //    //Assert.Throws<ArgumentNullException>(() => comparer.Compare(null!, "test"));
        //    Assert.Throws<ArgumentNullException>(() => comparer.Compare("test", null!));
        //}
    }
}