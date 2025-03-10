using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ResumeBuilderAPI.ServiceExtensions
{
    public static class ServiceExtension
    {



        public static void AddUnitOfWorkRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddScoped(typeof(IGRepository<>), typeof(GRepository<>));
        }
        public static void AddDiServices(this IServiceCollection services)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(AppDomain.CurrentDomain.Load("Application"));
            services.AddScoped(typeof(IGRepository<>), typeof(GRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            //services.AddSingleton<ITokenService, TokenService>();
        }

        public static void AddRateLimitings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRateLimiter(options =>
            {
                var rateLimitSection = configuration.GetSection("RateLimit");
                var permitLimit = rateLimitSection.GetValue<int>("PermitLimit");
                var queueLimit = rateLimitSection.GetValue<int>("QueueLimit");
                var windowSeconds = rateLimitSection.GetValue<int>("WindowSeconds");
                options.AddFixedWindowLimiter("FixedWindowPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(windowSeconds);
                    opt.PermitLimit = permitLimit;
                    opt.QueueLimit = queueLimit;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                }).RejectionStatusCode = 429;
            });
        }




    }
}
