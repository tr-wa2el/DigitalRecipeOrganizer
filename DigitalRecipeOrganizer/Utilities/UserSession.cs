using DigitalRecipeOrganizer.Models;

namespace DigitalRecipeOrganizer.Utilities
{
    /// <summary>
    /// Manages the current user session
    /// </summary>
    public static class UserSession
    {
        public static User? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static void Login(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
