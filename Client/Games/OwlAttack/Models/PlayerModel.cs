namespace BlazorGames.Client.Games.OwlAttack.Models
{
    public class PlayerModel
    {
        public const int MaxVelocity = 2;

        public int Velocity { get; set; } = 2;

        public int DistanceFromLeft { get; private set; } = 250;

        public void Move()
        {
            
            DistanceFromLeft += Velocity;
        }

    }
}
