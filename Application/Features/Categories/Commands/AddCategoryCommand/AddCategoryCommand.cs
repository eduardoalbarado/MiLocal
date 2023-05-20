using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.AddCategoryCommand
{
    public class AddCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            var category = _mapper.Map<Category>(request);
            await repository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }
    }
}
