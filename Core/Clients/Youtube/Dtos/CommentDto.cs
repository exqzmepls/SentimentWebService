namespace Core.Clients.Youtube.Dtos;

public class CommentDto
{
    public CommentDto(string author, string htmlText, string plainText)
    {
        Author = author;
        HtmlText = htmlText;
        PlainText = plainText;
    }

    public string Author { get; }
    public string HtmlText { get; }
    public string PlainText { get; }
}
