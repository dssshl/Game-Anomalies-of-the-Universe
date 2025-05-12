using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game_Anomalies_of_the_Universe.Code.Views
{
    public class PlayerView
    {
        public Texture2D playerTexture;
        private Texture2D runRightTexture;
        private Texture2D runLeftTexture;
        private Texture2D heartFull;
        private Texture2D heartEmpty;
        public Texture2D bulletTexture;

        public PlayerView(ContentManager content)
        {
            LoadContent(content);
        }

        private void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("Player");
            runRightTexture = content.Load<Texture2D>("runRight");
            runLeftTexture = content.Load<Texture2D>("runLeft");
            heartFull = content.Load<Texture2D>("heartFull");
            heartEmpty = content.Load<Texture2D>("heartEmpty");
            bulletTexture = content.Load<Texture2D>("bullet");
        }

        public void Draw(SpriteBatch spriteBatch, PlayerModel player)
        {
            Texture2D currentTexture = playerTexture;
            if (Math.Abs(player.Move.X) > 0.1f)
                currentTexture = player.DirectionRight ? runRightTexture : runLeftTexture;

            Color color = player.Invulnerability > 0 ? Color.Red : Color.White;
            spriteBatch.Draw(currentTexture, player.Position, color);
            DrawHealth(spriteBatch, player);

            foreach (var bullet in player.Bullets)
                spriteBatch.Draw(bulletTexture, bullet.Position, Color.White);
        }

        private void DrawHealth(SpriteBatch spriteBatch, PlayerModel player)
        {
            const int space = 50;
            Vector2 heartPosition = new Vector2(100, 100);

            for (int i = 0; i < player.MaxHealth; i++)
            {
                Texture2D heartTexture = (i < player.Health) ? heartFull : heartEmpty;
                spriteBatch.Draw(heartTexture, heartPosition + new Vector2(i * space, 0), Color.White);
            }
        }
    }
}