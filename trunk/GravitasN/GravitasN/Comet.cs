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

using Microsoft.Xna;
using Microsoft.Xna.Framework;


namespace GravitasN
{
    public class Comet : Planet
    {

        #region Fields

        private Vector2 mVelocity;

        #endregion

        #region Properties
        public Vector2 Velocity
        {
            get { return mVelocity; }
            set { mVelocity = value; }
        }

        #endregion

        #region Methods
        public Comet(string contentManagerName, float mass, float x, float y, float radius, Vector2 velocity)
            : base(contentManagerName, mass, x, y, radius)
        {
            this.mVelocity = velocity;
        }

        protected override void Initialize(bool addToManagers)
        {
            // Here you can preload any content you will be using
            // like .scnx files or texture files.

            if (addToManagers)
            {
                AddToManagers(null);
            }
        }

        public override void AddToManagers(Layer layerToAddTo)
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
            mCollision.Radius = mRadius;

            //Initializes the body at the point given in the constructor.
            mBody = BodyFactory.Instance.CreateCircleBody(Game1.PhysicsSim, mRadius, mMass);
            mBody.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y);
            mBody.Rotation = 0.1f;
            mBody.IsStatic = false;
            
            mGeom = GeomFactory.Instance.CreateCircleGeom(Game1.PhysicsSim, mBody, mRadius, 50);
            mGeom.RestitutionCoefficient = 0;
            mGeom.FrictionCoefficient = 10.0f;

            //mBody.LinearVelocity = mVelocity;
            mBody.ApplyForce(mVelocity);

        }

        public override void Activity()
        {
            mBody.ApplyForce(mVelocity);

            this.X = mBody.Position.X;
            this.Y = mBody.Position.Y;
        }
        #endregion
    }
}
