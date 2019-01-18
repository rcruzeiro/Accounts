using System;
using System.IO;
using System.Reflection;
using Accounts.API.Services;
using Accounts.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Accounts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Title = "User Repository",
                    Version = "v1",
                    Description = "Microservice for storing user accounts with OAuth framework."
                });
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });

            #region Redis configuration
            var redisConfiguration = new RedisDTO();
            new ConfigureFromConfigurationOptions<RedisDTO>(
                Configuration.GetSection("Redis")).Configure(redisConfiguration);
            services.AddSingleton(redisConfiguration);
            #endregion

            #region JWT configuration
            var signingConfigurations = new SigninConfiguration();
            services.AddSingleton(signingConfigurations);
            var tokenConfigurations = new TokenDTO();
            new ConfigureFromConfigurationOptions<TokenDTO>(
                Configuration.GetSection("Token")).Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);
            // API Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var paramsValidation = options.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                // validate token signature
                paramsValidation.ValidateIssuerSigningKey = true;
                // validate token lifetime
                paramsValidation.ValidateLifetime = true;
                // token clock skew
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });
            // API Authorization
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                            .RequireAuthenticatedUser().Build());
            });
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error");

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                s.RoutePrefix = "docs";
            });
        }
    }
}
