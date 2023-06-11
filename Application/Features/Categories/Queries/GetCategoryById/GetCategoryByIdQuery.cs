using Application.Common.Models;
using Application.Common.Models.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<Result<CategoryDto>>
    {
        public int Id { get; set; }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (category == null)
            {
                return Result<CategoryDto>.Failure($"Category with Id {request.Id} not found");
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.Success(categoryDto);
        }
    }
}
