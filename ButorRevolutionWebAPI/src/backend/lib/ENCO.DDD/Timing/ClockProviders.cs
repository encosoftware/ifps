namespace ENCO.DDD.Timing
{
    public static class ClockProviders
    {
        public static UnspecifiedClockProvider Unspecified { get; } = new UnspecifiedClockProvider();
        public static LocalClockProvider Local { get; } = new LocalClockProvider();
    }
}
