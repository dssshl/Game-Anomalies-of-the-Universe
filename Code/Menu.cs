using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Anomalies_of_the_Universe.Code
{
    static class Menu
    {
        static public Texture2D GameMenu { get; set; }
        static int timeCount = 0;
        static Color color;
        static private bool animationDone = false;

        static public void Draw(SpriteBatch spriteBatch)
        {
            Color newColor = animationDone ? Color.White : Color.FromNonPremultiplied(255, 255, 255, timeCount % 256);
            spriteBatch.Draw(GameMenu, new Rectangle(0, 0, 1280, 768), newColor);
        }

        static public void Update()
        {
            if (!animationDone)
            {
                timeCount ++;

                if (timeCount >= 255)
                {
                    animationDone = true;
                    timeCount = 255;
                }
            }
        }
    }
}