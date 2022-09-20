using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Text;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace Ender;

public class TwitchOauthProvider : OAuth2Provider
{
    public const string Name = "twitch";
    public static string Realm = "https://id.twitch.tv/";


    public bool RetrieveEmail { get; set; } = true;

    public override Dictionary<string, string> Meta { get; } = new Dictionary<string, string>
    {
        [Keywords.Allows] = Keywords.AccessTokenAuth,
    };

    public TwitchOauthProvider(IAppSettings appSettings)
        : base(appSettings, Realm, Name)
    {
        AuthorizeUrl = "https://id.twitch.tv/oauth2/authorize";
        AccessTokenUrl = "https://id.twitch.tv/oauth2/token";
        RequestTokenUrl = "https://id.twitch.tv/oauth2/auth";
        CallbackUrl = appSettings.Get<string>("Twitch:CallBack");
        ConsumerKey = appSettings.Get<string>("Twitch:Id");
        Scopes = appSettings.Get<string[]>("Twitch:Scopes") ?? Array.Empty<string>();
        ConsumerSecret = appSettings.Get<string>("Twitch:Secret");

        Icon = Svg.ImageSvg(
            "<svg xmlns='http://www.w3.org/2000/svg' fill='currentColor' viewBox='0 0 20 20'><path d='M6.29 18.251c7.547 0 11.675-6.253 11.675-11.675 0-.178 0-.355-.012-.53A8.348 8.348 0 0020 3.92a8.19 8.19 0 01-2.357.646 4.118 4.118 0 001.804-2.27 8.224 8.224 0 01-2.605.996 4.107 4.107 0 00-6.993 3.743 11.65 11.65 0 01-8.457-4.287 4.106 4.106 0 001.27 5.477A4.073 4.073 0 01.8 7.713v.052a4.105 4.105 0 003.292 4.022 4.095 4.095 0 01-1.853.07 4.108 4.108 0 003.834 2.85A8.233 8.233 0 010 16.407a11.616 11.616 0 006.29 1.84' /></svg>");
        NavItem = new NavItem
        {
            Href = "/auth/" + Name,
            Label = "Sign In with Twitch",
            Id = "btn-" + Name,
            ClassName = "btn-social btn-twitch",
            IconClass = "fab svg-twitch",
        };
    }


    protected override async Task<Dictionary<string, string>> CreateAuthInfoAsync(string accessToken,
        CancellationToken token = new CancellationToken())
    {
        return new Dictionary<string, string>()
        {
        };
    }

    protected override async Task LoadUserAuthInfoAsync(AuthUserSession user, IAuthTokens tokens,
        Dictionary<string, string> authInfo, CancellationToken token = default)
    {
        if (user is not CustomUserSession userSession)
        {
            return;
        }


        var json = await DownloadTwitchUserInfoAsync(
            ConsumerKey, ConsumerSecret,
            tokens.AccessToken, tokens.AccessTokenSecret, token).ConfigAwait();
        if (json == null)
        {
            throw new Exception("Could not get user info from helix");
        }

        tokens.DisplayName = json.DisplayName;
        tokens.UserName = json.Login;
        tokens.UserId = json.Login;
        tokens.Email = json.Email?? json.Login + "@twitch.tv";
        tokens.Items[AuthMetadataProvider.ProfileUrlKey] = json.ProfileImageUrl.SanitizeOAuthUrl();


        userSession.UserAuthName = tokens.Email ?? json.Login;


        await LoadUserOAuthProviderAsync(userSession, tokens);
    }

    public override Task LoadUserOAuthProviderAsync(IAuthSession authSession, IAuthTokens tokens)
    {
        if (authSession is not CustomUserSession userSession) return Task.CompletedTask;

        userSession.TwitchUserId = tokens.UserId ?? userSession.TwitchUserId;
        userSession.UserName = tokens.UserName ?? userSession.UserName;
        userSession.DisplayName = tokens.DisplayName ?? userSession.DisplayName;
        userSession.Email = tokens.Email ?? userSession.Email;

        return Task.CompletedTask;
    }

    static async Task<User?> DownloadTwitchUserInfoAsync(string consumerKey,
        string consumerSecret, string accessToken, string accessTokenSecret, CancellationToken token = default)
    {
        var api = new TwitchAPI(settings: new ApiSettings()
        {
            AccessToken = accessToken,
            ClientId = consumerKey,
            Secret = consumerSecret,
        });
        try
        {
            var usr = await api.Helix.Users.GetUsersAsync();
            return usr.Users.Length == 0 ? null : usr.Users[0];
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    static async Task<AuthId?> VerifyTwitchAccessTokenAsync(string consumerKey,
        string consumerSecret, string accessToken, string accessTokenSecret, CancellationToken token = default)
    {
        var api = new TwitchAPI(settings: new ApiSettings()
        {
            AccessToken = accessToken,
            ClientId = consumerKey,
            Secret = consumerSecret,
        });
        try
        {
            var usr = await api.Helix.Users.GetUsersAsync();
            if (usr == null) return null;
            return new AuthId()
            {
                Email = usr.Users[0].Login + "@twitch.tv",
                UserId = usr.Users[0].Login
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}