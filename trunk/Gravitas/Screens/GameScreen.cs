using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FlatRedBall;

using Microsoft.Xna.Framework;

namespace Gravitas.Screens
{
    public class GameScreen : Screen
    {
        private List<Body> bodyList;
        private Player player1;

        #region Methods

        #region Constructor and Initialize

        public GameScreen()
            : base("GameScreen")
        {
            // Don't put initialization code here, do it in
            // the Initialize method below
            //   |   |   |   |   |   |   |   |   |   |   |
            //   |   |   |   |   |   |   |   |   |   |   |
            //   V   V   V   V   V   V   V   V   V   V   V

        }

        public override void Initialize(bool addToManagers)
        {
            // Set the screen up here instead of in the Constructor to avoid
            // exceptions occurring during the constructor.

            bodyList = new List<Body>();
            player1 = new Player("global", 10.0f);

            InitializeBodyList();

            bodyList.ElementAt<Body>(0).Position = new Vector3(5.0f, 3.0f, 0.0f);


            // AddToManagers should be called LAST in this method:
            if (addToManagers)
            {
                AddToManagers();
            }
        }

        private void InitializeBodyList()
        {
            bodyList.Add(new Body("global", 20.0f, true));
            bodyList.Add(new Body("global", 40.0f, true));
        }

        public override void AddToManagers()
        {


        }

        #endregion

        #region Public Methods

        public override void Activity(bool firstTimeCalled)
        {
            base.Activity(firstTimeCalled);

            player1.Activity();
            Gravitation();
        }

        private void Gravitation()
        {
            foreach (Body element in bodyList)
            {
                player1.Acceleration = new Vector3(player1.Acceleration.X + (element.Position.X - player1.Position.X) * 5.0f,
                                                   player1.Acceleration.Y + (element.Position.Y - player1.Position.Y) * 5.0f,
                                                   0.0f);
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            bodyList.Clear();


        }

        #endregion


        #endregion
    }
}
