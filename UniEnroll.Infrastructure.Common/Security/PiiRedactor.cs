
using System.Text.RegularExpressions;

namespace UniEnroll.Infrastructure.Common.Security;

public sealed class PiiRedactor
{
    private static readonly Regex EmailRx = new(@"(?<=.).(?=[^@]*?@)", RegexOptions.Compiled);
    public string RedactEmail(string email) => EmailRx.Replace(email, "*");
}
