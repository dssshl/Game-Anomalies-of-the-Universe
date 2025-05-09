using Game_Anomalies_of_the_Universe.Code.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game_Anomalies_of_the_Universe
{
    public class PlayerModel
    {
        // Позиция и движение персонажа
        public Vector2 Position { get; set; }
        public Vector2 Move { get; set; }
        public bool IsGround { get; set; }

        // Направление движения для спрайта
        public bool DirectionRight { get; set; } = true;

        // Здоровье персонажа
        public int Health { get; set; } = 3;
        public int MaxHealth { get; } = 3;
        public bool Alive => Health > 0;

        // Неуязвимость
        public float Invulnerability { get; set; }
        public const float InvulnerabilityTime = 1f;

        // Стрельба
        public List<BulletModel> Bullets { get; } = new List<BulletModel>();
        public float Shoot { get; set; }
        public const float ShootCount = 0.5f;

        // Экран
        public readonly int screenWidth;
        public readonly int screenHeight;
        public int Ground { get; }

        public Rectangle Hitbox => new Rectangle((int)Position.X + 10, (int)Position.Y + 5, 100, 70);

        public PlayerModel(int screenWidth, int screenHeight, int ground)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            Ground = ground;
            Position = new Vector2(100, 370);
        }

        public void TakeDamage(int damage)
        {
            if (Invulnerability <= 0)
            {
                Health = MathHelper.Clamp(Health - damage, 0, MaxHealth);
                Invulnerability = InvulnerabilityTime;
            }
        }

        public void RestoreHealth()
        {
            Health = MaxHealth;
        }
    }
}