namespace hangnow_back.DataTransferObject;

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CreateTagDto
{
    public string Name { get; set; }
}