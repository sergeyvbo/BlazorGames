using System.Timers;

namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class PlayerModel
    {
        public int MaxVelocity { get; set; }

        public int Velocity { get; set; }

        public int DistanceFromLeft { get; set; }
        public int Altitude { get; set; }

        public bool CanMove { get; set; }

        private PlayerState _state;
        public  PlayerState State 
        {
            get => _state;
            set
            {
                CanMove = true;
                _state = value;
                if (_state == PlayerState.Empty)
                {
                    MaxVelocity = 3;
                }
                if (_state == PlayerState.Carry)
                {
                    MaxVelocity = 2;
                }
                if (_state == PlayerState.Caught)
                {
                    CanMove = false;
                }
            }
        }

        public int Score { get; set; }
        public int Carry { get; set; }

        private Timer _timer;

        public PlayerModel()
        {
            State = PlayerState.Empty;
            MaxVelocity = 3;
            Velocity = 0;
            DistanceFromLeft = 250;
            Altitude = -400;
            CanMove = true;

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += TimerElapsed;
        }

        public void Move()
        {
            if (!CanMove) return;

            DistanceFromLeft += Velocity;
            if (DistanceFromLeft < 0)
            {
                DistanceFromLeft = 0;
            }
            if (DistanceFromLeft > 550)
            {
                DistanceFromLeft = 550;
            }

            UpdateState();
        }

        private void UpdateState()
        {
            if (State == PlayerState.Caught)
            {
                StopChopping();
                return;
            }

            if (State == PlayerState.Empty)
            {
                if (DistanceFromLeft > 500 && Velocity == 0)
                {
                    State = PlayerState.Chop;
                    StartChopping();
                    return;
                }
            }

            if (State == PlayerState.Chop)
            {
                if (Velocity != 0)
                {
                    State = Carry == 0 ? PlayerState.Empty : PlayerState.Carry;
                    StopChopping();
                }
            }

            if (State == PlayerState.Carry)
            {
                if (DistanceFromLeft > 450 && Velocity == 0)
                {
                    State = PlayerState.Chop;
                    StartChopping();
                    return;
                }

                if (DistanceFromLeft <= 0)
                {
                    Unload();
                    State = PlayerState.Empty;
                    Velocity = 0;
                }
            }
                      
        }

        private void StartChopping()
        {
            _timer.Start();
        }

        private void StopChopping()
        {
            _timer.Stop();
        }

        private void Unload()
        {
            Score += Carry;
            Carry = 0;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Carry += 10;
        }

    }

    public enum PlayerState
    {
        Empty,
        Chop,
        Carry,
        Caught
    }
}
