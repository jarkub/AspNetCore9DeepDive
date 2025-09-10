using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AuthMvc.Policies;

public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}



public class MinimumAgeAuthorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
        {
            // User does not have a DateOfBirth claim, so the requirement cannot be met.
            return Task.CompletedTask;
        }

        var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
        if (DateTime.TryParse(dateOfBirthClaim.Value, out DateTime dateOfBirth))
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement); // Requirement met
            }
        }

        return Task.CompletedTask;
    }
}

public class CustomClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Example: Add a custom claim if not already present
        if (!principal.HasClaim(c => c.Type == "CustomClaimType"))
        {
            var identity = (ClaimsIdentity)principal.Identity;
            identity.AddClaim(new Claim("CustomClaimType", "CustomClaimValue"));
        }
        return Task.FromResult(principal);

        //if (!principal.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
        //{
        //    if (principal.Identity is null || !principal.Identity.IsAuthenticated)
        //    {
        //        // User is not authenticated, cannot add claims
        //        return Task.FromResult(principal);
        //    }
        //    var identity = (ClaimsIdentity)principal.Identity;
        //    identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, "21"));
        //}

        //return Task.FromResult(principal);
    }
}