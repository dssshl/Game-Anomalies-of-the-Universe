using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Monster : MonsterCreater
    {
        public Vector2 monsterPosition;
        public Texture2D monsterTexture;
        public float monsterSpeed;
        public bool flying;
        public float amplitude;
        public float frequency;
        private float time;

        public Rectangle Hitbox => new Rectangle((int)monsterPosition.X, (int)monsterPosition.Y, monsterTexture.Width - 15, monsterTexture.Height - 10);

        public Monster(Vector2 startPos, bool fly)
        {
            monsterPosition = startPos;
            flying = fly;
        }

        public virtual void Update(GameTime gameTime)
        {
            float moveY = 0f;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flying)
                moveY = (float)Math.Sin(time * frequency) * amplitude * (float)gameTime.ElapsedGameTime.TotalSeconds;

            monsterPosition -= new Vector2(monsterSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, moveY);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            MonsterView.Draw(spriteBatch, monsterTexture, monsterPosition);
        }
    }
}
