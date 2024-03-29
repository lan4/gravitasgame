﻿using System;
using System.Collections.Generic;
using System.Text;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Input;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;

using Microsoft.Xna;
using Microsoft.Xna.Framework;


// Be sure to replace:
// 1.  The namespace
// 2.  The class name
// 3.  The constructor (should be the same as the class name)


namespace Gravitas
{
    public class Planet : PositionedObject
    {
        #region Fields

        private Body mBody;
        private Geom mGeom;

        private Sprite mVisibleRepresentation;
        private Circle mCollision;

        public Circle Collision
        {
            get { return mCollision; }
        }

        private bool mIsStatic;

        public bool IsStatic
        {
            get { return mIsStatic; }
        }

        private double mMass;

        public double Mass
        {
            get { return mMass; }
        }

        // Keep the ContentManager for easy access:
        string mContentManagerName;

        #endregion

        #region Properties


        // Here you'd define properties for things
        // you want to give other Entities and game code
        // access to, like your Collision property:
        //public Circle Collision
        //{
        //    get { return mCollision; }
        //}

        #endregion

        #region Methods

        // Constructor
        public Planet(string contentManagerName, double mass, bool isStatic, float x, float y)
        {
            // Set the ContentManagerName and call Initialize:
            mContentManagerName = contentManagerName;
            mMass = mass;
            mIsStatic = isStatic;
            this.Position = new Vector3(x, y, 0.0f);

            // If you don't want to add to managers, make an overriding constructor
            Initialize(true);
        }

        protected virtual void Initialize(bool addToManagers)
        {
            // Here you can preload any content you will be using
            // like .scnx files or texture files.

            //Initializes the body at the point given in the constructor.
            mBody = BodyFactory.Instance.CreateBody(Screens.GameScreen.PhysicsSim, 1, 1000);
            mBody.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y);
            mBody.Rotation = 0.1f;
            mBody.IsStatic = true;
            mGeom = GeomFactory.Instance.CreateCircleGeom(mBody, 40, 360);
            mGeom.RestitutionCoefficient = 1;
            Screens.GameScreen.PhysicsSim.Add(mBody);

            if (addToManagers)
            {
                AddToManagers(null);
            }
        }

        public virtual void AddToManagers(Layer layerToAddTo)
        {
            // Add the Entity to the SpriteManager
            // so it gets managed properly (velocity, acceleration, attachments, etc.)
            SpriteManager.AddPositionedObject(this);

            // Here you may want to add your objects to the engine.  Use layerToAddTo
            // when adding if your Entity supports layers.  Make sure to attach things
            // to this if appropriate.
            mVisibleRepresentation = SpriteManager.AddSprite("redball.bmp", mContentManagerName);
            mVisibleRepresentation.AttachTo(this, false);

            mCollision = ShapeManager.AddCircle();
            mCollision.AttachTo(this, false);
            mCollision.Radius = 4.0f;
        }



        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            mCollision.X = mBody.Position.X;
            mCollision.Y = mBody.Position.Y;

        }

        public virtual void Destroy()
        {
            // Remove self from the SpriteManager:
            SpriteManager.RemovePositionedObject(this);

            // Remove any other objects you've created:
            SpriteManager.RemoveSprite(mVisibleRepresentation);
            ShapeManager.Remove(mCollision);
        }

        #endregion
    }
}
