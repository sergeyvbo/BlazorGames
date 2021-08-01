using System;
using System.Timers;

namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class OwlModel
    {
        public GameManager GameManager { get; set; }

        private const int START_X = 600;
        private const int START_Y = 0;
        private const int PURSUE_ALTITUDE = -400;
        private const int PURSUE_DISTANCE = 100;

        private const int MaxVelocity = 2;

        private int _pursueStart = 0;

        public OwlState State;

        public int Altitude { get; private set; }
        public int DistanceFromLeft { get; private set; }

        public int VerticalSpeed { get; set; }
        public int HorizontalSpeed { get; set; }

        private Timer _timer;

        public OwlModel()
        {
            Init();
        }

        private void Init()
        {
            Altitude = START_Y;
            DistanceFromLeft = START_X;
            VerticalSpeed = 0;
            HorizontalSpeed = 0;
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Elapsed += TimerElapsed;
            State = OwlState.Approach;
        }

        internal void Move()
        {
            Altitude -= VerticalSpeed;
            DistanceFromLeft += HorizontalSpeed;

            UpdateState();
        }

        private void UpdateState()
        {
            if (State == OwlState.Approach)
            {
                if (HasCaught())
                {
                    State = OwlState.CarryOut;
                    return;
                }

                if (Altitude <= PURSUE_ALTITUDE)
                {
                    State = OwlState.Pursue;
                    _pursueStart = DistanceFromLeft;
                    return;
                }
                
                VerticalSpeed = 2;
                
                if (DistanceFromLeft > GameManager.PlayerModel.DistanceFromLeft + 20)
                {
                    
                    HorizontalSpeed = Math.Min(-MaxVelocity, HorizontalSpeed--);
                }
                else if (DistanceFromLeft < GameManager.PlayerModel.DistanceFromLeft - 20)
                {
                    HorizontalSpeed = Math.Max(MaxVelocity, HorizontalSpeed++);
                }
                else
                {
                    HorizontalSpeed = 0;
                }
                                
            }

            if (State == OwlState.Pursue)
            {
                if (HasCaught())
                {
                    State = OwlState.CarryOut;
                    return;
                }

                if ((_pursueStart - DistanceFromLeft) > PURSUE_DISTANCE)
                {
                    State = OwlState.Retreat;
                    return;
                }

                VerticalSpeed = 0;
                if (DistanceFromLeft > GameManager.PlayerModel.DistanceFromLeft)
                {

                    HorizontalSpeed = Math.Min(-MaxVelocity-1, HorizontalSpeed--);
                }
                else
                {
                    HorizontalSpeed = Math.Max(MaxVelocity+1, HorizontalSpeed++);
                }
            }

            if (State == OwlState.Retreat)
            {
                if (DistanceFromLeft < -50 || Altitude > -50)
                {
                    State = OwlState.Return;
                    return;
                }

                VerticalSpeed = -2;
                HorizontalSpeed = -2;
            }

            if (State == OwlState.Return)
            {
                VerticalSpeed = 0;
                HorizontalSpeed = 0;
                State = OwlState.Await;
                _timer.Start();
            }

            if (State == OwlState.CarryOut)
            {
                if (DistanceFromLeft < -50 || Altitude > -50)
                {
                    GameManager.SetHighScore();
                    GameManager.Stop();
                    return;
                }

                VerticalSpeed = -MaxVelocity;
                HorizontalSpeed = MaxVelocity;
                GameManager.PlayerModel.State = PlayerState.Caught;
                GameManager.PlayerModel.DistanceFromLeft = DistanceFromLeft + 10;
                GameManager.PlayerModel.Altitude = Altitude + 10;
            }

        }

        private bool HasCaught()
        {
            if (Altitude > PURSUE_ALTITUDE) return false;

            var dist = Math.Abs(DistanceFromLeft - GameManager.PlayerModel.DistanceFromLeft);
            if (dist > 50) return false;

            return true;
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            Init();
        }

    }

    public enum OwlState
    {
        Approach,
        Pursue,
        Retreat,
        Return,
        Await,
        CarryOut
    }
}
