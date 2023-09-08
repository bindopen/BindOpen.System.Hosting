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
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        public static IServiceCollection AddSingletonBdoScope<TImplementation>(
            this IServiceCollection services,
            Func<IBdoHost, TImplementation> setupAction)
            where TImplementation : class, IBdoScope
            => services.AddBdoScope<TImplementation>(ServiceLifetime.Singleton, setupAction);

        /// <summary>
        /// Adds a BidnOpen scoped service as scoped.
        /// </summary>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        public static IServiceCollection AddScopedBdoScope<TImplementation>(
            this IServiceCollection services,
            Func<IBdoHost, TImplementation> setupAction)
            where TImplementation : class, IBdoScope
            => services.AddBdoScope<TImplementation>(ServiceLifetime.Scoped, setupAction);

        /// <summary>
        /// Adds a BidnOpen scoped service as transient.
        /// </summary>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        public static IServiceCollection AddTransientBdoScope<TImplementation>(
            this IServiceCollection services,
            Func<IBdoHost, TImplementation> setupAction)
            where TImplementation : class, IBdoScope
            => services.AddBdoScope<TImplementation>(ServiceLifetime.Transient, setupAction);

        /// <summary>
        /// Adds a BidnOpen hosted service.
        /// </summary>
        /// <typeparam name="TImplementation">The service implementation to consider.</typeparam>
        /// <param key="services">The set of services to populate.</param>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <param key="serviceLifetime">The service life time to consider.</param>
        /// <returns>Returns the updated service set.</returns>
        private static IServiceCollection AddBdoScope<TImplementation>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            Func<IBdoHost, TImplementation> setupAction)
            where TImplementation : class, IBdoScope
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
                    services.AddSingleton<IBdoScope, TImplementation>(initializer);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<IBdoScope, TImplementation>(initializer);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<IBdoScope, TImplementation>(initializer);
                    break;
            }

            return services;
        }
    }
}