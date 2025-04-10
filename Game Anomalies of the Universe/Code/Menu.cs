using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Anomalies_of_the_Universe.Code
{
    static class Menu
    {
        static public Texture2D GameMenu { get; set; }
        static int timeCount = 0;
        static Color color;

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameMenu, new Rectangle(0, 0, 1280, 768), color);
        }

        static public void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCount % 255);
            timeCount++;
        }
    }
}