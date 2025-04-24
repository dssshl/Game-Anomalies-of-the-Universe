using Game_Anomalies_of_the_Universe.Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        private Texture2D gameOver;

        private Monster monster;
        private List<Monster> monsters;
        private Texture2D monsterTexture1;
        private Texture2D monsterTexture2;
        private Texture2D flyingMonsterTexture;
        private const float monsterSpawn = 1.8f;
        private float monsterSpawnTimer = 0f;

        private Portal portal;
        private int monstersKilled = 0;
        private const int win = 10;

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
            player = new Player(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, ground) { playerPosition = new Vector2(100, 370) };
            monsters = new List<Monster>();
            portal = new Portal(new Vector2(graphics.PreferredBackBufferWidth - 130, graphics.PreferredBackBufferHeight / 2 - 70));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.GameMenu = Content.Load<Texture2D>("MenuGame");
            backgroundLevel1 = Content.Load<Texture2D>("Level1");
            gameOver = Content.Load<Texture2D>("end");
            player.LoadTexture(Content);
            monsterTexture1 = Content.Load<Texture2D>("monster1");
            monsterTexture2 = Content.Load<Texture2D>("monster2");
            flyingMonsterTexture = Content.Load<Texture2D>("FlyMonster");
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
                    {
                        _state = State.Game;
                        monsters.Clear();
                    }
                    break;
                case State.Game:
                    player.UpdatePlayer(gameTime, keyboardState);
                    CheckBullet();

                    if (!portal.Active)
                    {
                        if (monstersKilled < win)
                        {
                            monsterSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (monsterSpawnTimer >= monsterSpawn)
                            {
                                monsterSpawnTimer = 0;
                                float random = (float)new Random().NextDouble();

                                if (random < 0.4f)
                                {
                                    monsters.Add(new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 380), true)
                                    { monsterTexture = flyingMonsterTexture, monsterSpeed = 230f, amplitude = 60f, frequency = 1.5f });
                                }
                                else if (random < 0.8f)
                                {
                                    monsters.Add(new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 180 - monsterTexture2.Height), false)
                                    { monsterTexture = monsterTexture2, monsterSpeed = 250f });
                                }
                                monsters.Add(new Monster(
                                        new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 180 - monsterTexture1.Height), false)
                                { monsterTexture = monsterTexture1, monsterSpeed = 300f });
                            }
                            UpdateMonster(gameTime);
                        }
                        portal.Update(gameTime);
                    }
                    break;
                case State.End:
                    if (keyboardState.IsKeyDown(Keys.Space))
                        Menu.Update();
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

                    foreach (var monster in monsters)
                        monster.DrawTexture(spriteBatch);

                    player.DrawTexture(spriteBatch);

                    portal.Draw(spriteBatch);

                    if (!player.IsAlive)
                        _state = State.End;
                    break;
                case State.End:
                    spriteBatch.Draw(gameOver, new Rectangle(0, 0, 1280, 768), Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateMonster(GameTime gameTime)
        {

            for (int i = monsters.Count - 1; i >= 0; i--)
            {
                monsters[i].Update(gameTime);

                if (monsters[i].Hitbox.Intersects(player.Hitbox))
                {
                    player.TakeDamage(1);
                    monsters.RemoveAt(i);
                    continue;
                }

                if (monsters[i].monsterPosition.X + monsters[i].monsterTexture.Width < 0)
                    monsters.RemoveAt(i);
            }
        }

        private void CheckBullet()
        {
            foreach (var bullet in player.Bullets.ToArray())
            {
                foreach (var monster in monsters.ToArray())
                {
                    if (bullet.Hitbox.Intersects(monster.Hitbox))
                    {
                        bullet.InScreen = false;
                        monsters.Remove(monster);
                        monstersKilled++;

                        if (monstersKilled >= win && !portal.Active)
                            portal.Active = true;
                        break;
                    }
                }
            }
        }
    }
}