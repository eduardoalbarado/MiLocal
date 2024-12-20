using Application.Common.Models;
using Application.Common.Models.Responses;
using Application.Exceptions;
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
            var spec = new CategoryByIdSpecification(request.Id);
            var category = await repository.FirstOrDefaultAsync(spec, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException("Category", request.Id);
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.Success(categoryDto);
        }
    }
}
