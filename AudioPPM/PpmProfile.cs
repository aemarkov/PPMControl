namespace AudioPPM
{
    /// <summary>
    /// Polarity of the various length part of PPM signal
    /// </summary>
    public enum PpmPolarity
    {
        HIGH,
        LOW
    }

    /// <summary>
    /// Set of PPM settings (timings and polarity)
    /// </summary>
    public struct PpmProfile
    {
        /// <summary>
        /// Duration of the pause
        /// (part of PPM cycle which duration is constant)
        /// </summary>
        public int PauseDuration { get; set; }

        /// <summary>
        /// Polarity of the various length part of PPM signal
        /// </summary>
        public PpmPolarity Polarity { get; set; }

        /// <summary>
        /// Minimum period duration (mks)
        /// </summary>
        public int MinChannelDuration { get; set; }

        /// <summary>
        /// Maximum period duration (mks)
        /// </summary>
        public int MaxChannelDuration { get; set; }

        /// <summary>
        /// Whole period (all channels + sync idle) (mks)
        /// </summary>
        public int Period { get; set; }
    }
}