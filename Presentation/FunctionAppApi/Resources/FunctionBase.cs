global using Application.Common.Models;
global using AutoMapper;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Azure.Functions.Worker;
global using Microsoft.Azure.Functions.Worker.Http;
global using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
global using Microsoft.Extensions.Logging;
global using Microsoft.OpenApi.Models;
global using System.Net;
global using System.Threading.Tasks;



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