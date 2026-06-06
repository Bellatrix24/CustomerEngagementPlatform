using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerEngagementPlatform.Helpers
{
    public class CustomUserManager : UserManager<IdentityUser>
    {
        public CustomUserManager(
            IUserStore<IdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<CustomUserManager> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (!user.Email!.Equals("staff@demo.com", StringComparison.OrdinalIgnoreCase))
                {
                    await AddToRoleAsync(user, "Customer");
                }
            }
            return result;
        }

        public override async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await base.CreateAsync(user);
            if (result.Succeeded)
            {
                if (!user.Email!.Equals("staff@demo.com", StringComparison.OrdinalIgnoreCase))
                {
                    await AddToRoleAsync(user, "Customer");
                }
            }
            return result;
        }
    }
}
