using Blazored.LocalStorage;
using System;
using System.Timers;

namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class GameManager
    {
        private const int FPS = 30;
        private readonly ISyncLocalStorageService _localStorage;

        public int HighScore { get; set; }

        public event EventHandler FrameUpdated;
        public OwlModel OwlModel { get; private set; }

        public PlayerModel PlayerModel { get; set; }

        private Timer _timer;

        public GameManager(ISyncLocalStorageService localStorage)
        {
            Init();
            _localStorage = localStorage;
            HighScore = LoadHighScore();
        }

        public void Init()
        {
            OwlModel = new();
            OwlModel.GameManager = this;
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
            Init();
            Start();
        }

        public int LoadHighScore()
        {
            return _localStorage.GetItem<int>("highScore");
        }

        public void SetHighScore()
        {
            if (PlayerModel.Score > HighScore)
            {
                _localStorage.SetItem<int>("highScore", PlayerModel.Score);
                HighScore = PlayerModel.Score;
            }
            
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
