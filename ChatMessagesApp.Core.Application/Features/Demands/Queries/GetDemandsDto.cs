﻿namespace ChatMessagesApp.Core.Application.Features.Demands.Queries;

public class GetDemandsDto
{
    public Guid Id { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; }
}