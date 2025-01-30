using Application.DTOs;
using MediatR;

namespace Application.Queries.Requests;

public record GetMessagesQuery(string UserIpAddress) : IRequest<GetMessageResponse>;