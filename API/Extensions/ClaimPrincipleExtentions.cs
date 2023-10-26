using System.Security.Claims;

namespace API.Extensions
{
  public static class ClaimPrincipleExtentions
    {

        public static string GetUsername(this ClaimsPrincipal user) 
{
    var usernameClaim = user.FindFirst(ClaimTypes.Name);

    if (usernameClaim != null)
    {
        var username = usernameClaim.Value;
        Console.WriteLine($"Username found: {username}");
        return username;
    }

    Console.WriteLine("No username claim found.");
    return null;
}

    }
}