using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Anomalies_of_the_Universe.Code.Models
{
    public static class MonsterTextures
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
    }
}
