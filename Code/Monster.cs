using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Monster
    {
        public Texture2D monsterTexture { get; set; }
        public Vector2 monsterPosition { get; set; }
        public float monsterSpeed { get; set; }


        public bool Flying { get; set; }
        public float amplitude { get; set; }
        public float frequency { get; set; }
        private float count;

        public Rectangle Hitbox { get { return new Rectangle( (int)monsterPosition.X + 5, (int)monsterPosition.Y + 5,
            monsterTexture.Width - 15, monsterTexture.Height - 10); } }

        public Monster(Vector2 startPos, bool isFlying)
        {
            monsterPosition = startPos;
            Flying = isFlying;
        }

        public void Update(GameTime gameTime)
        {
            float moveY = 0f;
            count += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Flying)
                moveY = (float)Math.Sin(count * frequency) * amplitude * (float)gameTime.ElapsedGameTime.TotalSeconds;

            monsterPosition -= new Vector2(monsterSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, moveY);
        }

        public void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(monsterTexture, monsterPosition, Color.White);
        }

    }
}
