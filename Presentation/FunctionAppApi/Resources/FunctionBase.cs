using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FunctionAppApi.Resources;
public abstract class FunctionBase
{
    protected readonly ILogger _logger;
    protected readonly IMediator _mediator;
    protected readonly IMapper _mapper;

    protected FunctionBase(ILogger logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }
}