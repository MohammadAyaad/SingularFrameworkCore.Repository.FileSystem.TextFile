using System.IO;
using Xunit;

namespace SingularFrameworkCore.Repository.FileSystem.TextFile.Tests
{
    public class TextFileRepositoryTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly string _tempDirectory;

        public TextFileRepositoryTests()
        {
            // Create unique temp directory for each test
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
            _tempFilePath = Path.Combine(_tempDirectory, "test.txt");
        }

        public void Dispose()
        {
            // Clean up temp directory
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }

        // --- Constructor Tests ---
        [Fact]
        public void Constructor_SetsPathAndOverwrite()
        {
            var repository = new TextFileRepository("test.txt", overwrite: true);
            Assert.Equal("test.txt", repository.Path);
            Assert.True(repository.Overwrite);
        }

        // --- Create Tests ---
        [Fact]
        public void Create_WhenFileDoesNotExist_CreatesFileWithContent()
        {
            // Arrange
            var repository = new TextFileRepository(_tempFilePath);
            const string content = "Hello World";

            // Act
            repository.Create(content);

            // Assert
            Assert.True(File.Exists(_tempFilePath));
            Assert.Equal(content, File.ReadAllText(_tempFilePath));
        }

        [Fact]
        public void Create_WhenFileExistsAndOverwriteFalse_ThrowsException()
        {
            // Arrange
            const string originalContent = "Existing content";
            File.WriteAllText(_tempFilePath, originalContent);
            var repository = new TextFileRepository(_tempFilePath, overwrite: false);
            const string newContent = "New content";

            // Act & Assert
            var ex = Assert.Throws<TextFileRepositoryFileAlreadyExistsException>(
                () => repository.Create(newContent)
            );
            Assert.Equal("File already exists", ex.Message);
            Assert.Equal(originalContent, File.ReadAllText(_tempFilePath));
        }

        [Fact]
        public void Create_WhenFileExistsAndOverwriteTrue_OverwritesFile()
        {
            // Arrange
            File.WriteAllText(_tempFilePath, "Old content");
            var repository = new TextFileRepository(_tempFilePath, overwrite: true);
            const string newContent = "New content";

            // Act
            repository.Create(newContent);

            // Assert
            Assert.Equal(newContent, File.ReadAllText(_tempFilePath));
        }

        // --- Read Tests ---
        [Fact]
        public void Read_WhenFileExists_ReturnsContent()
        {
            // Arrange
            const string expected = "Test content";
            File.WriteAllText(_tempFilePath, expected);
            var repository = new TextFileRepository(_tempFilePath);

            // Act
            var actual = repository.Read();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_WhenFileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            var repository = new TextFileRepository(_tempFilePath);

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => repository.Read());
        }

        // --- Update Tests ---
        [Fact]
        public void Update_OverwritesFileContent()
        {
            // Arrange
            File.WriteAllText(_tempFilePath, "Initial content");
            var repository = new TextFileRepository(_tempFilePath);
            const string newContent = "Updated content";

            // Act
            repository.Update(newContent);

            // Assert
            Assert.Equal(newContent, File.ReadAllText(_tempFilePath));
        }

        // --- Delete Tests ---
        [Fact]
        public void Delete_WhenFileExists_RemovesFile()
        {
            // Arrange
            File.WriteAllText(_tempFilePath, "Content to delete");
            var repository = new TextFileRepository(_tempFilePath);

            // Act
            repository.Delete();

            // Assert
            Assert.False(File.Exists(_tempFilePath));
        }

        [Fact]
        public void Delete_WhenFileDoesNotExist_DoesNothing()
        {
            // Arrange
            var repository = new TextFileRepository(_tempFilePath);

            // Act
            repository.Delete();

            // Assert
            Assert.False(File.Exists(_tempFilePath));
        }

        // --- Edge Cases ---
        [Fact]
        public void HandlesEmptyStringContent()
        {
            var repository = new TextFileRepository(_tempFilePath);
            repository.Create("");
            Assert.Equal("", repository.Read());
        }

        [Fact]
        public void HandlesSpecialCharacters()
        {
            const string content = "Line1\nLine2\r\n\tTabbed\tContent";
            var repository = new TextFileRepository(_tempFilePath);
            repository.Create(content);
            Assert.Equal(content, repository.Read());
        }
    }
}
