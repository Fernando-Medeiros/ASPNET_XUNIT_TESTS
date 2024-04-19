using Domain.Enums;

namespace WebApi.Infrastructure;

public static partial class Infrastructure
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        var roles = Enum.GetNames<ERole>().ToList();

        services.AddAuthorization(opt =>
        {
            roles.ForEach(role =>
            {
                opt.AddPolicy(role, policy => policy.RequireRole(role));
            });
        });
    }
}

