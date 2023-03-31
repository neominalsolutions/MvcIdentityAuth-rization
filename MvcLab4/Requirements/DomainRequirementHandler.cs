using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcLab4.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcLab4.Requirements
{
    public class DomainRequirementHandler : AuthorizationHandler<SpesificDomainRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DomainRequirementHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        // kullanıcı email domain eşleşiyor mu kontrolü
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, SpesificDomainRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                // httpcontext username üzerinden user bulalım 
                var user = await _userManager.FindByNameAsync(context.User.Identity.Name);

                // email requirementdan gelen email içeriyorsa
                if (user.Email.Contains(requirement._requiredDomain))
                {
                    // actiona girmesine izi ver
                    context.Succeed(requirement);
                    await Task.CompletedTask;
                }

                await Task.CompletedTask;
            }


        }
    }
}
