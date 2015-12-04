using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using RightpointLabs.Pourcast.Domain.Events;
using RightpointLabs.Pourcast.Domain.Models;
using RightpointLabs.Pourcast.Domain.Repositories;
using RightpointLabs.Pourcast.Domain.Services;

namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    public class StateTracker
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Lazy<Task<Dictionary<string, MessageUserInfo>>> _getUsers; 
        public StateTracker(IMessagePoster messagePoster)
        {
            _messagePoster = messagePoster;
            _getUsers = new Lazy<Task<Dictionary<string, MessageUserInfo>>>(() => _messagePoster.GetUsers());
        }

        private readonly IMessagePoster _messagePoster;
        private object _lockObject = new object();
        private Dictionary<string, Tuple<DateTime, PictureTaken>> _lastPicturesTaken = new Dictionary<string, Tuple<DateTime, PictureTaken>>();
        private List<CancellationTokenSource> _stoppedWaitSources = new List<CancellationTokenSource>();

        private void Handle(PictureTaken domainEvent)
        {
            if (string.IsNullOrEmpty(domainEvent.TapId))
                return;

            lock (_lockObject)
            {
                var tpl = new Tuple<DateTime, PictureTaken>(DateTime.UtcNow, domainEvent);
                _lastPicturesTaken[domainEvent.TapId] = tpl;
                _stoppedWaitSources.ForEach(i => i.Cancel());

                log.DebugFormat("Handled PictureTaken @ {0} ({1} characters), cancelling {2} waits", tpl.Item1.ToLocalTime(), domainEvent.DataUrl.Length, _stoppedWaitSources.Count);
                
                _stoppedWaitSources = new List<CancellationTokenSource>();
            }
        }

        private void Handle(PourStopped domainEvent, ITapNotificationStateRepository stateRepository, Keg keg, Beer beer)
        {
            try
            {
                log.DebugFormat("Handling PourStopped of {0}", domainEvent.Volume);
                if (domainEvent.Volume < 2)
                    return;
                var now = DateTime.Now;
                // wait until 2PM
                if (now.TimeOfDay < TimeSpan.FromHours(14))
                    return;
                // only post on Fridays
                if (now.DayOfWeek != DayOfWeek.Friday)
                    return;

                Task toWaitFor;
                lock (_lockObject)
                {
                    var state = stateRepository.GetByTapId(domainEvent.TapId);
                    if (null == state)
                    {
                        state = new TapNotificationState(string.Empty) { TapId = domainEvent.TapId, Today = DateTime.Today };
                        stateRepository.Add(state);
                    }

                    Tuple<DateTime, PictureTaken> lpt = null;
                    _lastPicturesTaken.TryGetValue(domainEvent.TapId, out lpt);

                    log.DebugFormat("State: LP: {0}, BP: {1}, LPTT: {2}", state.Today, state.TodaysBiggestPour, lpt == null ? null : (DateTime?)lpt.Item1);

                    Func<PictureTaken, Task> buildMessage = null;

                    var beerName = beer == null ? "an unknown beer" : beer.Name;

                    if (state.Today.Date == now.Date && state.TodaysNotificationThreadId.HasValue && state.KegId == domainEvent.KegId)
                    {
                        if (!state.TodaysBiggestPour.HasValue || state.TodaysBiggestPour.Value + 1 < domainEvent.Volume)
                        {
                            // biggest pour...
                            buildMessage = async pt =>
                            {
                                var msg = string.Format("Biggest pour of {0} for the day!  A solid {1:0.0}oz!", beerName, domainEvent.Volume);
                                if (pt.AddedOverlay.GetValueOrDefault())
                                {
                                    msg += "  Looking spiffy.";
                                }
                                string contentType = null;
                                string filename = null;
                                byte[] filedata = null;

                                int[] users = new int[0];
                                if (null != pt)
                                {
                                    filedata = GetDataFromUrl(pt.DataUrl, out contentType);
                                    filename = "image.jpg";

                                    if (pt.Faces != null && pt.Faces.Any())
                                    {
                                        var faces = new HashSet<string>(pt.Faces.Select(i => i.Split('@')[0].ToLowerInvariant()));
                                        var allUsers = await _getUsers.Value;
                                        users = AddUsers(ref msg, allUsers.Where(i => faces.Contains(i.Key.Split('@')[0])).Select(i => i.Value).ToArray());
                                    }
                                }

                                _messagePoster.PostReply(state.TodaysNotificationThreadId.Value, msg, users, filename, contentType, filedata);
                            };
                            state.TodaysBiggestPour = domainEvent.Volume;
                            stateRepository.Update(state);
                        }
                        else
                        {
                            // added overlay
                            buildMessage = async pt =>
                            {
                                if (null == pt || !pt.AddedOverlay.GetValueOrDefault())
                                {
                                    return;
                                }

                                var msg = string.Format("A solid pour of {1:0.0}oz of {0}!  Looking spiffy.", beerName, domainEvent.Volume);
                                string contentType = null;
                                string filename = null;
                                byte[] filedata = null;

                                int[] users = new int[0];
                                filedata = GetDataFromUrl(pt.DataUrl, out contentType);
                                filename = "image.jpg";

                                if (pt.Faces != null && pt.Faces.Any())
                                {
                                    var faces = new HashSet<string>(pt.Faces.Select(i => i.Split('@')[0].ToLowerInvariant()));
                                    var allUsers = await _getUsers.Value;
                                    users = AddUsers(ref msg, allUsers.Where(i => faces.Contains(i.Key.Split('@')[0])).Select(i => i.Value).ToArray());
                                }

                                _messagePoster.PostReply(state.TodaysNotificationThreadId.Value, msg, users, filename, contentType, filedata);
                            };
                        }
                    }
                    else
                    {
                        state.Today = now.Date;
                        state.KegId = domainEvent.KegId;

                        // first pour
                        buildMessage = async (PictureTaken pt) =>
                        {
                            var msg = string.Format("First pour of the day of {0}!  A solid {1:0.0}oz!", beerName, domainEvent.Volume);
                            if (pt.AddedOverlay.GetValueOrDefault())
                            {
                                msg += "  Looking spiffy.";
                            }
                            string contentType = null;
                            string filename = null;
                            byte[] filedata = null;

                            int[] users = new int[0];
                            if (null != pt)
                            {
                                filedata = GetDataFromUrl(pt.DataUrl, out contentType);
                                filename = "image.jpg";

                                if (pt.Faces != null && pt.Faces.Any())
                                {
                                    var faces = new HashSet<string>(pt.Faces.Select(i => i.Split('@')[0].ToLowerInvariant()));
                                    var allUsers = await _getUsers.Value;
                                    users = AddUsers(ref msg, allUsers.Where(i => faces.Contains(i.Key.Split('@')[0])).Select(i => i.Value).ToArray());
                                }
                            }

                            var newState = stateRepository.GetByTapId(domainEvent.TapId);
                            if (null != newState)
                            {
                                newState.TodaysNotificationThreadId = _messagePoster.PostNewMessage(msg, users, filename, contentType, filedata);
                                stateRepository.Update(newState);
                            }
                        };
                        state.TodaysBiggestPour = domainEvent.Volume;
                        stateRepository.Update(state);
                    }

                    log.DebugFormat("Has Message? {0}", buildMessage != null);
                    if (null == buildMessage)
                        return;

                    if (null != lpt)
                    {
                        var age = DateTime.UtcNow.Subtract(lpt.Item1);
                        if (age < TimeSpan.FromSeconds(30))
                        {
                            log.DebugFormat("Sending message sync");
                            // only use a current picture
                            var pic = lpt.Item2;
                            Task.Run(async () =>
                            {
                                try
                                {
                                    await buildMessage(pic);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Sending async", ex);
                                }
                            });
                            return;
                        }
                    }

                    log.DebugFormat("Preparing to send message async");
                    var cts = new CancellationTokenSource();
                    _stoppedWaitSources.Add(cts);
                    toWaitFor = Task.Delay(TimeSpan.FromSeconds(10), cts.Token).ContinueWith(async i =>
                    {
                        try
                        {
                            log.DebugFormat("Sending message async");
                            _lastPicturesTaken.TryGetValue(domainEvent.TapId, out lpt);
                            await buildMessage(null == lpt ? null : lpt.Item2);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Sending async", ex);
                        }
                    });
                }

                toWaitFor.Wait();
                log.DebugFormat("Message sent async");
            }
            catch(Exception ex)
            {
                log.Error("Error processing PourStopped", ex);
            }
        }

        private int[] AddUsers(ref string msg, MessageUserInfo[] nearbyUsers)
        {
            int[] users = null;
            if (null != nearbyUsers && nearbyUsers.Length > 0)
            {
                users = nearbyUsers.Select(i => i.id).ToArray();
                var userNames = nearbyUsers.Select(i => "@" + i.name).ToArray();
                if (userNames.Length == 2)
                {
                    userNames = new[] { userNames[0] + " and " + userNames[1] };
                }
                else if (userNames.Length > 2)
                {
                    userNames = userNames.Take(userNames.Length - 2).Concat(new[] { userNames[userNames.Length - 2] + ", and " + userNames[userNames.Length - 1] }).ToArray();
                }

                msg += " Do I see " + string.Join(", ", userNames) + "?";
            }
            return users;
        }

        private byte[] GetDataFromUrl(string dataUrl, out string contentType)
        {
            // https://gist.github.com/vbfox/484643
            var match = Regex.Match(dataUrl, @"data:image/(?<type>.+?);base64,(?<data>.+)");
            var type = match.Groups["type"].Value;
            var base64Data = match.Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            contentType = "image/" + type;
            return binData;
        }
    
        public class StateTrackerEventHandler : IEventHandler<PictureTaken>, IEventHandler<PourStopped>
        {
            private readonly StateTracker _tracker;
            private readonly ITapNotificationStateRepository _tapNotificationStateRepository;
            private readonly IKegRepository _kegRepository;
            private readonly IBeerRepository _beerRepository;

            public StateTrackerEventHandler(StateTracker tracker, ITapNotificationStateRepository tapNotificationStateRepository, IKegRepository kegRepository, IBeerRepository beerRepository)
            {
                _tracker = tracker;
                _tapNotificationStateRepository = tapNotificationStateRepository;
                _kegRepository = kegRepository;
                _beerRepository = beerRepository;
            }

            public void Handle(PictureTaken domainEvent)
            {
                _tracker.Handle(domainEvent);
            }

            public void Handle(PourStopped domainEvent)
            {
                var keg = _kegRepository.GetById(domainEvent.KegId);
                var beer = null == keg ? null : _beerRepository.GetById(keg.BeerId);

                _tracker.Handle(domainEvent, _tapNotificationStateRepository, keg, beer);
            }
        }
    }
}
