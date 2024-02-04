namespace TestWebAPI.Models
{
    public record ServerMessage (string Object, string Body, List<int> Recipients);
}
