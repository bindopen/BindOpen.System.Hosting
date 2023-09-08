using BindOpen.Kernel.Hosting.Hosts;
using BindOpen.Kernel.Scoping;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This static class extends .Net core dependency injection namespace.
    /// </summary>
    public static partial class BindOpenServiceCollectionExtensions
    {
        // BindOpen hosted services --------------------------

        /// <summary>
        /// Adds a BidnOpen hosted service as singleton.
        /// </summary>
        /// <typeparam name="TService">The interface of BindOpen hosted service to consider.</typeparam>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        public static IServiceCollection AddSingletonBdoService<TService, TImplementation>(
            this IServiceCollection services,
            Func<IBdoHost, TImplementation> setupAction)
            where TService : class, IBdoScoped
            where TImplementation : class, TService
            => services.AddBdoService<TService, TImplementation>(ServiceLifetime.Singleton, setupAction);

        /// <summary>
        /// Adds a BidnOpen scoped service as scoped.
        /// </summary>
        /// <typeparam name="TService">The interface of BindOpen hosted service to consider.</typeparam>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        public static IServiceCollection AddScopedBdoService<TService, TImplementation>(
            this IServiceCollection services,
            Func<IBdoHost, TImplementation> setupAction)
            where TService : class, IBdoScoped
            where TImplementation : class, TService
            => services.AddBdoService<TService, TImplementation>(ServiceLifetime.Scoped, setupAction);

        /// <summary>
        /// Adds a BidnOpen scoped service as transient.
        /// </summary>
        /// <typeparam name="TService">The interface of BindOpen hosted service to consider.</typeparam>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        public static IServiceCollection AddTransientBdoService<TService, TImplementation>(
            this IServiceCollection services,
            Func<IBdoHost, TImplementation> setupAction)
            where TService : class, IBdoScoped
            where TImplementation : class, TService
            => services.AddBdoService<TService, TImplementation>(ServiceLifetime.Transient, setupAction);

        /// <summary>
        /// Adds a BidnOpen hosted service.
        /// </summary>
        /// <typeparam name="TService">The interface of BindOpen hosted service to consider.</typeparam>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <param key="serviceLifetime">The service life time to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        private static IServiceCollection AddBdoService<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            Func<IBdoHost, TImplementation> setupAction)
            where TService : class, IBdoScoped
            where TImplementation : class, TService
        {
            TImplementation initializer(IServiceProvider p)
            {
                var host = p.GetService<IBdoHost>();
                var repo = setupAction?.Invoke(host);

                return repo;
            }

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TService, TImplementation>(initializer);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<TService, TImplementation>(initializer);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<TService, TImplementation>(initializer);
                    break;
            }

            return services;
        }
    }
}