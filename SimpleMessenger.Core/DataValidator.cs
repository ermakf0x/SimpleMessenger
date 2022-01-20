namespace SimpleMessenger.Core;

public static class DataValidator
{
    public static IDataValidator<string> PasswordValidator => new _PasswordValidator();
    public static IDataValidator<string> UsernameValidator => new _UsernameValidator();

    class _UsernameValidator : IDataValidator<string>
    {
        bool IDataValidator<string>.HasValid(string data)
        {
            if(data is null) return false;
            if (data.Length < 4 || data.Length > 32) return false;
            if (data == string.Empty) return false;
            if (data.Where(c => !char.IsNumber(c) && !char.IsLetter(c) && c != '_').Any()) return false;

            return true;
        }
    }
    class _PasswordValidator : IDataValidator<string>
    {
        bool IDataValidator<string>.HasValid(string data)
        {
            if(data is null) return false;
            if (data.Length < 8 || data.Length > 32) return false;
            if (data == string.Empty) return false;
            if (data.Where(c => !char.IsNumber(c) && !char.IsLetter(c) && !",./\\|!@#$%^&*()_-=+<>".Contains(c)).Any()) return false;

            return true;
        }
    }
}
