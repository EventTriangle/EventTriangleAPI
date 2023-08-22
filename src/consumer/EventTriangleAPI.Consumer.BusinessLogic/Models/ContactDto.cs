namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public class ContactDto
{
    public string UserId { get; set; }
    
    public string ContactId { get; set; }

    public UserDto Contact { get; set; }
}