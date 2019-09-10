using AspNet.Security.OpenIdConnect.Primitives;
using AspNetCore.Identity.Mongo;
using lp_api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using System;

namespace lp_api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private MongoConnection _mongoConnection { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mongoConnection = Configuration.GetSection("MongoConnection").Get<MongoConnection>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            AddIdentityCoreServices(services);

            AddOpenIddict(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }

        private void AddIdentityCoreServices(IServiceCollection services)
        {
            services.AddIdentityMongoDbProvider<UserEntity, UserRoleEntity>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = false;
            }, mongoIdentityOptions => {
                mongoIdentityOptions.ConnectionString = $"{_mongoConnection.ConnectionString}{_mongoConnection.Database}";
            });

            var builder = services.AddIdentityCore<UserEntity>();
            builder = new IdentityBuilder(builder.UserType, typeof(UserRoleEntity), builder.Services);
            builder.AddRoles<UserRoleEntity>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<UserEntity>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            });
        }

        private void AddOpenIddict(IServiceCollection services)
        {
            services.AddOpenIddict()
                .AddCore(options => 
                {
                    options.UseMongoDb().UseDatabase(new MongoClient(_mongoConnection.ConnectionString).GetDatabase(_mongoConnection.Database));
                })
                .AddServer(options => 
                {
                    options.UseMvc();
                    options.EnableTokenEndpoint("/token");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();
                    options.DisableHttpsRequirement();
                })
                .AddValidation();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });
        }
    }
}
