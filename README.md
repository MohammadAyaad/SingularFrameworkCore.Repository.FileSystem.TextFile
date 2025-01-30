# SingularFrameworkCore.Repository.FileSystem.TextFile

A C# library that provides text file storage implementation for the SingularFrameworkCore repository interface. This library offers both synchronous and asynchronous implementations for storing string data in text files.

## Features

- **Text File Storage**: Simple implementation for storing string data in text files
- **Dual Implementation**: Both synchronous (`TextFileRepository`) and asynchronous (`TextFileRepositoryAsync`) versions
- **CRUD Operations**: Full support for Create, Read, Update, and Delete operations
- **Path Management**: Secure file path handling
- **Exception Handling**: Custom exceptions for common scenarios

## Installation

The package is available on NuGet. To install it, use the following command:

```bash
Install-Package SingularFrameworkCore.Repository.FileSystem.TextFile
```

Or using the .NET CLI:

```bash
dotnet add package SingularFrameworkCore.Repository.FileSystem.TextFile
```

## Usage

### Synchronous Implementation

```csharp
using SingularFrameworkCore.Repository.FileSystem.TextFile;

// Create an instance with a file path
var repository = new TextFileRepository("path/to/your/file.txt");

// Create
repository.Create("Hello, World!");

// Read
string content = repository.Read();

// Update
repository.Update("Updated content");

// Delete
repository.Delete();
```

### Asynchronous Implementation

```csharp
using SingularFrameworkCore.Repository.FileSystem.TextFile;

// Create an instance with a file path
var repository = new TextFileRepositoryAsync("path/to/your/file.txt");

// Create
await repository.Create("Hello, World!");

// Read
string content = await repository.Read();

// Update
await repository.Update("Updated content");

// Delete
await repository.Delete();
```

## Integration with SingularFrameworkCore

This library implements the `ISingularCrudRepository<string>` and `ISingularCrudAsyncRepository<string>` interfaces from SingularFrameworkCore, making it perfect for use with the Singular pipeline:

```csharp
var singular = new Singular<MyClass, string>(
    new TextFileRepository("data.txt"), // or TextFileRepositoryAsync
    serializer,
    preProcessors,
    postProcessors
);
```

## Exception Handling

The library includes a custom exception:

- `TextFileRepositoryFileAlreadyExistsException`: Thrown when attempting to create a file that already exists

```csharp
try 
{
    repository.Create("content");
}
catch (TextFileRepositoryFileAlreadyExistsException ex)
{
    // Handle the case where the file already exists
}
```

## Requirements

- .NET Standard 8.0+ 
- SingularFrameworkCore

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Author
###  Made by [Mohammad Ayaad](https://github.com/MohammadAyaad) ![MohammadAyaad](https://img.shields.io/static/v1?label=|&message=MohammadAyaad&color=grey&logo=github&logoColor=white)

## Original Project

- [SingularFrameworkCore](https://github.com/MohammadAyaad/SingularFrameworkCore) - The core framework this implementation is built for