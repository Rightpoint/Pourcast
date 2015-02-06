using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
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
using RightpointLabs.Pourcast.Domain.Services;

namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    public class StateTracker
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public StateTracker(IMessagePoster messagePoster)
        {
            _messagePoster = messagePoster;
        }

        private readonly IMessagePoster _messagePoster;
        private object _lockObject = new object();
        private DateTime? _lastPour = null;
        private double? _biggestPour = null;
        private int? _todaysMessage = null;
        private DateTime? _lastPictureTakenTime = null;
        private PictureTaken _lastPictureTaken = null;
        private List<CancellationTokenSource> _stoppedWaitSources = new List<CancellationTokenSource>();

        private void Handle(PictureTaken domainEvent)
        {
            lock (_lockObject)
            {
                _lastPictureTakenTime = DateTime.UtcNow;
                _lastPictureTaken = domainEvent;
                _stoppedWaitSources.ForEach(i => i.Cancel());

                log.DebugFormat("Handled PictureTaken @ {0} ({1} characters), cancelling {2} waits", _lastPictureTakenTime.Value.ToLocalTime(), domainEvent.DataUrl.Length, _stoppedWaitSources.Count);
                
                _stoppedWaitSources = new List<CancellationTokenSource>();
            }
        }


        private void Handle(PourStopped domainEvent)
        {
            try
            {
                log.DebugFormat("Handling PourStopped of {0}", domainEvent.Volume);
                if (domainEvent.Volume < 2)
                    return;
                var now = DateTime.Now;
                //if (now.TimeOfDay < TimeSpan.FromHours(14))
                //    return;

                Task toWaitFor;
                lock (_lockObject)
                {
                    log.DebugFormat("State: LP: {0}, BP: {1}, LPTT: {2}", _lastPour, _biggestPour, _lastPictureTakenTime);

                    Action<PictureTaken> buildMessage = null;

                    if (_lastPour.HasValue && _lastPour.Value.Date == now.Date && _todaysMessage.HasValue)
                    {
                        if (!_biggestPour.HasValue || _biggestPour.Value + 1 < domainEvent.Volume)
                        {
                            // biggest pour...
                            buildMessage = pt =>
                            {
                                var msg = string.Format("Biggest pour of the day!  A solid {0:0.0}oz!", domainEvent.Volume);
                                string contentType = null;
                                string filename = null;
                                byte[] filedata = null;

                                if (null != pt)
                                {
                                    filedata = GetDataFromUrl(pt.DataUrl, out contentType);
                                    filename = "image.jpg";
                                }

                                _messagePoster.PostReply(_todaysMessage.Value, msg, filename, contentType, filedata);
                            };
                            _biggestPour = domainEvent.Volume;
                        }
                    }
                    else
                    {
                        // first pour
                        buildMessage = pt =>
                        {
                            var msg = string.Format("First pour of the day!  A solid {0:0.0}oz!", domainEvent.Volume);
                            string contentType = null;
                            string filename = null;
                            byte[] filedata = null;

                            if (null != pt)
                            {
                                filedata = GetDataFromUrl(pt.DataUrl, out contentType);
                                filename = "image.jpg";
                            }

                            _todaysMessage = _messagePoster.PostNewMessage(msg, filename, contentType, filedata);
                        };
                        _biggestPour = domainEvent.Volume;
                    }

                    _lastPour = now;

                    log.DebugFormat("Has Message? {0}", buildMessage != null);
                    if (null == buildMessage)
                        return;

                    if (_lastPictureTakenTime.HasValue && null != _lastPictureTaken)
                    {
                        var age = DateTime.UtcNow.Subtract(_lastPictureTakenTime.Value);
                        if (age < TimeSpan.FromSeconds(30))
                        {
                            log.DebugFormat("Sending message sync");
                            // only use a current picture
                            buildMessage(_lastPictureTaken);
                            return;
                        }
                    }

                    log.DebugFormat("Preparing to send message async");
                    var cts = new CancellationTokenSource();
                    _stoppedWaitSources.Add(cts);
                    toWaitFor = Task.Delay(TimeSpan.FromSeconds(10), cts.Token).ContinueWith(i =>
                    {
                        log.DebugFormat("Sending message async");
                        buildMessage(_lastPictureTaken);
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

            public StateTrackerEventHandler(StateTracker tracker)
            {
                _tracker = tracker;
            }

            public void Handle(PictureTaken domainEvent)
            {
                _tracker.Handle(domainEvent);
            }

            public void Handle(PourStopped domainEvent)
            {
                _tracker.Handle(domainEvent);
            }
        }
    }
}
