using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests;

[CollectionDefinition("Test collection")]
public class TestCollection : ICollectionFixture<TestFixture>;
