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

        private OwlState _state;

        public int Altitude { get; private set; }
        public int DistanceFromLeft { get; private set; }

        public int VerticalSpeed { get; set; }
        public int HorizontalSpeed { get; set; }

        private Timer _timer;

        public OwlModel(GameManager manager)
        {
            GameManager = manager;
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
            _state = OwlState.Approach;
        }

        internal void Move()
        {
            Altitude -= VerticalSpeed;
            DistanceFromLeft += HorizontalSpeed;

            UpdateState();
        }

        private void UpdateState()
        {
            if (_state == OwlState.Approach)
            {
                if (HasCaught())
                {
                    _state = OwlState.CarryOut;
                    return;
                }

                if (Altitude <= PURSUE_ALTITUDE)
                {
                    _state = OwlState.Pursue;
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

            if (_state == OwlState.Pursue)
            {
                if (HasCaught())
                {
                    _state = OwlState.CarryOut;
                    return;
                }

                if ((_pursueStart - DistanceFromLeft) > PURSUE_DISTANCE)
                {
                    _state = OwlState.Retreat;
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

            if (_state == OwlState.Retreat)
            {
                if (DistanceFromLeft < -50 || Altitude > -50)
                {
                    _state = OwlState.Return;
                    return;
                }

                VerticalSpeed = -2;
                HorizontalSpeed = -2;
            }

            if (_state == OwlState.Return)
            {
                VerticalSpeed = 0;
                HorizontalSpeed = 0;
                _state = OwlState.Await;
                _timer.Start();
            }

            if (_state == OwlState.CarryOut)
            {
                if (DistanceFromLeft < -50 || Altitude > -50)
                {
                    GameManager.Stop();
                    return;
                }

                VerticalSpeed = -MaxVelocity;
                HorizontalSpeed = MaxVelocity;
                GameManager.PlayerModel.State = PlayerState.Caught;
                GameManager.PlayerModel.DistanceFromLeft = DistanceFromLeft - 5;
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

    enum OwlState
    {
        Approach,
        Pursue,
        Retreat,
        Return,
        Await,
        CarryOut
    }
}
