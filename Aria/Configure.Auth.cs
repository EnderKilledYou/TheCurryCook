using Ender;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;

[assembly: HostingStartup(typeof(Aria.ConfigureAuth))]

namespace Aria;

// Add any additional metadata properties you want to store in the Users Typed Session
public class CustomUserSession : AuthUserSession
{
    public object? TwitchUserId { get; internal set; }
}

// Custom Validator to add custom validators to built-in /register Service requiring DisplayName and ConfirmPassword
public class CustomRegistrationValidator : RegistrationValidator
{
    public CustomRegistrationValidator()
    {
        RuleSet(ApplyTo.Post, () =>
        {
            RuleFor(x => x.DisplayName).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty();
        });
    }
}

public class ConfigureAuth : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        //.ConfigureServices(services => services.AddSingleton<ICacheClient>(new MemoryCacheClient()))
        .ConfigureAppHost(appHost =>
        {
            var appSettings = appHost.AppSettings;
            appHost.Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new IAuthProvider[] {
                    new JwtAuthProvider(appSettings) {
                        AuthKeyBase64 = appSettings.GetString("AuthKeyBase64") ?? "cARl12kvS/Ra4moVBIaVsrWwTpXYuZ0mZf/gNLUhDW5=",
                    },
                    new CredentialsAuthProvider(appSettings),     /* Sign In with Username / Password credentials */
                    new TwitchOauthProvider(appSettings),        /* Create App https://developers.facebook.com/apps */
                    
                })
            {
                IncludeDefaultLogin = false
            });

            appHost.Plugins.Add(new RegistrationFeature()); //Enable /register Service

            //override the default registration validation with your own custom implementation
            appHost.RegisterAs<CustomRegistrationValidator, IValidator<Register>>();
        });
}