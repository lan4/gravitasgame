using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FlatRedBall;
using FlatRedBall.Math.Geometry;

using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Controllers;


namespace GravitasN.Screens
{
    public class GameScreen : Screen
    {

        public static PhysicsSimulator PhysicsSim;
        public static GravityController GravityControl;

        private Player mPlayer;
        public static List<Planet> PlanetList;

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

            float gravity = 0;
            PhysicsSim = new PhysicsSimulator(new Microsoft.Xna.Framework.Vector2(0, gravity));

            mPlayer = new Player("global", 1.0f);

            PlanetList = new List<Planet>();

            //List<Body> pointGravList = new List<Body>();

            //pointGravList.Add(mPlanet.Body);

            GravityControl = ComplexFactory.Instance.CreateGravityController(PhysicsSim, new List<Body>(), 50.0f, 15.0f);
            GravityControl.PointList = new List<Vector2>();
            //GravityControl.PointList.Add(mPlanet.Body.Position);

            PhysicsSim.ControllerList.Add(GravityControl);

            PlanetList.Add(new Planet("global", 1.0f, 5.0f, 5.0f, 4.0f, GravityControl));
            PlanetList.Add(new Planet("global", 2.0f, -20.0f, -20.0f, 2.0f, GravityControl));

            // AddToManagers should be called LAST in this method:
            if (addToManagers)
            {
                AddToManagers();
            }
        }

        public override void AddToManagers()
        {


        }

        #endregion

        #region Public Methods

        public override void Activity(bool firstTimeCalled)
        {
            PhysicsSim.Update(TimeManager.SecondDifference);

            mPlayer.Activity();

            foreach (Planet aPlanet in PlanetList)
            {
                aPlanet.Activity();
            }

            base.Activity(firstTimeCalled);
        }

        public override void Destroy()
        {
            base.Destroy();




        }

        #endregion


        #endregion
    }
}
