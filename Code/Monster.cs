using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Content;
using Game_Anomalies_of_the_Universe.Code.Models;

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
        private float time;

        public Rectangle Hitbox => new Rectangle((int)monsterPosition.X + 5, (int)monsterPosition.Y + 5, monsterTexture.Width - 15, monsterTexture.Height - 10);

        public Monster(Vector2 startPos, bool fly)
        {
            monsterPosition = startPos;
            flying = fly;
        }

        public static void LoadContent(ContentManager content)
        {
            MonsterTextures.LoadContent(content);
        }

        public virtual void Update(GameTime gameTime)
        {
            float moveY = 0f;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flying)
                moveY = (float)Math.Sin(time * frequency) * amplitude * (float)gameTime.ElapsedGameTime.TotalSeconds;

            monsterPosition -= new Vector2(monsterSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, moveY);
        }

        public virtual void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(monsterTexture, monsterPosition, Color.White);
        }

        public static Monster CreateMonster(int level, int screenWidth, int screenHeight, Random random)
        {
            Monster monster;

            if (level == 1)
            {
                if (random.NextDouble() < 0.3)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 430), true)
                    { monsterTexture = MonsterTextures.FlyingMonsterLevel1, monsterSpeed = 300f, amplitude = 60f, frequency = 1.5f };
                }
                else if (random.NextDouble() < 0.5)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 440), false)
                    { monsterTexture = MonsterTextures.Monster2Level1, monsterSpeed = 330f };
                }
                else
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 440), false)
                    { monsterTexture = MonsterTextures.Monster1Level1, monsterSpeed = 360f };
                }
            }
            else 
            {
                if (random.NextDouble() < 0.4)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 430), true)
                    { monsterTexture = MonsterTextures.FlyingMonsterLevel2, monsterSpeed = 330f, amplitude = 70f, frequency = 1.8f };
                }
                else if (random.NextDouble() < 0.6)
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 530), false)
                    { monsterTexture = MonsterTextures.Monster2Level2, monsterSpeed = 360f };
                }
                else
                {
                    monster = new Monster(new Vector2(screenWidth, screenHeight - 390), false)
                    { monsterTexture = MonsterTextures.Monster1Level2, monsterSpeed = 390f };
                }
            }
            return monster;
        }

    }
}
