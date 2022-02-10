namespace SimpleMessenger.Core;

public static class ValidationHelper
{
    public static bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        if (password.Length < 8 || password.Length > 32) return false;
        if (password.Where(c => !char.IsNumber(c) && !char.IsLetter(c) && !",./\\|!@#$%^&*()_-=+<>".Contains(c)).Any()) return false;

        return true;
    }
    public static bool ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        if (username.Length < 4 || username.Length > 32) return false;
        if (username.Where(c => !char.IsNumber(c) && !char.IsLetter(c) && c != '_').Any()) return false;

        return true;
    }
}