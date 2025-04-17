using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Monster
    {
        public Texture2D monsterTexture { get; set; }
        public Vector2 monsterPosition { get; set; }
        public float monsterSpeed { get; set; } = 300f;

        public Rectangle Hitbox { get { return new Rectangle( (int)monsterPosition.X + 5, (int)monsterPosition.Y + 5,
            monsterTexture.Width - 15, monsterTexture.Height - 10); } }

        public Monster(Vector2 startPos)
        {
            monsterPosition = startPos;
        }

        public void Update(GameTime gameTime)
        {
            monsterPosition -= new Vector2( monsterSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
        }

        public void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(monsterTexture, monsterPosition, Color.White);
        }

    }
}
