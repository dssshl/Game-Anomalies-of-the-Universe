using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Bullet
    {
        public Texture2D bulletTexture { get; set; }
        public Vector2 bulletPosition { get; set; }
        public Vector2 bulletMove { get; set; }
        public float bulletSpeed { get; set; } = 700f;

        public bool InScreen { get; set; } = true;


        public Rectangle Hitbox => new Rectangle((int)bulletPosition.X, (int)bulletPosition.Y, bulletTexture.Width, bulletTexture.Height);
        public Bullet(Texture2D texture, Vector2 startPosition, Vector2 move)
        {
            bulletTexture = texture;
            bulletPosition = startPosition;
            bulletMove = move;
        }
        public void Update(GameTime gameTime)
        {
            bulletPosition += bulletMove * bulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (bulletPosition.X < 0  ||  bulletPosition.X > 1280 || bulletPosition.Y < 0 || bulletPosition.Y > 768)
                InScreen = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, bulletPosition, Color.White);
        }
    }
}
