using Application.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.AddUser
{
    public class AddUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            var userSpec = new UserByB2CUserIdSpec(request.UserId);
            var existingUser = await userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (existingUser != null)
            {
                return Unit.Value;
            }

            var user = new User
            {
                B2CUserId = request.UserId,
                Email = request.Email,
                FirstName = request.UserName,
                LastName = string.Empty
            };

            await userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
