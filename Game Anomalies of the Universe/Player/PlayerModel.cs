using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Game_Anomalies_of_the_Universe.PlayerModel
{
    public class PlayerModel
    {
        public static int playerSpeed = 5;
        public static int healthPoints = 100;

        public static Rectangle playerHitBox = new Rectangle();

        public static Vector2 personPosition = new Vector2();
        public static Vector2 PersonPosition
        {
            get
            {
                return new Vector2(personPosition.X + 30, personPosition.Y + 130);
            }
            set
            {
                personPosition.X = value.X;
                personPosition.Y = value.Y;
            }
        }

        public static double stepsTime = 0;

        PlayerController playerController;

        public PlayerModel()
        {
            playerController = new PlayerController();
        }


        public void Moving(GameTime gameTime)
        {
            playerController.Move(gameTime);
        }
    }
}