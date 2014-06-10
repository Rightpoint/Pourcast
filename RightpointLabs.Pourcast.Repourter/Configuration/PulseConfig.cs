using System;
using Microsoft.SPOT;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class PulseConfig
    {
        public const double SF800_PULSES_PER_OUNCE = 5600.0/33.814;

        /// <summary>
        /// The number of pulses that should be recieved to indicate one ounce of liquid was dispensed/
        /// </summary>
        public double PulsesPerOunce { get; set; }

        /// <summary>
        /// The delay (in ms) between the first pulse and when the stopped event is triggered (extended every <see cref="PulsesPerStoppedExtension"/> pulses).
        /// </summary>
        public int PourStoppedDelay { get; set; }

        /// <summary>
        /// The number of pulses necessary to extend the <see cref="PourStoppedDelay"/>.
        /// </summary>
        public int PulsesPerStoppedExtension { get; set; }

        /// <summary>
        /// Trigger a 'Pouring' event once every this number of pulses during a pour.
        /// </summary>
        public int PulsesPerPouring { get; set; }
    }
}
