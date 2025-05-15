using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Anomalies_of_the_Universe.Code
{
    public static class MonsterView
    {
        public static Texture2D Monster1Level1 { get; private set; }
        public static Texture2D Monster2Level1 { get; private set; }
        public static Texture2D FlyingMonsterLevel1 { get; private set; }
        public static Texture2D Monster1Level2 { get; private set; }
        public static Texture2D Monster2Level2 { get; private set; }
        public static Texture2D FlyingMonsterLevel2 { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Monster1Level1 = content.Load<Texture2D>("monster1Level1");
            Monster2Level1 = content.Load<Texture2D>("monster2Level1");
            FlyingMonsterLevel1 = content.Load<Texture2D>("FlyMonster1");
            Monster1Level2 = content.Load<Texture2D>("monster1Level2");
            Monster2Level2 = content.Load<Texture2D>("monster2Level2");
            FlyingMonsterLevel2 = content.Load<Texture2D>("FlyMonster2");
        }

        public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
