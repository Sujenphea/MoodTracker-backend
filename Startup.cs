using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.GraphQL.Dailies;
using MoodTrackerBackendCosmos.GraphQL.DataLoader;
using MoodTrackerBackendCosmos.GraphQL.UserGraph;
using MoodTrackerBackendCosmos.GraphQL.Users;

namespace MoodTrackerBackendCosmos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; } = default!;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            var connectionString = "AccountEndpoint=https://moodtracker.documents.azure.com:443/;AccountKey=7bQ6dhl6Te9y35yOVjQQnYT6BN62UTZSeOTGuqRBYsonjtpQPCDuh1CboKw3j1qG7TbNnIyFAacZ48kZAatYuw==;";
            services.AddPooledDbContextFactory<AppDbContext>(options => options.UseCosmos(connectionString, "moodtracker"));

            services.AddControllers();

            services.AddAuthorization();

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<UserQueries>()
                    .AddTypeExtension<DailyQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<DailyMutations>()
                    .AddTypeExtension<UserMutations>()
                .AddType<DailyType>()
                .AddType<UserType>()
                .AddDataLoader<DailiesByUserIdDataLoader>()
            ;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSecret"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidIssuer = "MSA-Yearbook",
                            ValidAudience = "MSA-Student",
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey
                        };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                endpoints.MapControllers();
            });
        }
    }
}
