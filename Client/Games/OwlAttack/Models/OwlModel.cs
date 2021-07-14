namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class OwlModel
    {
        private const int PURSUE_ALTITUDE = -400;

        private OwlState _state = OwlState.Approach;

        public float Altitude { get; private set; } = 0;
        public float DistanceFromLeft { get; private set; } = 600;

        public int VerticalSpeed { get; set; } = 1;
        public int HorizontalSpeed { get; set; } = 1;

        internal void Move()
        {
            Altitude -= VerticalSpeed;
            DistanceFromLeft -= HorizontalSpeed;

            UpdateState();
        }

        private void UpdateState()
        {
            if (_state == OwlState.Approach)
            {
                if (Altitude <= PURSUE_ALTITUDE)
                {
                    _state = OwlState.Pursue;
                    return;
                }
                VerticalSpeed = 2;
                HorizontalSpeed = 2;
            }

            if (_state == OwlState.Pursue)
            {
                VerticalSpeed = 0;
            }
        }

    }

    enum OwlState
    {
        Approach,
        Pursue,
        Retreat
    }
}
