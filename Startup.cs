using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using dotnet_rpg.Services.WeaponService;
using dotnet_rpg.Services.CharacterSkillService;

namespace dotnet_rpg
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("RpgConnection")));
            services.AddControllers();
			services.AddAutoMapper(typeof(Startup));
			services.AddScoped<ICharacterService, CharacterService>();
			services.AddScoped<IAuthRepository, AuthRepository>();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options => 
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
								Configuration.GetSection("AppSettings:Token").Value)),
								ValidateIssuer = false,
								ValidateAudience = false
						};
					});
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IWeaponService, WeaponService>();
			services.AddScoped<ICharacterSkillService, CharacterSkillService>();
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection(); used for from http to https

            app.UseRouting();
			app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

		
    }
}
		