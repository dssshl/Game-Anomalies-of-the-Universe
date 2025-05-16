using Game_Anomalies_of_the_Universe.Code;
using Game_Anomalies_of_the_Universe.Code.Controllers;
using Game_Anomalies_of_the_Universe.Code.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Game_Anomalies_of_the_Universe
{
    public enum State
    {
        Menu,
        Game,
        End,
        Win
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerController playerController;

        private Texture2D gameOver;
        private Texture2D winGame;

        private Monster monster;
        private List<Monster> monsters;
        private const float monsterSpawn = 1f;
        private float monsterSpawnTimer = 0f;

        private Boss boss;
        private Portal portal;
        private int monstersKilled = 0;
        private int win = 15;
        private Song backgroundMusic;
        private Texture2D[] backgrounds;
        private int currentLevel = 1;
        private const int maxLevels = 3;

        State _state = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1650;
            graphics.PreferredBackBufferHeight = 990;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            int ground = graphics.PreferredBackBufferHeight - 370;
            playerModel = new PlayerModel(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, ground);
            playerController = new PlayerController(playerModel);
            monsters = new List<Monster>();
            portal = new Portal(new Vector2(graphics.PreferredBackBufferWidth - 130, graphics.PreferredBackBufferHeight / 2 - 20));
            boss = new Boss(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight - 430), 0, graphics.PreferredBackBufferWidth);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.gameMenu = Content.Load<Texture2D>("MenuGame");

            backgroundMusic = Content.Load<Song>("BackgroundMusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;

            backgrounds = new Texture2D[maxLevels];
            for (int i = 0; i < maxLevels; i++)
                backgrounds[i] = Content.Load<Texture2D>($"Level{i + 1}");
            gameOver = Content.Load<Texture2D>("end");
            winGame = Content.Load<Texture2D>("Win");
            playerView = new PlayerView(Content);
            MonsterView.LoadContent(Content);
            boss.LoadTexture(Content);
            boss.monsterSpeed = 200f;

            portal.LoadTexture(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            switch (_state)
            {
                case State.Menu:
                    Menu.Update();
                    if (keyboardState.IsKeyDown(Keys.Space))
                        _state = State.Game;
                    break;

                case State.Game:
                    playerController.Update((float)gameTime.ElapsedGameTime.TotalSeconds, keyboardState, Mouse.GetState());
                    CheckBullet();

                    if (currentLevel == 3 && boss.Alive)
                    {
                        boss.Update(gameTime);
                        if (boss.Hitbox.Intersects(playerModel.Hitbox))
                            playerModel.TakeDamage(1);
                    }
                    else if (!portal.Active)
                        SpawnMonsters(gameTime);

                    if (portal.Active && playerModel.Hitbox.Intersects(portal.Hitbox))
                        NextLevel();
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
                    Menu.DrawTexture(spriteBatch);
                    break;

                case State.Game:
                    spriteBatch.Draw(backgrounds[currentLevel - 1], new Rectangle(0, 0, 1650, 990), Color.White);

                    if (currentLevel == 3 && boss.Alive)
                        boss.Draw(spriteBatch);
                    else
                    {
                        foreach (var monster in monsters)
                            monster.Draw(spriteBatch);
                    }

                    playerView.Draw(spriteBatch, playerModel);
                    portal.DrawTexture(spriteBatch);

                    if (!playerModel.Alive)
                        _state = State.End;
                    break;

                case State.End:
                    spriteBatch.Draw(gameOver, new Rectangle(0, 0, 1650, 990), Color.White);
                    break;

                case State.Win:
                    spriteBatch.Draw(winGame, new Rectangle(0, 0, 1650, 990), Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckBullet()
        {
            foreach (var bullet in playerModel.Bullets.ToArray())
            {
                foreach (var monster in monsters.ToArray())
                {
                    if (bullet.Hitbox.Intersects(monster.Hitbox))
                    {
                        bullet.OnScreen = false;
                        monsters.Remove(monster);
                        monstersKilled++;
                        break;
                    }
                }
                if (currentLevel == 3 && boss.Alive && bullet.Hitbox.Intersects(boss.Hitbox))
                {
                    bullet.OnScreen = false;
                    boss.TakeDamage(1);
                }
            }
            playerModel.Bullets.RemoveAll(b => !b.OnScreen);

            if (monstersKilled >= win && !portal.Active || !boss.Alive)
                portal.Active = true;
        }

        private void NextLevel()
        {
            if (currentLevel < maxLevels)
            {
                currentLevel++;
                monstersKilled = 0;
                portal.Active = false;
                win += 5;
                monsters.Clear();
                playerModel.RestoreHealth();
            }
            else 
                _state = State.Win;
        }

        private void SpawnMonsters(GameTime gameTime)
        {
            if (monstersKilled < win)
            {
                monsterSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (monsterSpawnTimer >= monsterSpawn)
                {
                    monsterSpawnTimer = 0;
                    monsters.Add(Monster.CreateMonster(currentLevel, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, new Random()));
                }

                for (int i = monsters.Count - 1; i >= 0; i--)
                {
                    monsters[i].Update(gameTime);
                    if (monsters[i].Hitbox.Intersects(playerModel.Hitbox))
                    {
                        playerModel.TakeDamage(1);
                        monsters.RemoveAt(i);
                    }
                    if (monsters[i].monsterPosition.X + monsters[i].monsterTexture.Width < 0)
                        monsters.RemoveAt(i);
                }
            }
        }
    }
}