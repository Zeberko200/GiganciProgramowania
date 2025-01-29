namespace Application.DTOs;

public record SendMessageRequest(string Message);
public record SendMessageResponse(Guid ResponseMessageId);