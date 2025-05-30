﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game_Anomalies_of_the_Universe.Code
{
    public class Player
    {
        public Texture2D playerTexture;
        public Texture2D runRightTexture;
        public Texture2D runLeftTexture;

        // Позиция и движение персонажа
        public Vector2 playerPosition;
        public Vector2 playerMove;

        // Параметры движения
        public float playerSpeed = 500f;
        public float jump = -700f;
        public float gravity = 1000f;
        public bool isGrounded;

        // Направление движения для спрайта
        public bool direction = true;

        //размеры экрана и земли
        private int screenWidth;
        private int screenHeight;
        private int ground;

        // Здоровье персонажа
        private int health = 3;
        public int MaxHealth { get; } = 3;
        public bool Alive => health > 0;

        //отображение здоровья в виде сердечек
        private Texture2D heartFull;
        private Texture2D heartEmpty;
        private Vector2 heartPosition = new Vector2(50, 50);

        public Rectangle Hitbox => new Rectangle((int)playerPosition.X + 10, (int)playerPosition.Y + 5, playerTexture.Width - 20, playerTexture.Height - 10);

        //стрельба
        private List<Bullet> bullets = new List<Bullet>();
        private Texture2D bulletTexture;
        private float shoot = 0f;
        private const float shootCount = 0.5f;
        public List<Bullet> Bullets => bullets;

        //неуязвимость
        private float invulnerability = 0f;
        private const float invulnerabilityTime = 1f;

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
            heartFull = content.Load<Texture2D>("heartFull");
            heartEmpty = content.Load<Texture2D>("heartEmpty");
            bulletTexture = content.Load<Texture2D>("bullet");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
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

            playerMove = new Vector2(playerMove.X, playerMove.Y + gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            playerPosition += playerMove * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (playerPosition.Y >= ground - playerTexture.Height)
            {
                playerPosition = new Vector2(playerPosition.X, ground - playerTexture.Height);
                playerMove = new Vector2(playerMove.X, 0);
                isGrounded = true;
            }

            playerPosition = new Vector2(MathHelper.Clamp(playerPosition.X, 0, screenWidth - playerTexture.Width), playerPosition.Y);

            if (Math.Abs(playerMove.X) > 0.1f)
                playerTexture = direction ? runRightTexture : runLeftTexture;

            if (shoot > 0)
                shoot -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && shoot <= 0)
            {
                Vector2 bulletPosition = new Vector2(playerPosition.X + playerTexture.Width / 2 - bulletTexture.Width / 2, playerPosition.Y + playerTexture.Height / 2 - bulletTexture.Height / 2);
                Vector2 directionBullet = direction ? Vector2.UnitX : -Vector2.UnitX;

                bullets.Add(new Bullet(bulletTexture, bulletPosition, directionBullet));

                shoot = shootCount;
            }

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);

                if (!bullets[i].onScreen)
                    bullets.RemoveAt(i);
            }

            if (invulnerability > 0)
                invulnerability -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void DrawTexture(SpriteBatch spriteBatch)
        {
            Color color = invulnerability > 0 ? Color.Red : Color.White;
            spriteBatch.Draw(playerTexture, playerPosition, color);
            DrawHealth(spriteBatch);

            foreach (var bullet in bullets)
                bullet.DrawTexture(spriteBatch);
        }

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            const int space = 50;

            for (int i = 0; i < MaxHealth; i++)
            {
                Texture2D heartTexture = (i < health) ? heartFull : heartEmpty;
                spriteBatch.Draw(heartTexture, heartPosition + new Vector2(i * space, 0), Color.White);
            }

        }

        public void TakeDamage(int damage)
        {
            if (invulnerability <= 0)
            {
                health = MathHelper.Clamp(health - damage, 0, MaxHealth);
                invulnerability = invulnerabilityTime;
            }
        }

        public void RestoreHealth()
        {
            health = MaxHealth;
        }
    }
}