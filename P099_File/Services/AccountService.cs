using Microsoft.AspNetCore.Identity;
using P099_File.Models;
using P099_File.Services;
using System.Security.Cryptography;

public interface IAccountService
{
    bool Login(string username, string password, out string role);
    Account SignupNewAccount(string username, string password);
}

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _dbContext;

    public AccountService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Account SignupNewAccount(string username, string password)
    {
        var account = CreateAccount(username, password);

        // Save the account to the database
        _dbContext.Accounts.Add(account);
        _dbContext.SaveChanges(); // Persist changes to the database

        return account;
    }

    private Account CreateAccount(string username, string password)
    {
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        return new Account
        {
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "User"
        };
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool Login(string username, string password, out string role)
    {
        // Retrieve account from the database
        var account = _dbContext.Accounts.FirstOrDefault(a => a.Username == username);
        role = account.Role; 
        if (account != null && VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
        {
            return true;
        }

        return false;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
}
