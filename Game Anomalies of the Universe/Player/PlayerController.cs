using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Game_Anomalies_of_the_Universe.PlayerModel
{
    public class PlayerController : PlayerModel
    {
        public static bool IsAlive() => healthPoints > 0;

        public static Direction Direction { get; set; }
        public bool isStopped = false;

        public void Move(GameTime gameTime)
        {

            if (IsAlive())
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    //UpdatePlayerState();
                    personPosition.X -= playerSpeed;
                    Direction = Direction.Left;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    //UpdatePlayerState();
                    personPosition.X += playerSpeed;
                    Direction = Direction.Right;
                }
            }
            else
            {
                personPosition = Vector2.Zero;
            }
            stepsTime = 0;
        }

        //void UpdatePlayerState()
        //{
        //    if (stepsInstance.State == SoundState.Paused && !isStopped)
        //    {
        //        stepsInstance.Resume();
        //        isStopped = true;
        //    }
        //    PlayerView.currentTime += Globals.GameTime.ElapsedGameTime.Milliseconds;
        //    PlayerView.UpdateFrame(Globals.GameTime);
        //}
    }

    public class Direction
    {
        public Direction Left { get; internal set; }
        public Direction Right { get; internal set; }
        public Direction None { get; internal set; }
    }
}
