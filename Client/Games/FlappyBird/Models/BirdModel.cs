using System;

namespace BlazorGames.Client.Games.FlappyBird.Models
{
    public class BirdModel
    {
        private const int MAX_HEIGHT = 500;
        public int DistanceFromGround { get; private set; } = 100;
        public int JumpStrength { get; private set; } = 50;

        public bool IsOnGround() => DistanceFromGround <= 0;

        public void Fall(int gravity)
        {
            DistanceFromGround -= Math.Min(gravity, DistanceFromGround);
        }

        public void Jump()
        {
            if (DistanceFromGround <= MAX_HEIGHT)
            {
                DistanceFromGround += JumpStrength;
            }
            
        }
    }
}
