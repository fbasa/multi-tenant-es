
namespace UniEnroll.Infrastructure.Common.Email;

public sealed class EmailTemplateRenderer
{
    public string RenderWelcome(string name) => $"<p>Hello {System.Net.WebUtility.HtmlEncode(name)}, welcome to UniEnroll!</p>";
}
