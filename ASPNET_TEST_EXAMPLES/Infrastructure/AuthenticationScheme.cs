using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Infrastructure;

internal sealed class TokenEnvironment(IConfiguration x)
{
    public string PrivateKey { get; } = x.GetValue<string>("PRIVATE_KEY")!;
    public double AccessTokenExp { get; } = x.GetValue<double>("TOKEN_ACCESS_EXP");
    public double RefreshTokenExp { get; } = x.GetValue<double>("TOKEN_REFRESH_EXP");
    public double RecoverPasswordTokenExp { get; } = x.GetValue<double>("TOKEN_RECOVER_PASS_EXP");
    public double AuthenticateEmailTokenExp { get; } = x.GetValue<double>("TOKEN_AUTH_EMAIL_EXP");
}

public static partial class Infrastructure
{
    public static void AddAuthenticationSchemes(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var tokenEnvironment = new TokenEnvironment(configuration);

        services
            .AddAuthentication(opt =>
            {
                string scheme = JwtBearerDefaults.AuthenticationScheme;

                opt.DefaultAuthenticateScheme = scheme;
                opt.DefaultChallengeScheme = scheme;
            })
            .AddJwtBearer(opt =>
            {
                byte[] key = Encoding.ASCII.GetBytes(tokenEnvironment.PrivateKey);

                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            }
        );
    }
}