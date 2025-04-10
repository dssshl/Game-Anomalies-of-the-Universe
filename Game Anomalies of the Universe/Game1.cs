using Game_Anomalies_of_the_Universe.Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        private Texture2D backgroundLevel1;

        // Текстура персонажа 
        private Texture2D playerTexture;
        private Texture2D runRightTexture;
        private Texture2D runLeftTexture;

        // Позиция и движение персонажа
        private Vector2 playerPosition;
        private Vector2 playerMove;

        // Параметры движения
        private float playerSpeed = 500f;
        private float jumpForce = -500f;
        private float gravity = 1000f;
        private bool isGround = false;

        //направление движения для спрайта
        private bool direction = true;

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

            playerPosition = new Vector2(100, 370);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.GameMenu = Content.Load<Texture2D>("MenuGame");
            backgroundLevel1 = Content.Load<Texture2D>("Level1");
            playerTexture = Content.Load<Texture2D>("Player");
            runRightTexture = Content.Load<Texture2D>("runRight");
            runLeftTexture = Content.Load<Texture2D>("runLeft");
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            float move = 0;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                move -= 1;
                direction = false;
            }
            if (keyboardState.IsKeyDown(Keys.D)) 
            {
                move += 1;
                direction = true;
            }

            playerMove.X = move * playerSpeed;

            if (keyboardState.IsKeyDown(Keys.Space) && isGround)
            {
                playerMove.Y = jumpForce;
                isGround = false;
            }

            playerMove.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            playerPosition += playerMove * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float ground = graphics.PreferredBackBufferHeight - 180;
            if (playerPosition.Y >= ground - playerTexture.Height)
            {
                playerPosition.Y = ground - playerTexture.Height;
                playerMove.Y = 0;
                isGround = true;
            }

            playerPosition.X = MathHelper.Clamp(playerPosition.X, 0,
                graphics.PreferredBackBufferWidth - playerTexture.Width);

            if (Math.Abs(playerMove.X) > 0.1f)
            {
                playerTexture = direction ? runRightTexture : runLeftTexture;
            }

            switch (_state) 
            { 
                case State.Menu:
                    Menu.Update();
                    if (keyboardState.IsKeyDown(Keys.Space)) _state = State.Game;
                    break;
                case State.Game:
                    if (keyboardState.IsKeyDown(Keys.Escape)) _state = State.Menu;
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
                    spriteBatch.Draw(playerTexture, playerPosition, Color.White);
                    break;

            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}