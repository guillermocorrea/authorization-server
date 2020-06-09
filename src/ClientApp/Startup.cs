using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ClientApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ClientApp
{
  public class Startup
  {
    public Startup()
    {
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<IJobBoardApiClient, JobBoardApiClient>();

      services.AddAuthentication(opt =>
        {
          opt.DefaultScheme = "Cookies";
          opt.DefaultChallengeScheme = "oidc";
        }).AddCookie("Cookies", (opt) =>
        {
          opt.AccessDeniedPath = "/Account/AccessDenied"; // This is the default value.
        })
        .AddOpenIdConnect("oidc", opt =>
        {
          opt.SignInScheme = "Cookies";
          opt.Authority = "https://localhost:5005";
          opt.ClientId = "mvc-client";
          opt.ResponseType = "code id_token";
          opt.SaveTokens = true;
          opt.ClientSecret = "MVCSecret";
          opt.GetClaimsFromUserInfoEndpoint = true;
          opt.ClaimActions.DeleteClaims(new string[] { "sid", "idp" });
          opt.Scope.Add("address");
          // opt.ClaimActions.MapUniqueJsonKey("address", "address");
          opt.Scope.Add("roles");
          opt.ClaimActions.MapUniqueJsonKey("role", "role");

          opt.TokenValidationParameters = new TokenValidationParameters
          {
            RoleClaimType = "role"
          };

          opt.Scope.Add("jobBoardApi");

          opt.Scope.Add("position");
          opt.Scope.Add("country");
          opt.ClaimActions.MapUniqueJsonKey("position", "position");
          opt.ClaimActions.MapUniqueJsonKey("country", "country");
        });

      services.AddAuthorization(authOpt =>
      {
        authOpt.AddPolicy("CanCreateAndModifyData", policyBuilder =>
        {
          policyBuilder.RequireAuthenticatedUser();
          policyBuilder.RequireClaim("position", "Administrator");
          policyBuilder.RequireClaim("country", "USA");
        });
      });

      services.AddControllersWithViews();
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
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}