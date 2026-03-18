using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    // Relación de navegación
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();
    private readonly List<Project> _projects = [];

    protected User() { } // Constructor vacío para EF Core

    public User(string fullName, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new DomainException("El hash de la contraseña no puede estar vacío.");
        }

        PasswordHash = newPasswordHash;
    }
}
