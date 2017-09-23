namespace AudioPPM
{
    /// <summary>
    /// Freqently used PPM settings for popular transmitters
    /// </summary>
    public static class StandartProfiles
    {
        public static PpmProfile FlySky =>
            new PpmProfile()
            {
                PauseDuration = 400,
                Polarity = PpmPolarity.HIGH,
                MinChannelDuration = 1000,
                MaxChannelDuration = 2000,
                Period = 20000
            };
    }
}