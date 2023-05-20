using Application.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.DeleteCategoryCommand
{
    public class DeleteCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            await repository.DeleteAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
