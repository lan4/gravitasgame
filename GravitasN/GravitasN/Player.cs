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
    public class Player : PositionedObject
    {
        #region Fields

        private Sprite mVisibleRepresentation;
        private Circle mCollision;

        private Body mBody;
        private Geom mGeom;

        public Circle Collision
        {
            get { return mCollision; }
        }

        private float mMass;

        public float Mass
        {
            get { return mMass; }
        }

        private bool mIsOnGround;

        public bool IsOnGround
        {
            get { return mIsOnGround; }
            set { mIsOnGround = value; }
        }

        private bool mIsJumping = false;

        public bool IsJumping
        {
            get { return mIsJumping; }
            set { mIsJumping = value; }
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
        public Player(string contentManagerName, float mass)
        {
            // Set the ContentManagerName and call Initialize:
            mContentManagerName = contentManagerName;
            mMass = mass;

            // If you don't want to add to managers, make an overriding constructor
            Initialize(true);
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

            //Farseer Body of Player
            mBody = BodyFactory.Instance.CreateCircleBody(Game1.PhysicsSim, 1.0f, mMass);
            mGeom = GeomFactory.Instance.CreateCircleGeom(Game1.PhysicsSim, mBody, 1, 12);
            mGeom.RestitutionCoefficient = 0;

            SpriteManager.Camera.AttachTo(this, false);
            SpriteManager.Camera.RelativePosition.Z = 30.0f;

        }



        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.

            //HandleInput();

            this.X = mBody.Position.X;
            this.Y = mBody.Position.Y;
        }

        private void HandleInput()
        {
            //if (mIsOnGround)
            //{
                if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Left) ||
                    InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    
                    
                    mBody.ApplyForce(hForce);
                }
                else if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Right) ||
                         InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                {

                }
                else
                {

                }

                if (InputManager.Xbox360GamePads[0].ButtonPushed(Xbox360GamePad.Button.A) ||
                    InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Space))
                {

                }

            //}

            if (InputManager.Xbox360GamePads[0].ButtonDown(Xbox360GamePad.Button.LeftTrigger) ||
                InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                SpriteManager.Camera.RelativeVelocity.Z = -3.0f;
            }
            else if (InputManager.Xbox360GamePads[0].ButtonDown(Xbox360GamePad.Button.RightTrigger) ||
                     InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                SpriteManager.Camera.RelativeVelocity.Z = 3.0f;
            }
            else
            {
                SpriteManager.Camera.RelativeVelocity.Z = 0.0f;
            }
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
