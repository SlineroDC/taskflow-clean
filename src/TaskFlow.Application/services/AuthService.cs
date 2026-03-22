using System;
using System.Threading.Tasks;
using BCrypt.Net;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        // 1. Verificar si el correo ya existe
        var existingUser = await userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new DomainException("El correo electrónico ya está en uso.");

        // 2. Encriptar la contraseña con BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 3. Crear el usuario
        var user = new User(dto.FullName, dto.Email, passwordHash);

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<User> LoginAsync(LoginDto dto)
    {
        // 1. Buscar el usuario
        var user =
            await userRepository.GetByEmailAsync(dto.Email)
            ?? throw new DomainException("Credenciales inválidas.");

        // 2. Verificar la contraseña
        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValid)
            throw new DomainException("Credenciales inválidas.");

        return user;
    }

    public async Task ChangePasswordAsync(Guid id, ChangePasswordDto dto)
    {
        // 1. Validaciones de entrada
        if (dto.NewPassword != dto.ConfirmNewPassword)
            throw new DomainException("La nueva contraseña y la confirmación no coinciden.");

        // Buscamos al usuario por su Id real
        var user =
            await userRepository.GetByIdAsync(id)
            ?? throw new DomainException("Usuario no encontrado.");

        // 2. Verificar que la contraseña actual sea la correcta antes de permitir el cambio
        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new DomainException("La contraseña actual es incorrecta.");

        // 3. Generar el nuevo Hash
        var newHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        // 4. Llamamos al método de la entidad
        user.UpdatePassword(newHash);

        // 5. Persistir los cambios en la base de datos
        await userRepository.SaveChangesAsync();
    }

    // --- NUEVO MÉTODO PARA GOOGLE LOGIN ---
    public async Task<User> HandleExternalLoginAsync(string email, string name)
    {
        // 1. Buscamos si el usuario ya existe por su correo
        var user = await userRepository.GetByEmailAsync(email);

        // 2. Si no existe, lo creamos automáticamente
        if (user == null)
        {
            var randomPassword = Guid.NewGuid().ToString("N") + "Aa1@";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(randomPassword);

            user = new User(name, email, hashedPassword);

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
        }

        // 3. Devolvemos el usuario (nuevo o existente) para que el Controlador arme la Cookie
        return user;
    }
}
