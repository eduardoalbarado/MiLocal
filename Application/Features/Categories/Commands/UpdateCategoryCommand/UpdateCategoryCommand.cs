using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryCommand : UpdateCategoryDto, IRequest<Unit>
    {
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            category.Name = request.Name;
            await repository.UpdateAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
