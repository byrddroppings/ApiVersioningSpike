using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ApiVersioningSpike
{
    public interface IGreetingBuilder
    {
        string Build();
    }
    
    public class GreetingBuilder1 : IGreetingBuilder
    {
        public string Build() => "[v1] greetings!";
    }

    public class GreetingBuilder2 : IGreetingBuilder
    {
        public string Build() => $"[v2] greetings!";
    }

    public static class GreetingBuilderExtensions
    {
        public static IServiceCollection AddGreetingBuilders(this IServiceCollection services)
        {
            services.AddScoped<GreetingBuilder1>();
            services.AddScoped<GreetingBuilder2>();

            services.AddScoped<IGreetingBuilder>(serviceProvider =>
            {
                var requestedVersion = serviceProvider
                    .GetRequiredService<IHttpContextAccessor>().HttpContext
                    .GetRequestedApiVersion()
                    ?? ApiVersion.Default;

                return requestedVersion.MajorVersion switch
                {
                    1 => serviceProvider.GetRequiredService<GreetingBuilder1>(),
                    _ => serviceProvider.GetRequiredService<GreetingBuilder2>(),
                };
            });
            
            return services;
        }
    }
}