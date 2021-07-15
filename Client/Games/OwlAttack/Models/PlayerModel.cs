using System.ComponentModel.Design;

namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class PlayerModel
    {
        public const int MaxVelocity = 3;

        public int Velocity { get; set; } = 2;

        public int DistanceFromLeft { get; set; } = 250;
        public int Altitude { get; set; } = -400;

        public bool CanMove { get; set; } = true;

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
        }

    }
}
