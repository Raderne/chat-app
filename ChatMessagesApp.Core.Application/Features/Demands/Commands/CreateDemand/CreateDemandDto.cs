namespace ChatMessagesApp.Core.Application.Features.Demands.Commands.CreateDemand;

public class CreateDemandDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedByUserId { get; set; }
}
