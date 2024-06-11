using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Authentication.Queries.GetUsers;

public record GetUsersQuery : IRequest<Result<List<UserDto>>>;