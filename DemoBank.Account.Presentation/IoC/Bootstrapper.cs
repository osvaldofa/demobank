using DemoBank.Account.Domain.Interfaces;
using DemoBank.Account.Domain.Services;
using DemoBank.Account.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DemoBank.Account.Presentation
{
    public class Bootstrapper
    {
        /// <summary>
        /// Configure dependency injections.
        /// </summary>
        /// <param name="service">Service collection instantiated on StartUp class.</param>
        public static void ConfigureInjections(IServiceCollection service) 
        {
            // Configure dependency injection for Domain Services
            ConfigureServiceInjections(service);

            // Configure dependency injection for Repositories
            ConfigureRepositoryInjections(service);
        }

        private static void ConfigureServiceInjections(IServiceCollection service)
        {
            // Attaching Account service dependency injection.
            service.AddSingleton(typeof(IAccountService), typeof(AccountService));

            // Attaching Transaction service dependency injection.
            service.AddSingleton(typeof(ITransactionServices), typeof(TransactionServices));
        }

        private static void ConfigureRepositoryInjections(IServiceCollection service)
        {
            // Attaching Account Repository for data access provider.
            service.AddSingleton(typeof(IAccountRepository), typeof(AccountRepository));

            // Attaching Customer Repository for data access provider.
            service.AddSingleton(typeof(ICustomerRepository), typeof(CustomerRepository));

            // Attaching Transaction Repository for data access provider.
            service.AddSingleton(typeof(ITransactionRepository), typeof(TransactionRepository));
        }
    }
}
