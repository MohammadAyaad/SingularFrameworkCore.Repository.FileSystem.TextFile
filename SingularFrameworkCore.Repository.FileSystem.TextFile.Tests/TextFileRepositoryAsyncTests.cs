using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SingularFrameworkCore.Repository.FileSystem.TextFile.Tests
{
    public class TextFileRepositoryAsyncTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly string _tempDirectory;

        public TextFileRepositoryAsyncTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
            _tempFilePath = Path.Combine(_tempDirectory, "test.txt");
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }

        // --- Constructor Tests ---
        [Fact]
        public void Constructor_SetsPathAndOverwrite()
        {
            var repository = new TextFileRepositoryAsync("test.txt", overwrite: true);
            Assert.Equal("test.txt", repository.Path);
            Assert.True(repository.Overwrite);
        }

        // --- Create Tests ---
        [Fact]
        public async Task CreateAsync_WhenFileDoesNotExist_CreatesFileWithContent()
        {
            var repository = new TextFileRepositoryAsync(_tempFilePath);
            const string content = "Hello World";

            await repository.Create(content);

            Assert.True(File.Exists(_tempFilePath));
            Assert.Equal(content, await File.ReadAllTextAsync(_tempFilePath));
        }

        [Fact]
        public async Task CreateAsync_WhenFileExistsAndOverwriteFalse_ThrowsException()
        {
            const string originalContent = "Existing content";
            await File.WriteAllTextAsync(_tempFilePath, originalContent);
            var repository = new TextFileRepositoryAsync(_tempFilePath, overwrite: false);

            await Assert.ThrowsAsync<TextFileRepositoryFileAlreadyExistsException>(
                () => repository.Create("New content")
            );

            Assert.Equal(originalContent, await File.ReadAllTextAsync(_tempFilePath));
        }

        [Fact]
        public async Task CreateAsync_WhenFileExistsAndOverwriteTrue_OverwritesFile()
        {
            await File.WriteAllTextAsync(_tempFilePath, "Old content");
            var repository = new TextFileRepositoryAsync(_tempFilePath, overwrite: true);
            const string newContent = "New content";

            await repository.Create(newContent);

            Assert.Equal(newContent, await File.ReadAllTextAsync(_tempFilePath));
        }

        // --- Read Tests ---
        [Fact]
        public async Task ReadAsync_WhenFileExists_ReturnsContent()
        {
            const string expected = "Test content";
            await File.WriteAllTextAsync(_tempFilePath, expected);
            var repository = new TextFileRepositoryAsync(_tempFilePath);

            var actual = await repository.Read();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadAsync_WhenFileDoesNotExist_ThrowsFileNotFoundException()
        {
            var repository = new TextFileRepositoryAsync(_tempFilePath);

            await Assert.ThrowsAsync<FileNotFoundException>(() => repository.Read());
        }

        // --- Update Tests ---
        [Fact]
        public async Task UpdateAsync_OverwritesFileContent()
        {
            await File.WriteAllTextAsync(_tempFilePath, "Initial content");
            var repository = new TextFileRepositoryAsync(_tempFilePath);
            const string newContent = "Updated content";

            await repository.Update(newContent);

            Assert.Equal(newContent, await File.ReadAllTextAsync(_tempFilePath));
        }

        // --- Delete Tests ---
        [Fact]
        public async Task DeleteAsync_WhenFileExists_RemovesFile()
        {
            await File.WriteAllTextAsync(_tempFilePath, "Content to delete");
            var repository = new TextFileRepositoryAsync(_tempFilePath);

            await repository.Delete();

            Assert.False(File.Exists(_tempFilePath));
        }

        [Fact]
        public async Task DeleteAsync_WhenFileDoesNotExist_DoesNothing()
        {
            var repository = new TextFileRepositoryAsync(_tempFilePath);

            await repository.Delete();

            Assert.False(File.Exists(_tempFilePath));
        }

        // --- Edge Cases ---
        [Fact]
        public async Task HandlesLargeFiles()
        {
            var largeContent = new string('X', 1000000);
            var repository = new TextFileRepositoryAsync(_tempFilePath);

            await repository.Create(largeContent);
            var result = await repository.Read();

            Assert.Equal(largeContent.Length, result.Length);
        }

        [Fact]
        public async Task HandlesDifferentEncodings()
        {
            const string content = "特殊字符 ñáéíóú 🚀";
            var repository = new TextFileRepositoryAsync(_tempFilePath);

            await repository.Create(content);
            var result = await repository.Read();

            Assert.Equal(content, result);
        }
    }
}
