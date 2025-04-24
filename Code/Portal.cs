using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Portal
    {
        public Texture2D PortalTexture { get; set; }
        public Vector2 PortalPosition { get; set; }
        public bool Active { get; set; }

        public Rectangle Hitbox
                    { get { return new Rectangle((int)PortalPosition.X, (int)PortalPosition.Y, PortalTexture.Width, PortalTexture.Height); } }

        public Portal(Vector2 position)
        {
            PortalPosition = position;
            Active = false;
        }

        public void LoadTexture(ContentManager content)
        {
            PortalTexture = content.Load<Texture2D>("portal");
        }

        public void Update(GameTime gameTime)
        {
            if (!Active) return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Active) return;

            spriteBatch.Draw(PortalTexture, PortalPosition, Color.White);
        }
    }
}