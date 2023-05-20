using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<PaginatedList<CategoryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }
    }

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();

            GetCategoryPagedSpecifications spec = new(request.PageNumber, request.PageSize, request.Name);
            var categories = await repository.ListAsync(spec, cancellationToken);
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return new PaginatedList<CategoryDto>(categoryDtos, categories.Count, request.PageNumber, request.PageSize);
        }
    }
}
