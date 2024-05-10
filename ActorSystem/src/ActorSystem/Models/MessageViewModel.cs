public class MessageViewModel
{
    public string? Group { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
}