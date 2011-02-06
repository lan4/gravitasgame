using System;
using System.Collections.Generic;
using System.Text;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Input;

using Microsoft.Xna;
using Microsoft.Xna.Framework;


// Be sure to replace:
// 1.  The namespace
// 2.  The class name
// 3.  The constructor (should be the same as the class name)


namespace Gravitas
{
    public class Player : PositionedObject
    {
        #region Fields

        // Here you'd define things that your Entity contains, like Sprites
        // or Circles:
        // private Sprite mVisibleRepresentation;
        // private Circle mCollision;
        private Sprite mVisibleRepresentation;
        private Circle mCollision;

        private double mMass;

        public double Mass
        {
            get { return mMass; }
        }

        private Line mDirection;

        private Circle mBottom;

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
        public Player(string contentManagerName, double mass)
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

            mDirection = ShapeManager.AddLine();
            mDirection.AttachTo(this, false);

            mBottom = ShapeManager.AddCircle();
            mBottom.AttachTo(this, false);
            mBottom.RelativePosition.Y = -1.0f;
            mBottom.Radius = 0.1f;
        }



        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            HandleInput();
        }

        private void HandleInput()
        {
            if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Left))
            {
                this.Velocity = new Vector3((float)(mDirection.AbsolutePoint1.X - mDirection.AbsolutePoint2.X) * 10.0f, 
                                            (float)(mDirection.AbsolutePoint1.Y - mDirection.AbsolutePoint2.Y) * 10.0f, 
                                            0.0f);
            }
            else if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Right))
            {
                this.Velocity = new Vector3((float)(mDirection.AbsolutePoint2.X - mDirection.AbsolutePoint1.X) * 10.0f,
                                            (float)(mDirection.AbsolutePoint2.Y - mDirection.AbsolutePoint1.Y) * 10.0f,
                                            0.0f);
            }
            else
            {
                this.Velocity = Vector3.Zero;
            }

            if (InputManager.Xbox360GamePads[0].RightTrigger.Position > 0.2)
            {
                this.RotationZVelocity = 2.0f;
            }
            else if (InputManager.Xbox360GamePads[0].LeftTrigger.Position > 0.2)
            {
                this.RotationZVelocity = -2.0f;
            }
            else
            {
                this.RotationZVelocity = 0.0f;
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
