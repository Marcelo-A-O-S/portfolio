using AuthService.Application.DTOs.Request;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Users.Interfaces;
using AuthService.Application.Validations;
using AuthService.Domain.Entities;

namespace AuthService.Application.UseCases.Users
{
    public class ModifyRoleUser : IModifyRoleUser
    {
        private readonly IUserServices userServices;
        public ModifyRoleUser(
            IUserServices _userServices
        )
        {
            this.userServices = _userServices;
        }
        public async Task ExecuteAsync(Guid Id, UpdateRoleRequest updateRoleRequest)
        {
            ValidateRequest(updateRoleRequest);
            var user = await this.GetUserByIdAsync(Id);
            user.UpdateRole(updateRoleRequest.Role);
            await this.userServices.Update(user);
        }
        private static void ValidateRequest(UpdateRoleRequest updateRoleRequest)
        {
            var validationError = ValidationHelper.Validate(updateRoleRequest);
            if(validationError.Count > 0)
            {
                var erros = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {erros}");
            }
        }
        private async Task<User> GetUserByIdAsync(Guid Id)
        {
            var user = await this.userServices.GetById(Id);
            if(user == null)
                throw new NotFoundException("Usuário não encontrado");
            return user;
        }
    }
}