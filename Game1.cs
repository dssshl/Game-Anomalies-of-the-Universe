using Game_Anomalies_of_the_Universe.Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Texture2D monsterTexture;
        private const float monsterSpawn = 2f;
        private float monsterSpawnTimer = 0f;


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
            monsters = new List<Monster>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.GameMenu = Content.Load<Texture2D>("MenuGame");
            backgroundLevel1 = Content.Load<Texture2D>("Level1");
            gameOver = Content.Load<Texture2D>("end");
            player.LoadTexture(Content);
            monsterTexture = Content.Load<Texture2D>("monster1");
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
                    //появление монстров
                    monsterSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (monsterSpawnTimer >= monsterSpawn)
                    {
                        monsterSpawnTimer = 0f;
                        monster = new Monster(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 180 - monsterTexture.Height))
                        { monsterTexture = monsterTexture };
                        monsters.Add(monster);
                    }
                    UpdateMonster(gameTime);
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
                    {
                        monster.DrawTexture(spriteBatch);
                    }

                    player.DrawTexture(spriteBatch);

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

                if (monsters[i].monsterPosition.X + monsterTexture.Width < 0)
                    monsters.RemoveAt(i);
            }
        }
    }
}