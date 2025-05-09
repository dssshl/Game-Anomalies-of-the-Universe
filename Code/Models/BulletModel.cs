using Microsoft.Xna.Framework;

namespace Game_Anomalies_of_the_Universe.Code.Models
{
    public class BulletModel
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public bool OnScreen { get; set; } = true;
        public float Speed { get; } = 700f;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, 10, 5);

        public BulletModel(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        public void Update(float time)
        {
            Position += Direction * Speed * time;
            if (Position.X < 0 || Position.X > 1650  || Position.Y < 0 || Position.Y > 990)
                OnScreen = false;
        }
    }
}