using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Monster
    {
        public Vector2 monsterPosition;
        public Texture2D monsterTexture;
        public float monsterSpeed;
        public bool flying;
        public float amplitude;
        public float frequency;
        private float count;

        public Rectangle Hitbox => new Rectangle( (int)monsterPosition.X + 5, (int)monsterPosition.Y + 5, monsterTexture.Width - 15, monsterTexture.Height - 10);

        public Monster(Vector2 startPos, bool fly)
        {
            monsterPosition = startPos;
            flying = fly;
        }

        public virtual void Update(GameTime gameTime)
        {
            float moveY = 0f;
            count += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flying)
                moveY = (float)Math.Sin(count * frequency) * amplitude * (float)gameTime.ElapsedGameTime.TotalSeconds;

            monsterPosition -= new Vector2(monsterSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, moveY);
        }

        public virtual void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(monsterTexture, monsterPosition, Color.White);
        }

    }
}
