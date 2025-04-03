using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Anomalies_of_the_Universe
{
    static class HomeScreen
    {
        static public Texture2D homeScreen {  get; set; }
        static int timeCount = 0;
        static Color color;

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(homeScreen, new Rectangle(0, 0, 1280, 768), color);
        }

        static public void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCount % 256);
            timeCount++;
        }
    }
}
