using Application.Common.Models;
using Application.Common.Specifications;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.AddCategoryCommand;

public class AddCategoryCommand : AddCategoryDto, IRequest<int>
{
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

        var categoryExistsSpec = new GetCategoryByNameSpecification(request.Name);
        var existingCategory = await repository.FirstOrDefaultAsync(categoryExistsSpec);

        if (existingCategory != null)
        {
            throw new ConflictException("Category with the same name already exists.");
        }
        
        var category = _mapper.Map<Category>(request);
        await repository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return category.Id;
    }
}
