using Game_Anomalies_of_the_Universe.Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Anomalies_of_the_Universe
{
    public enum State
    {
        Menu,
        Game,
        End
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        private Texture2D backgroundLevel1;

        State _state = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            int ground = graphics.PreferredBackBufferHeight - 180;
            player = new Player(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, ground) { playerPosition = new Vector2 (100, 370) };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.GameMenu = Content.Load<Texture2D>("MenuGame");
            backgroundLevel1 = Content.Load<Texture2D>("Level1");
            player.LoadTexture(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            switch (_state) 
            { 
                case State.Menu:
                    Menu.Update();
                    if (keyboardState.IsKeyDown(Keys.Space)) _state = State.Game;
                    break;
                case State.Game:
                    player.UpdatePlayer(gameTime, keyboardState);
                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            
            switch (_state)
            {
                case State.Menu:
                    Menu.Draw(spriteBatch);
                    break;
                case State.Game:
                    spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 1280, 768), Color.White);
                    player.DrawTexture(spriteBatch);
                    break;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}