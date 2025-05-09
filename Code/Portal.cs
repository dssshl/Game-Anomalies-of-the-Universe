using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Portal
    {
        public Texture2D portalTexture;
        public Vector2 portalPosition;
        public bool Active;
        public Rectangle Hitbox => new Rectangle((int)portalPosition.X, (int)portalPosition.Y, portalTexture.Width, portalTexture.Height);

        public Portal(Vector2 position)
        {
            portalPosition = position;
            Active = false;
        }

        public void LoadTexture(ContentManager content)
        {
            portalTexture = content.Load<Texture2D>("portal");
        }

        public void Update(GameTime gameTime)
        {
            if (!Active) return;
        }

        public void DrawTexture(SpriteBatch spriteBatch)
        {
            if (!Active) return;

            spriteBatch.Draw(portalTexture, portalPosition, Color.White);
        }
    }
}