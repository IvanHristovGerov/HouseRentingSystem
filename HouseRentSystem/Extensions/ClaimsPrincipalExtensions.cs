
namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        //This is our Identity class

        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
