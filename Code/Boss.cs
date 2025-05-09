using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Boss : Monster
    {
        public bool moveRight;
        private int left;
        private int right;
        private SpriteEffects turn = SpriteEffects.None;
        public int health = 20;
        public bool Alive => health > 0;

        public Boss(Vector2 startPos, int left, int right) : base(startPos, false)
        {
            this.left = left;
            this.right = right;
            moveRight = false;
        }

        public void LoadTexture(ContentManager content)
        {
            monsterTexture = content.Load<Texture2D>("boss");
        }

        public override void Update(GameTime gameTime)
        {
            float moveX = moveRight ? monsterSpeed : -monsterSpeed;

            monsterPosition.X += moveX * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (monsterPosition.X <= left)
            {
                monsterPosition.X = left;
                moveRight = true;
                turn = SpriteEffects.FlipHorizontally;
            }
            else if (monsterPosition.X + monsterTexture.Width >= right)
            {
                monsterPosition.X = right - monsterTexture.Width;
                moveRight = false;
                turn = SpriteEffects.None;
            }
            if (!Alive) return;
        }

        public override void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(monsterTexture, monsterPosition, null, Color.White, 0f, Vector2.Zero, 1f, turn, 0f);
            if (!Alive) return;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0)
                health = 0;
        }
    }
}
