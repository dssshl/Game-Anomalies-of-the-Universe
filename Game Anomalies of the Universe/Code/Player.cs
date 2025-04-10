using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Player
    {
        public Texture2D playerTexture { get; set; }
        public Texture2D runRightTexture { get; set; }
        public Texture2D runLeftTexture { get; set; }

        // Позиция и движение персонажа
        public Vector2 playerPosition { get; set; }
        public Vector2 playerMove { get; set; }

        // Параметры движения
        public float playerSpeed { get; set; } = 500f;
        public float jump { get; set; } = -500f;
        public float gravity { get; set; } = 1000f;
        public bool isGrounded { get; set; }

        // Направление движения для спрайта
        public bool direction { get; set; } = true;

        //размеры экрана и земли
        private int screenWidth;
        private int screenHeight;
        private int ground;

        public Player(int screenWidth, int screenHeight, int groundLevel)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.ground = groundLevel;
        }

        public void LoadTexture(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("Player");
            runRightTexture = content.Load<Texture2D>("runRight");
            runLeftTexture = content.Load<Texture2D>("runLeft");
        }

        public void UpdatePlayer(GameTime gameTime, KeyboardState keyboardState)
        {
            float moveX = 0;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveX -= 1;
                direction = false;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveX += 1;
                direction = true;
            }

            playerMove = new Vector2(moveX * playerSpeed, playerMove.Y);

            if (keyboardState.IsKeyDown(Keys.Space) && isGrounded)
            {
                playerMove = new Vector2(playerMove.X, jump);
                isGrounded = false;
            }

            playerMove = new Vector2( playerMove.X, playerMove.Y + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            playerPosition += playerMove * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (playerPosition.Y >= ground - playerTexture.Height)
            {
                playerPosition = new Vector2(playerPosition.X, ground - playerTexture.Height);
                playerMove = new Vector2(playerMove.X, 0);
                isGrounded = true;
            }

            playerPosition = new Vector2( MathHelper.Clamp(playerPosition.X, 0, screenWidth - playerTexture.Width), playerPosition.Y);

            if (Math.Abs(playerMove.X) > 0.1f)
            {
                playerTexture = direction ? runRightTexture : runLeftTexture;
            }
        }

        public void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, playerPosition, Color.White);
        }
    }
}