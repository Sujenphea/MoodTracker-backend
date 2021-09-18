using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using User = MoodTrackerBackendCosmos.Models.User;
using Octokit;
using System.Threading;
using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MoodTrackerBackendCosmos.GraphQL.UserGraph
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseAppDbContext]
        public async Task<User> AddUserAsync(UserInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var user = new User {
                Id = Guid.NewGuid().ToString(),
                Name = input.Name,
                GitHub = input.GitHub,
                ImageURI = input.ImageURI
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        [UseAppDbContext]
        public async Task<LoginPayload> LoginAsync(LoginInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var client = new GitHubClient(new ProductHeaderValue("MSA-Yearbook"));

            var request = new OauthTokenRequest(Startup.Configuration["Github:ClientId"], Startup.Configuration["Github:ClientSecret"], input.Code);
            var tokenInfo = await client.Oauth.CreateAccessToken(request);

            if (tokenInfo.AccessToken == null)
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Bad code")
                    .SetCode("AUTH_NOT_AUTHENTICATED")
                    .Build());
            }

            client.Credentials = new Credentials(tokenInfo.AccessToken);
            var userClient = await client.User.Current();
            var user = await context.Users.FirstOrDefaultAsync(s => s.GitHub == userClient.Login, cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = userClient.Name ?? userClient.Login,
                    GitHub = userClient.Login,
                    ImageURI = userClient.AvatarUrl
                };

                context.Users.Add(user);
                await context.SaveChangesAsync(cancellationToken);
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
                new Claim("userId", user.Id),
            };

            var jwtToken = new JwtSecurityToken(
                "MSA-Yearbook",
                "MSA-Student",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new LoginPayload(user, token);
        }
    }

    public record UserInput(string Name, string GitHub, string ImageURI);
    public record LoginInput(string Code);
    public record LoginPayload(User User, string Jwt);
}
