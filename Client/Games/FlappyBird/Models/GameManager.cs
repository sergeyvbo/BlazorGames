using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGames.Client.Games.FlappyBird.Models
{
    public class GameManager
    {
        public const int GRAVITY = 2;

        public event EventHandler MainLoopCompleted;

        public BirdModel Bird { get; private set; }
        public List<PipeModel> Pipes { get; private set; }

        public bool IsRunning { get; private set; } = false;

        public GameManager()
        {
            Bird = new();
            Pipes = new();
        }

        public async void MainLoop()
        {
            IsRunning = true;
            while (IsRunning)
            {
                MoveObjects();
                CheckForCollisions();
                ManagePipes();

                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(20);
            }
        }

        public void StartGame()
        {
            if (IsRunning)
            {
                return;
            }
            Bird = new();
            Pipes = new();
            MainLoop();
        }

        public void Jump()
        {
            if (!IsRunning)
            {
                return;
            }

            Bird.Jump();
        }

        void CheckForCollisions()
        {
            if (Bird.IsOnGround())
            {
                GameOver();
            }

            var centeredPipe = Pipes.FirstOrDefault(pipe => pipe.IsCentered());

            if (centeredPipe is not null)
            {
                bool hasCollidedWithBottom = Bird.DistanceFromGround < centeredPipe.GapBottom - 150;
                bool hasCollidedWithTop = Bird.DistanceFromGround + 45 > centeredPipe.GapTop - 150;

                if (hasCollidedWithBottom || hasCollidedWithTop)
                {
                    GameOver();
                }
            }
        }
        void ManagePipes()
        {
            if (!Pipes.Any())
            {
                Pipes.Add(new PipeModel());
            }

            if (Pipes.Last().DistanceFromLeft <=250)
            {
                Pipes.Add(new PipeModel());
            }

            if (Pipes.First().IsOffScreen())
            {
                Pipes.Remove(Pipes.First());
            }
        }

        void MoveObjects()
        {
            Bird.Fall(GRAVITY);
            Pipes.ForEach(pipe => pipe.Move());
        }


        void GameOver()
        {
            IsRunning = false;
        }
    }
}
