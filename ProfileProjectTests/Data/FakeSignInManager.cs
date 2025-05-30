using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileProjectTests.Data
{
    public class FakeSignInManager : SignInManager<IdentityUser>
    {
        public FakeSignInManager()
            : base(
                  new Mock<FakeUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                  new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                  new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
                  new Mock<Microsoft.AspNetCore.Identity.IUserConfirmation<IdentityUser>>().Object
                  )
        { }
    }
}
