using Azure;

namespace NextPost.Api.Helpers
{
    public static class CookieHelper
    {
        public static void SetRefreshToken(HttpResponse response, string refreshToken, DateTime expires)
        {
            var cookie = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
            };

            response.Cookies.Append("RefreshToken", refreshToken, cookie);
        }

        public static string? GetRefreshToken(HttpRequest request)
        {
            if(request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                return refreshToken;
            }

            return null;
        }
    }
}
