using BudgetApp.Models;
namespace BudgetApp.DTOs
{
    public static class UserMappingExtensions
    {
        public static UserResponse ToDto(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };
        }

        public static User ToEntity(this RegisterRequest request, string passwordHash)
        {
            return new User
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = passwordHash,
            };
        }
    }
}
