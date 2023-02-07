using System.Reflection;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using MailKit;

namespace EBird.Api.Configurations
{
    public static class ConfigureApplicationService
    {
        public static void AddAppServices (this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddScoped<IBirdTypeService, BirdTypeService>();
            services.AddScoped<IBirdService, BirdService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRuleService, RuleService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
