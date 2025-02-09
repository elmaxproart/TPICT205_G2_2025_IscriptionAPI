using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using gradeManagerServerAPi.Data.UserManagementAPI.Data;
using gradeManagerServerAPi.Models.UserConnexion;
using gradeManagerServerAPi.Services.UserService._2FA;
using Microsoft.EntityFrameworkCore;

public class ValidationCodeService
{
    private readonly ApplicationDbContext _context;

    public ValidationCodeService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Générer un code et le stocker en BD
    public async Task<string> GenerateAndSaveCodeAsync(ApplicationUser user)
    {
        var code = new Random().Next(100000, 999999).ToString(); // Code 6 chiffres
        var hashedCode = HashCode(code); // Hacher le code
        var expiration = DateTime.UtcNow.AddMinutes(10); // Expiration dans 10 minutes

        // Supprimer les anciens codes de l'utilisateur
        var oldCodes = _context.ValidationCodes.Where(v => v.UserId == user.Id);
        _context.ValidationCodes.RemoveRange(oldCodes);

        var validationCode = new ValidationCode2FA
        {
            UserId = user.Id,
            HashedCode = hashedCode,
            CreatedAt = DateTime.UtcNow,
            Expiration = expiration
        };

        await _context.ValidationCodes.AddAsync(validationCode);
        await _context.SaveChangesAsync();

        return code; // Retourner le code en clair pour l'envoyer par email
    }

    // Vérifier si un code est valide
    public async Task<bool> ValidateCodeAsync(ApplicationUser user, string code)
    {
        var validationCode = await _context.ValidationCodes
            .Where(v => v.UserId == user.Id && v.Expiration > DateTime.UtcNow)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();

        if (validationCode == null)

        {
            Debug.WriteLine($"Code fourni : {code}, Code en BD : {validationCode.HashedCode}");

            return false; // Aucun code valide trouvé
        }
        Debug.WriteLine($"Code fourni : {code}, Code en BD : {validationCode.HashedCode}");

        return VerifyHashedCode(code, validationCode.HashedCode);
    }

    // Hachage du code
    private string HashCode(string code)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(code);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    // Vérification du code hashé
    private bool VerifyHashedCode(string code, string hashedCode)
    {
        return HashCode(code) == hashedCode;
    }
}
