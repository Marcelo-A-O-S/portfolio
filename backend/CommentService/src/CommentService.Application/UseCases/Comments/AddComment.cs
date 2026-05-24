using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using System.Text.Json;

namespace CommentService.Application.UseCases.Comments
{
    public class AddComment : IAddComment
    {
        public Task ExecuteAsync(CommentRequest commentRequest)
        {
            throw new NotImplementedException();
        }
        private static void ValidateRequest(CommentRequest commentRequest)
        {
            var validationError = ValidationHelper.Validate(commentRequest);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ",validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
    }
}