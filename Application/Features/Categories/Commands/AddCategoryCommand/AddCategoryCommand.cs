using Application.Common.Models;
using Application.Common.Models.Responses;
using Application.Common.Specifications;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.Features.Categories.Commands.AddCategoryCommand;

public class AddCategoryCommand : AddCategoryDto, IRequest<Result<int>>
{
}

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Category>();

        var categoryExistsSpec = new GetCategoryByNameSpecification(request.Name);
        var existingCategory = await repository.FirstOrDefaultAsync(categoryExistsSpec, cancellationToken);

        if (existingCategory != null)
        {
            throw new BusinessException("Category with the same name already exists.", HttpStatusCode.Conflict);

        }

        var category = _mapper.Map<Category>(request);
        await repository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<int>.Success(category.Id);
    }
}
