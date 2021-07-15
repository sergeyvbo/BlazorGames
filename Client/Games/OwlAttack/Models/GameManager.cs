using System;
using System.Timers;

namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class GameManager
    {
        private const int FPS = 30;

        public event EventHandler FrameUpdated;
        public OwlModel OwlModel { get; private set; }

        public PlayerModel PlayerModel { get; set; }

        private Timer _timer;

        public GameManager()
        {
            OwlModel = new(this);
            PlayerModel = new();

            _timer = new Timer()
            {
                AutoReset = true,
                Interval = 1000 / FPS,
            };
            _timer.Elapsed += TimerElapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            MainLoop();
        }

        public void MainLoop()
        {
            OwlModel.Move();
            PlayerModel.Move();
            FrameUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
