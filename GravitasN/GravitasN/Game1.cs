using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FlatRedBall;
using FlatRedBall.Math.Geometry;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Controllers;

namespace GravitasN
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Test Farseer Objects
        public static PhysicsSimulator PhysicsSim;
        public static GravityController GravityControl;

        Planet mPlanet;
        Player mPlayer;
        List<Planet> mPlanetList;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            FlatRedBallServices.InitializeFlatRedBall(this, graphics);

            // Uncomment the following line and add your Screen's fully qualified name
            // if using Screens in your project.  If not, or if you don't know what it means,
            // just ignore the following line for now.
            // For more information on Screens see the Screens wiki article on FlatRedBall.com.
            //Screens.ScreenManager.Start(typeof(GravitasN.Screens.TestScreen).FullName);


            // This is the same as setting FlatRedBall acceleration values

            //Initializing Physics Simulator
            float gravity = 0;
            PhysicsSim = new PhysicsSimulator(new Microsoft.Xna.Framework.Vector2(0, gravity));
            
            mPlayer = new Player("global", 1.0f);

            mPlanetList = new List<Planet>();

            mPlanet = new Planet("global", 1.0f, 5.0f, 5.0f, 4.0f);
            List<Body> pointGravList = new List<Body>();

            pointGravList.Add(mPlanet.Body);

            GravityControl = ComplexFactory.Instance.CreateGravityController(PhysicsSim, pointGravList, 50.0f, 15.0f);
            GravityControl.PointList = new List<Vector2>();
            GravityControl.PointList.Add(mPlanet.Body.Position);

            PhysicsSim.ControllerList.Add(GravityControl);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            FlatRedBallServices.Update(gameTime);
            Screens.ScreenManager.Activity();
            
            //Farseer test code
            PhysicsSim.Update(TimeManager.SecondDifference);
            //mBallSprite.X = mBallBody.Position.X;
            //mBallSprite.Y = mBallBody.Position.Y;

            mPlayer.Activity();

            mPlanet.Activity();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FlatRedBallServices.Draw();
            base.Draw(gameTime);
        }
    }
}
