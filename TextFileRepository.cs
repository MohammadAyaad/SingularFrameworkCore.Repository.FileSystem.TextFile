namespace SingularFrameworkCore.Repository.FileSystem.TextFile;

public class TextFileRepository : ISingularCrudRepository<string>
{
    public string Path { get; }

    public TextFileRepository(string path)
    {
        this.Path = path;
    }

    public void Create(string entity)
    {
        if (!File.Exists(this.Path))
        {
            File.Create(this.Path);
            File.WriteAllText(this.Path, entity);
        }
        else
            throw new TextFileRepositoryFileAlreadyExistsException("File already exists");
    }

    public string Read()
    {
        return File.ReadAllText(this.Path);
    }

    public void Update(string newEntity)
    {
        File.WriteAllText(this.Path, newEntity);
    }

    public void Delete()
    {
        if (File.Exists(this.Path))
            File.Delete(this.Path);
    }
}

[Serializable]
public class TextFileRepositoryFileAlreadyExistsException : Exception
{
    public TextFileRepositoryFileAlreadyExistsException() { }

    public TextFileRepositoryFileAlreadyExistsException(string message)
        : base(message) { }

    public TextFileRepositoryFileAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }

    protected TextFileRepositoryFileAlreadyExistsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context
    ) { }
}
