using Application.Cache;
using Application.Data;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Mediator;

namespace Application.Features.Users.Commands;

public record CreateUserCommand(uint Id, string Name, int Age) : ICommand<UserDto>;

internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService cacheService;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        this.cacheService = cacheService;
    }

    public async ValueTask<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User user = User.Create(command.Id, command.Name, command.Age);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new(user.Id, command.Name, command.Age);
    }
}
