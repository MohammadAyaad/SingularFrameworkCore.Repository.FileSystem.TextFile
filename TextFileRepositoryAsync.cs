namespace SingularFrameworkCore.Repository.FileSystem.TextFile;

public class TextFileRepositoryAsync : ISingularCrudAsyncRepository<string>
{
    public string Path { get; }

    public TextFileRepositoryAsync(string path)
    {
        this.Path = path;
    }

    public async Task Create(string entity)
    {
        if (!File.Exists(this.Path))
        {
            File.Create(this.Path).Close();
            await File.WriteAllTextAsync(this.Path, entity);
        }
        else
            throw new TextFileRepositoryFileAlreadyExistsException("File already exists");
    }

    public async Task<string> Read()
    {
        return await File.ReadAllTextAsync(this.Path);
    }

    public async Task Update(string newEntity)
    {
        await File.WriteAllTextAsync(this.Path, newEntity);
    }

    public Task Delete()
    {
        if (File.Exists(this.Path))
            File.Delete(this.Path);
        return Task.CompletedTask;
    }
}
