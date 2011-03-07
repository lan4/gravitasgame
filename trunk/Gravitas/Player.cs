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


// Be sure to replace:
// 1.  The namespace
// 2.  The class name
// 3.  The constructor (should be the same as the class name)


namespace Gravitas
{
    public class Player : PositionedObject
    {
        #region Fields

        private Sprite mVisibleRepresentation;
        private Circle mCollision;

        private Body mBody;
        private Geom mGeom;

        private double mStartTime;

        private const double JUMP_TIME = 0.1f;

        public Circle Collision
        {
            get { return mCollision; }
        }

        private double mMass;

        public double Mass
        {
            get { return mMass; }
        }

        private Line mDirection;
        private Line mVelocityVector;
        private Line mAccelerationVector;

        private bool mBottomDisabled;

        public bool BottomDisabled
        {
            get { return mBottomDisabled; }
        }

        private Circle mBottom;

        public Circle Bottom
        {
            get { return mBottom; }
        }

        private bool mIsOnGround;

        public bool IsOnGround
        {
            get { return mIsOnGround; }
            set { mIsOnGround = value; }
        }

        private Vector3 bottomPointer;
        private Vector3 targetPointer;

        private bool mIsJumping = false;

        public bool IsJumping
        {
            get { return mIsJumping; }
            set { mIsJumping = value; }
        }

        public const float MAX_ACCELERATION = 40.0f;
        public const float FRICTION_CONSTANT = -30.0f;

        private Vector3 mTangentialVelocity;
        private Vector3 mPerpendicularVelocity;

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

            bottomPointer = Vector3.Zero;
            targetPointer = Vector3.Zero;

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
            mDirection.RelativeRotationZ = (float)(3.0f * Math.PI) / 2.0f;

            mVelocityVector = ShapeManager.AddLine();
            mVelocityVector.Color = Microsoft.Xna.Framework.Graphics.Color.Aquamarine;

            mAccelerationVector = ShapeManager.AddLine();
            mAccelerationVector.Color = Microsoft.Xna.Framework.Graphics.Color.Goldenrod;

            mBottom = ShapeManager.AddCircle();
            mBottom.AttachTo(this, false);
            mBottom.RelativePosition.X = -1.0f;
            mBottom.Radius = 0.1f;

            //Farseer Body of Player
            mBody = BodyFactory.Instance.CreateBody(Screens.GameScreen.PhysicsSim, 1, 1);
            mGeom = GeomFactory.Instance.CreateCircleGeom(Screens.GameScreen.PhysicsSim, mBody, 1, 12);
            mGeom.RestitutionCoefficient = 1;

            SpriteManager.Camera.AttachTo(this, false);
            SpriteManager.Camera.RelativePosition.Z = 30.0f;

            //velocityText = TextManager.AddText("Velocity: " + this.Velocity);
            //velocityText.AttachTo(SpriteManager.Camera, false);
            //velocityText.RelativePosition.X = 5.0f;
            //velocityText.RelativePosition.Y = 5.0f;
            //velocityText.RelativePosition.Z = -30.0f;
            
            //accelerationText = TextManager.AddText("Acceleration: " + this.Acceleration);
            //accelerationText.AttachTo(SpriteManager.Camera, false);
            //accelerationText.RelativePosition.X = 5.0f;
            //accelerationText.RelativePosition.Y = 4.0f;
            //accelerationText.RelativePosition.Z = -30.0f;
        }



        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            //DisplayHUD();
            DisplayVector();

            HandleInput();

            mVisibleRepresentation.X = mBody.Position.X;
            mVisibleRepresentation.Y = mBody.Position.Y;

            if (mStartTime - TimeManager.CurrentTime >= JUMP_TIME && mBottomDisabled)
            {
                mBottomDisabled = false; 
            }

