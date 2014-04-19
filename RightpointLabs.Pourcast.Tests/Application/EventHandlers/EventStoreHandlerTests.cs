namespace RightpointLabs.Pourcast.Tests.Application.EventHandlers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ploeh.AutoFixture;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    [TestClass]
    public class EventStoreHandlerTests
    {
        [TestClass]
        public class HandleMethod
        {
            [TestMethod]
            public void AddsEventToRepository()
            {
                var fixture = new Fixture();
                var mockEvent = fixture.Create<BeerPourEnded>();
                var mockRepo = new Mock<IStoredEventRepository>();
                var mockDateTimeProvider = new Mock<IDateTimeProvider>();

                var sut = new EventStoreHandler<BeerPourEnded>(mockRepo.Object, mockDateTimeProvider.Object);

                sut.Handle(mockEvent);

                mockRepo.Verify(r => r.Add(It.IsAny<StoredEvent>()));
            }
        }
    }
}
