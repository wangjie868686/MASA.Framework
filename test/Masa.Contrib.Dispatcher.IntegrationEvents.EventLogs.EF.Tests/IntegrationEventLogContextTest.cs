namespace Masa.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF.Tests;

[TestClass]
public class IntegrationEventLogContextTest : TestBase
{
    [TestMethod]
    public void TestCreateDbContext()
    {
        var serviceProvider = CreateDefaultProvider(option => option.UseEventLog<CustomDbContext>());

        var customDbContext = serviceProvider.GetRequiredService<CustomDbContext>();
        var entity = customDbContext.Model.GetEntityTypes()
            .FirstOrDefault(entityType => entityType.Name == typeof(IntegrationEventLog).FullName)!;

        Assert.IsTrue(entity.GetTableName() == "IntegrationEventLog");
        var properties = entity.GetProperties().ToList();
        Assert.IsTrue(properties.Where(x => x.Name == "Id").Select(x => x.IsPrimaryKey()).FirstOrDefault());
        Assert.IsFalse(properties.Where(x => x.Name == "Id").Select(x => x.IsNullable).FirstOrDefault());
        Assert.IsFalse(properties.Where(x => x.Name == "Content").Select(x => x.IsNullable).FirstOrDefault());
        Assert.IsFalse(properties.Where(x => x.Name == "CreationTime").Select(x => x.IsNullable).FirstOrDefault());
        Assert.IsFalse(properties.Where(x => x.Name == "State").Select(x => x.IsNullable).FirstOrDefault());
        Assert.IsFalse(properties.Where(x => x.Name == "TimesSent").Select(x => x.IsNullable).FirstOrDefault());
        Assert.IsFalse(properties.Where(x => x.Name == "EventTypeName").Select(x => x.IsNullable).FirstOrDefault());

        var integrationEventLogDbContext = serviceProvider.GetRequiredService<IntegrationEventLogContext>();
        Assert.IsTrue(customDbContext == integrationEventLogDbContext.DbContext);
    }

    [TestMethod]
    public void TestAddDbContext()
    {
        var services = new ServiceCollection();
        services.AddDbContext<CustomDbContext>(options => options.UseSqlite(Connection));
        var serviceProvider = services.BuildServiceProvider();

        var dbContext = serviceProvider.GetService<IntegrationEventLogContext>();
        Assert.IsTrue(dbContext == null);

        Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetService<CustomDbContext>());
    }

    [TestMethod]
    public void TestUseEventLog()
    {
        var dispatcherOptions = new DispatcherOptions(new ServiceCollection());
        dispatcherOptions.Services.AddDbContext<CustomDbContext>(options => options.UseSqlite(Connection));
        dispatcherOptions.UseEventLog<CustomDbContext>();
        var serviceProvider = dispatcherOptions.Services.BuildServiceProvider();

        Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetService<IntegrationEventLogContext>());

        Assert.ThrowsException<InvalidOperationException>(() => serviceProvider.GetService<CustomDbContext>());
    }
}