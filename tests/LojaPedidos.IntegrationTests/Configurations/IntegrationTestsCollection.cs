namespace LojaPedidos.IntegrationTests.Configurations;

[CollectionDefinition(Name)]
public sealed class IntegrationTestsCollection : ICollectionFixture<AspireAppFixture>
{
    public const string Name = "LojaPedidos integration tests";
}
