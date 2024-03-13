using Microsoft.AspNetCore.Mvc;

namespace WAFido2;

public static class UrlHelperExtensions
{
    public static string ToGithub(this IUrlHelper url, string path)
    {
        return "https://github.com/passwordless-lib/fido2-net-lib/blob/master/" + path;
    }
}
