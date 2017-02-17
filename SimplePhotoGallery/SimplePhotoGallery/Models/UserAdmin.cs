using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;

namespace SimplePhotoGallery.Models
{
    public class UserAdmin
    {
        // Database connection string name for users
        string myConStr;
        // Default identity DB context
        IdentityDbContext myDbContext;
        // Default user store
        UserStore<IdentityUser> myUserStore;
        // Default user manager
        UserManager<IdentityUser> myUserManager;
        // Default signin manager
        SignInManager<IdentityUser, string> mySignInManager;

        public UserAdmin()
        {
            // Constructor to initialize user service objects.
            myConStr = "UserStoreConnection";
            myDbContext = new IdentityDbContext(myConStr);
            myUserStore = new UserStore<IdentityUser>(myDbContext);
            myUserManager = new UserManager<IdentityUser>(myUserStore);
            IAuthenticationManager authManager = HttpContext.Current.GetOwinContext().Authentication;
            mySignInManager = new SignInManager<IdentityUser, string>(myUserManager, authManager);
        }

        public IdentityResult AddUser(IdentityUser objUser, string strPassword)
        {
            IdentityResult result = myUserManager.Create(objUser, strPassword);
            return result;
        }

        public bool SignInUser(string strUserName, string strPassword)
        {
            IdentityUser user = myUserManager.Find(strUserName, strPassword);
            if (user != null)
            {
                mySignInManager.SignIn(user, false, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SignOut()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
        }
    }
}