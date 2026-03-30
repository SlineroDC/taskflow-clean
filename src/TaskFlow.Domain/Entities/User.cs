using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Avatar { get; private set; } = "/images/avatars/default.png";

    // Relación de navegación
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();
    private readonly List<Project> _projects = [];

    protected User() { } // Constructor vacío para EF Core

    public User(
        string fullName,
        string email,
        string passwordHash,
        string avatar = "/images/avatars/default.png"
    )
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Avatar = avatar;
    }

    public void UpdateProfile(string fullName, string avatar)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainException("El nombre completo no puede estar vacío.");
        }

        FullName = fullName;
        Avatar = avatar;
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
