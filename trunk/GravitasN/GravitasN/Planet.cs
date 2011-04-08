using System;
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
using FarseerGames.FarseerPhysics.Controllers;

using Microsoft.Xna;
using Microsoft.Xna.Framework;


namespace GravitasN
{
    public class Planet : PositionedObject
    {
        #region Fields

        private Body mBody;
        private Geom mGeom;

        public Geom Geometry
        {
            get { return mGeom; }
        }

        private Sprite mVisibleRepresentation;

        public Sprite VisibleRepresentation
        {
            get { return mVisibleRepresentation; }
        }

        private Circle mCollision;

        public Circle Collision
        {
            get { return mCollision; }
        }

        private float mMass;

        public float Mass
        {
            get { return mMass; }
        }

        public Body Body
        {
            get { return mBody; }
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
        public Planet(string contentManagerName, float mass, float x, float y, float radius, GravityController gravCont)
        {
            // Set the ContentManagerName and call Initialize:
            mContentManagerName = contentManagerName;
            mMass = mass;
            this.Position = new Vector3(x, y, 0.0f);

            // If you don't want to add to managers, make an overriding constructor
            Initialize(true);

            gravCont.BodyList.Add(mBody);
            gravCont.PointList.Add(mBody.Position);
        }

        protected virtual void Initialize(bool addToManagers)
        {
            // Here you can preload any content you will be using
            // like .scnx files or texture files.

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

            //Initializes the body at the point given in the constructor.
            mBody = BodyFactory.Instance.CreateCircleBody(Screens.GameScreen.PhysicsSim, 4.0f, mMass);
            mBody.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y);
            mBody.Rotation = 0.1f;
            mBody.IsStatic = true;
            mGeom = GeomFactory.Instance.CreateCircleGeom(Screens.GameScreen.PhysicsSim, mBody, 4.0f, 50);
            mGeom.RestitutionCoefficient = 0;
            mGeom.FrictionCoefficient = 100.0f;
        }



        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            this.X = mBody.Position.X;
            this.Y = mBody.Position.Y;

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
