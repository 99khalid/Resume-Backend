//using Microsoft.AspNetCore.Http;

//namespace Infrastructure
//{
//    public class LoggedInUserProvider
//    {
//        public static Guid GetLoggedInUserId(IHttpContextAccessor httpContextAccessor)
//        {
//            try
//            {
//                var userIdClaim = httpContextAccessor.HttpContext.User.Claims
//                    .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

//                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
//                {
//                    return userId;
//                }

//                throw new UnauthorizedAccessException("User ID claim not found or not valid.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                throw new UnauthorizedAccessException("Failed to retrieve logged-in user ID.");
//            }
//        }
//    }
//}
