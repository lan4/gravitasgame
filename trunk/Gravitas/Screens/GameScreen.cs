﻿using System;
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

        private const float GRAVITY_CONSTANT = 150.0f;

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

            // AddToManagers should be called LAST in this method:
            if (addToManagers)
            {
                AddToManagers();
            }
        }

        private void InitializeBodyList()
        {
            bodyList.Add(new Body("global", 20.0f, true, 10.0f, 3.0f));
            //bodyList.Add(new Body("global", 40.0f, true, -2.0f, -2.0f));
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
            player1.RotateToward(FindClosestBody(player1.Position));
            Gravitation();
            CheckCollisions();
        }

        private void Gravitation()
        {
            Vector3 F = new Vector3(0, 0, 0);
            foreach (Body element in bodyList)
            {
                Vector3 R = (element.Position - player1.Position);
                R.Normalize();

                float rSquared = Vector3.DistanceSquared(player1.Position, element.Position);

                F += Vector3.Multiply(R, (float)(GRAVITY_CONSTANT * element.Mass * player1.Mass / rSquared));
            }
            player1.Acceleration = Vector3.Divide(F, (float)player1.Mass);
            
        }

        private void CheckCollisions()
        {
            foreach (Body element in bodyList)
            {
                player1.Collision.CollideAgainstBounce(element.Collision, 0.0f, (float)element.Mass, 0.0f);
            }
        }

        public Body FindClosestBody(Vector3 position)
        {
            Body closest = bodyList.ElementAt<Body>(0);
            float dist_closest = Vector3.DistanceSquared(position, closest.Position);
            float dist_current;

            foreach (Body element in bodyList)
            {
                dist_current = Vector3.DistanceSquared(position, element.Position);
                if (dist_current < dist_closest)
                {
                    closest = element;
                    dist_closest = dist_current;
                }
            }

            return closest;
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
