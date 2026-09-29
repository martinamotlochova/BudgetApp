// Services/AuthValidationService.cs
using BudgetApp.DTOs;

namespace BudgetApp.Services
{
    public class AuthValidationService
    {
        public string? ValidateRegisterRequest(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return "Name cannot be empty.";
            }

            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                return "Invalid email format.";
            }

            if (!IsValidPassword(request.Password))
            {
                return "Password must be at least 8 characters long, with an uppercase letter and a digit.";
            }

            return null;
        }

        public string? ValidateLoginRequest(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                return "Invalid email format.";
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return "Password cannot be empty.";
            }
            return null;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsDigit)) return false;
            return true;
        }
    }
}