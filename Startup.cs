using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Quizon
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
        }

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapPost("/api/auth/send-otp", async context =>
                {
                    var request = await context.Request.ReadFromJsonAsync<SendOtpRequest>();
                    var authService = context.RequestServices.GetRequiredService<IAuthService>();
                    var result = await authService.SendOtpAsync(request.Email);
                    context.Response.StatusCode = result ? 200 : 400;
                });

                endpoints.MapPost("/api/auth/verify-otp", async context =>
                {
                    var request = await context.Request.ReadFromJsonAsync<VerifyOtpRequest>();
                    var authService = context.RequestServices.GetRequiredService<IAuthService>();
                    var result = await authService.VerifyOtpAsync(request.Email, request.Otp);
                    context.Response.StatusCode = result ? 200 : 400;
                });
            });
        }
    }
}