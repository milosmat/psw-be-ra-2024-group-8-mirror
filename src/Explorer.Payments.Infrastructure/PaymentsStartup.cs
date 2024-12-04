using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Core.Mappers;
using Explorer.Payments.Core.UseCases.Tourist;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure;

public static class PaymentsStartup
{
    public static IServiceCollection ConfigurePaymentsModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PaymentsProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }



    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        services.AddScoped<ITourPurchaseTokenService, TourPurchaseTokenService>();

        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ICouponRepository, CouponDataBaseRepository>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IPaymentRecordService, PaymentRecordService>();

    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<ShoppingCart>), typeof(CrudDatabaseRepository<ShoppingCart, PaymentsContext>));

        services.AddScoped(typeof(ICrudRepository<TourPurchaseToken>), typeof(CrudDatabaseRepository<TourPurchaseToken, PaymentsContext>));

        services.AddScoped(typeof(ICrudRepository<Wallet>), typeof(CrudDatabaseRepository<Wallet, PaymentsContext>));

        services.AddScoped(typeof(ICrudRepository<Transaction>), typeof(CrudDatabaseRepository<Transaction,PaymentsContext>));

        services.AddScoped(typeof(ICrudRepository<Coupon>), typeof(CrudDatabaseRepository<Coupon, PaymentsContext>));

        services.AddScoped(typeof(ICrudRepository<PaymentRecord>), typeof(CrudDatabaseRepository<PaymentRecord, PaymentsContext>));


        services.AddScoped<ICardRepository, CardDataBaseRepository>();
        services.AddScoped<ITourPurchaseTokenRepository, TourPurchaseTokenRepository>();
        services.AddScoped<IWalletRepository, WalletDataBaseRepository>();
        services.AddScoped<ITransactionRepository, TransactionDataBaseRepository>();
        services.AddScoped<IPaymentRecordRepository, PaymentRecordRepository>();

        services.AddDbContext<PaymentsContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("payments"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payments")));
    }
}