            //if (mIsOnGround)
            //{
            //    ApplyFriction();
            //}
        }

        private void DisplayVector()
        {
            mVelocityVector.RelativePoint1.X = this.Position.X;
            mVelocityVector.RelativePoint1.Y = this.Position.Y;

            mVelocityVector.RelativePoint2.X = (this.Position.X - this.Velocity.X);
            mVelocityVector.RelativePoint2.Y = (this.Position.Y - this.Velocity.Y);

            mAccelerationVector.RelativePoint1.X = this.Position.X;
            mAccelerationVector.RelativePoint1.Y = this.Position.Y;

            mAccelerationVector.RelativePoint2.X = (this.Position.X - this.Acceleration.X);
            mAccelerationVector.RelativePoint2.Y = (this.Position.Y - this.Acceleration.Y);
            
        }

        //private void DisplayHUD()
        //{
        //    velocityText.DisplayText = "Velocity: " + this.Velocity;
        //    accelerationText.DisplayText = "Acceleration: " + this.Acceleration;
        //}

        private void HandleInput()
        {
            //DOES ALL OF THIS ONLY IF YOU'RE ON THE GROUND, ALEX!
            if (mIsOnGround)
            {
                if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Left) ||
                    InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    // Calculates the Tangential Velocity while moving left.
                    mTangentialVelocity.X = (float)(mDirection.AbsolutePoint1.X - mDirection.AbsolutePoint2.X) * 3.0f;
                    mTangentialVelocity.Y = (float)(mDirection.AbsolutePoint1.Y - mDirection.AbsolutePoint2.Y) * 3.0f;
                    //this.Velocity.X = (float)(mDirection.AbsolutePoint1.X - mDirection.AbsolutePoint2.X) * 3.0f;
                    //this.Velocity.Y = (float)(mDirection.AbsolutePoint1.Y - mDirection.AbsolutePoint2.Y) * 3.0f;

                }
                else if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Right) ||
                         InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    // Calculates the Tangential Velocity while moving left.
                    mTangentialVelocity.X = (float)(mDirection.AbsolutePoint2.X - mDirection.AbsolutePoint1.X) * 3.0f;
                    mTangentialVelocity.Y = (float)(mDirection.AbsolutePoint2.Y - mDirection.AbsolutePoint1.Y) * 3.0f;
                    //this.Velocity.X = (float)(mDirection.AbsolutePoint2.X - mDirection.AbsolutePoint1.X) * 3.0f;
                    //this.Velocity.Y = (float)(mDirection.AbsolutePoint2.Y - mDirection.AbsolutePoint1.Y) * 3.0f;

                }
                else
                {
                    // Sets tangential velocity to zero when not moving.
                    mTangentialVelocity = Vector3.Zero;
                }

                if (InputManager.Xbox360GamePads[0].ButtonPushed(Xbox360GamePad.Button.A) ||
                    InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    Vector3 unitVector = Vector3.Zero;
                    unitVector = Vector3.Normalize(this.Acceleration);

                    mIsOnGround = false;

                    mStartTime = TimeManager.CurrentTime;
                    mBottomDisabled = true;

                    mPerpendicularVelocity = Vector3.Multiply(unitVector, 100000.0f);

                    //this.mPerpendicularVelocity.X = (this.Position.X - mBottom.Position.X) * 10.0f;
                    //this.mPerpendicularVelocity.Y = (this.Position.Y - mBottom.Position.Y) * 10.0f;
                    //this.Velocity.X += (float)(this.Position.X - mBottom.Position.X) * 10.0f;
                    //this.Velocity.Y += (float)(this.Position.Y - mBottom.Position.Y) * 10.0f;
                    //mIsJumping = true;
                }


                //float vX = (mTangentialVelocity.X + mPerpendicularVelocity.X);
                //float vY = (mTangentialVelocity.Y + mPerpendicularVelocity.Y);

                this.Velocity.X = (mTangentialVelocity.X + mPerpendicularVelocity.X);
                this.Velocity.Y = (mTangentialVelocity.Y + mPerpendicularVelocity.Y);
            }

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

        private void ApplyFriction()
        {
            Vector3 unitVector = Vector3.Zero;

            if (this.mTangentialVelocity != Vector3.Zero)
            {
                unitVector = Vector3.Normalize(this.mTangentialVelocity);
            }

            this.Acceleration += Vector3.Multiply(unitVector, FRICTION_CONSTANT);
        }

        public void RotateToward(Planet target)
        {
            targetPointer = Vector3.Zero;

            targetPointer.X = this.Position.X - target.Position.X;
            targetPointer.Y = this.Position.Y - target.Position.Y;
            /*
            targetPointer.Normalize();
            if (targetPointer.X < 0.0f)
                this.RotationZ = (float)Math.Acos((double)targetPointer.X);
            else
                this.RotationZ = (float)Math.Asin((double)targetPointer.Y);
            */
            float rotation = (float)Math.Atan2(targetPointer.Y, targetPointer.X);

            this.RotationZ = rotation;
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
