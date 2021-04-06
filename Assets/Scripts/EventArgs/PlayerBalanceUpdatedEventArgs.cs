namespace EventArgs
{
    public class PlayerBalanceUpdatedEventArgs : System.EventArgs
    {
        public long PlayerBalance { get; }

        public int Multipler { get; set; }

        public long InitialWin { get; set; }
    }
}