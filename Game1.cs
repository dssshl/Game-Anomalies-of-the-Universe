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
        End,
        Win
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private Texture2D gameOver;
        private Texture2D winGame;

        private Monster monster;
        private List<Monster> monsters;
        private Texture2D monsterTexture1Level1;
        private Texture2D monsterTexture2Level1;
        private Texture2D flyingMonsterTextureLevel1;
        private Texture2D monsterTexture1Level2;
        private Texture2D monsterTexture2Level2;
        private Texture2D flyingMonsterTextureLevel2;
        private const float monsterSpawn = 1.8f;
        private float monsterSpawnTimer = 0f;

        private Portal portal;
        private int monstersKilled = 0;
        private int win = 1;

        private Texture2D[] backgrounds;
        private int currentLevel = 1;
        private const int maxLevels = 3;

        private Boss boss;

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

            int ground = graphics.PreferredBackBufferHeight - 230;
            player = new Player(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, ground) { playerPosition = new Vector2(100, 370) };
            monsters = new List<Monster>();
            portal = new Portal(new Vector2(graphics.PreferredBackBufferWidth - 130, graphics.PreferredBackBufferHeight / 2 - 20));
            boss = new Boss(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight - 430), 0, graphics.PreferredBackBufferWidth);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.gameMenu = Content.Load<Texture2D>("MenuGame");

            backgrounds = new Texture2D[maxLevels];
            backgrounds[0] = Content.Load<Texture2D>("Level1");
            backgrounds[1] = Content.Load<Texture2D>("Level2");
            backgrounds[2] = Content.Load<Texture2D>("Level3");

            gameOver = Content.Load<Texture2D>("end");
            winGame = Content.Load<Texture2D>("Win");
            player.LoadTexture(Content);

            monsterTexture1Level1 = Content.Load<Texture2D>("monster1Level1");
            monsterTexture2Level1 = Content.Load<Texture2D>("monster2Level1");
            flyingMonsterTextureLevel1 = Content.Load<Texture2D>("FlyMonster1");
            monsterTexture1Level2 = Content.Load<Texture2D>("monster1Level2");
            monsterTexture2Level2 = Content.Load<Texture2D>("monster2Level2");
            flyingMonsterTextureLevel2 = Content.Load<Texture2D>("FlyMonster2");

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
                    {
                        _state = State.Game;
                        monsters.Clear();
                    }
                    break;

                case State.Game:
                    player.Update(gameTime, keyboardState);
                    CheckBullet();

                    if (currentLevel == 3)
                    {
                        if (boss.Alive)
                        {
                            boss.Update(gameTime);

                            if (boss.Hitbox.Intersects(player.Hitbox))
                                player.TakeDamage(1);

                            foreach (var bullet in player.Bullets.ToArray())
                            {
                                if (bullet.Hitbox.Intersects(boss.Hitbox))
                                {
                                    boss.TakeDamage(1);
                                    bullet.onScreen = false;
                                }
                            }
                        }
                        if (!boss.Alive)
                            portal.Active = true;
                    }
                    else if (!portal.Active)
                        SpawnMonsters(gameTime);

                    if (portal.Active && player.Hitbox.Intersects(portal.Hitbox))
                        NextLevel();
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
                    Menu.DrawTexture(spriteBatch);
                    break;

                case State.Game:
                    spriteBatch.Draw(backgrounds[currentLevel - 1], new Rectangle(0, 0, 1650, 990), Color.White);

                    if (currentLevel == 3 && boss.Alive)
                        boss.DrawTexture(spriteBatch);
                    else
                    {
                        foreach (var monster in monsters)
                            monster.DrawTexture(spriteBatch);
                    }

                    player.DrawTexture(spriteBatch);
                    portal.DrawTexture(spriteBatch);

                    if (!player.Alive)
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
                        bullet.onScreen = false;
                        monsters.Remove(monster);
                        monstersKilled++;

                        if (monstersKilled >= win && !portal.Active)
                            portal.Active = true;
                        break;
                    }
                }
            }
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
                player.RestoreHealth();
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
                    float random = (float)new Random().NextDouble();

                    if (currentLevel == 1)
                    {
                        if (random < 0.4f)
                        {
                            monsters.Add(new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 430), true)
                            { monsterTexture = flyingMonsterTextureLevel1, monsterSpeed = 250f, amplitude = 60f, frequency = 1.5f });
                        }
                        else if (random < 0.8f)
                        {
                            monsters.Add(new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 240 - monsterTexture2Level1.Height), false)
                            { monsterTexture = monsterTexture2Level1, monsterSpeed = 270f });
                        }
                        monsters.Add(new Monster(
                                new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 240 - monsterTexture1Level1.Height), false)
                        { monsterTexture = monsterTexture1Level1, monsterSpeed = 320f });
                    }

                    else if (currentLevel == 2)
                    {
                        if (random < 0.5f)
                        {
                            monsters.Add(new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 430), true)
                            { monsterTexture = flyingMonsterTextureLevel2, monsterSpeed = 270f, amplitude = 60f, frequency = 1.5f });
                        }
                        else if (random < 0.9f)
                        {
                            monsters.Add(new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 230 - monsterTexture2Level2.Height), false)
                            { monsterTexture = monsterTexture2Level2, monsterSpeed = 290f });
                        }
                        monsters.Add(new Monster(
                                new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 230 - monsterTexture1Level2.Height), false)
                        { monsterTexture = monsterTexture1Level2, monsterSpeed = 340f });
                    }
                    else if (currentLevel == 3)
                        boss.LoadTexture(Content);
                }
                UpdateMonster(gameTime);
            }
            portal.Update(gameTime);
        }
    }
}