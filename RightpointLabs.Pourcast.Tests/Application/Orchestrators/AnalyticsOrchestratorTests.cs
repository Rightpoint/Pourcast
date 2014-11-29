using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RightpointLabs.Pourcast.Application.Orchestrators.Concrete;
using RightpointLabs.Pourcast.Domain.Events;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Repositories;
using RightpointLabs.Pourcast.Domain.Repositories.Fakes;

namespace RightpointLabs.Pourcast.Tests.Application.Orchestrators
{
    [TestClass]
    public class AnalyticsOrchestratorTests
    {
        private DateTime startTime = new DateTime(2014, 02, 11, 16, 00, 00);
        private const int KegCapacity = 1984;
        private StubIKegRepository _kegRepository;
        private StubIBeerRepository _beerRepository;
        private StubIStoredEventRepository _storedEventRepository;
        private ICollection<StoredEvent> _storedEvents; 

        private void SetupRepos()
        {
            _beerRepository = new StubIBeerRepository();
            _kegRepository = new StubIKegRepository();
            _storedEventRepository = new StubIStoredEventRepository();

            _beerRepository.GetByIdString = id => new Beer("testbeer", "Miller Light");
            _kegRepository.GetByIdString = id => new Keg("testkeg", "testbeer", 1984);
            _storedEventRepository.GetAllOf1<PourStopped>(() => _storedEvents);
            _storedEventRepository.FindOf1FuncOfStoredEventBoolean<PourStopped>(func => _storedEvents);
            CreateStoredEvent();
        }

        private void CreateStoredEvent()
        {
            _storedEvents = new Collection<StoredEvent>();
            var amount = 1984;
            var startDate = startTime;
            var eventId = "event{0}";
            var currId = 1;
            var kegId = "testkeg";
            var tapId = "testtap1";

            while (amount > 0)
            {
                amount -= 16;
                var e = new StoredEvent(string.Format(eventId, currId), startDate.AddMinutes(currId),
                    new PourStopped(tapId, kegId, 16, (double)amount/KegCapacity));
                _storedEvents.Add(e);
                currId++;
            }
        }

        [TestMethod]
        public void Test_GetKegsDurationOnTap_Method()
        {
            SetupRepos();
            var orch = new AnalyticsOrchestrator(_kegRepository, _beerRepository, _storedEventRepository, null);
            var amtOf16OzPours = KegCapacity / 16;
            TimeSpan kegTimeSpan = startTime - startTime.AddMinutes(amtOf16OzPours);

            var result = orch.GetKegsDurationOnTap("testkeg", 5);

            var burndownCount = 21;

            Assert.AreEqual(burndownCount, result.Burndowns.Count());

        }
    }
}