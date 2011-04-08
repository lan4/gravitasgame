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
using FarseerGames.FarseerPhysics.Dynamics.Springs;

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

        private LinearSpring planetAttacher;

        private Planet onPlanet;

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

        private Line mDirection;

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

            onPlanet = null;

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

            //Farseer Body of Player
            mBody = BodyFactory.Instance.CreateCircleBody(Screens.GameScreen.PhysicsSim, 1.0f, mMass);
            mBody.LinearDragCoefficient = 0.0f;   
            mGeom = GeomFactory.Instance.CreateCircleGeom(Screens.GameScreen.PhysicsSim, mBody, 1, 50);
            mGeom.RestitutionCoefficient = 0;

            //planetAttacher = SpringFactory.Instance.CreateLinearSpring(mBody, Vector2.Zero, mBody, Vector2.One, 3.0f, 1.0f);

            SpriteManager.Camera.AttachTo(this, false);
            SpriteManager.Camera.RelativePosition.Z = 30.0f;

        }



        public virtual void Activity()
        {
            // This code should do things like set Animations, respond to input, and so on.
            onPlanet = FindClosestPlanet(Screens.GameScreen.PlanetList);

            mIsOnGround = mGeom.Collide(onPlanet.Geometry);

            if (mIsOnGround)
                AttachToPlanet();

            HandleInput();

            this.X = mBody.Position.X;
            this.Y = mBody.Position.Y;
        }

        private void AttachToPlanet()
        {
            if (planetAttacher == null)
            {
                planetAttacher = SpringFactory.Instance.CreateLinearSpring(mBody, Vector2.Zero, onPlanet.Body, Vector2.Zero, 300.0f, 1.0f);
            }
            
            planetAttacher.Body2 = onPlanet.Body;
        }

        private Planet FindClosestPlanet(List<Planet> planetList)
        {
            Planet closestPlanet = null;
            float newDistance = -1.0f;
            float curDistance = -1.0f;

            foreach (Planet aPlanet in planetList)
            {
                if (closestPlanet == null)
                {
                    closestPlanet = aPlanet;

                    curDistance = (float)Math.Sqrt(Math.Pow(mBody.Position.X - closestPlanet.Body.Position.X, 2.0f) + Math.Pow(mBody.Position.Y - closestPlanet.Body.Position.Y, 2.0f));
                }

                newDistance = (float)Math.Sqrt(Math.Pow(mBody.Position.X - aPlanet.Body.Position.X, 2.0f) + Math.Pow(mBody.Position.Y - aPlanet.Body.Position.Y, 2.0f));

                if (newDistance < curDistance)
                {
                    closestPlanet = aPlanet;

                    curDistance = (float)Math.Sqrt(Math.Pow(mBody.Position.X - closestPlanet.Body.Position.X, 2.0f) + Math.Pow(mBody.Position.Y - closestPlanet.Body.Position.Y, 2.0f));
                }
            }

            return closestPlanet;
        }

        private Vector2 CalculateDirection()
        {
            Vector3 zDir = new Vector3(0, 0, 2.0f);
            Vector3 towardPlanet = Vector3.Subtract(new Vector3(onPlanet.Body.Position, 0.0f), this.Position);
            towardPlanet.Normalize();
            towardPlanet = Vector3.Multiply(towardPlanet, 2.0f);

            Vector3 finalDirection = Vector3.Cross(towardPlanet, zDir);

            Vector2 finalDir = new Vector2(finalDirection.X, finalDirection.Y);

            return finalDir;
        }

        private void HandleInput()
        {
            if (mIsOnGround)
            {
                if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Left) ||
                    InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    mBody.ApplyForce(CalculateDirection());
                }
                else if (InputManager.Xbox360GamePads[0].LeftStick.AsDPadDown(Xbox360GamePad.DPadDirection.Right) ||
                         InputManager.Keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    mBody.ApplyForce(Vector2.Negate(CalculateDirection()));
                }
                else
                {

                }

                if (InputManager.Xbox360GamePads[0].ButtonPushed(Xbox360GamePad.Button.A) ||
                    InputManager.Keyboard.KeyPushed(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    Vector3 towardPlanet = Vector3.Subtract(new Vector3(onPlanet.Body.Position, 0.0f), this.Position);
                    towardPlanet.Normalize();
                    towardPlanet = Vector3.Multiply(towardPlanet, 10.0f);
                    towardPlanet = Vector3.Negate(towardPlanet);

                    Vector2 awayFromPlanet = new Vector2(towardPlanet.X, towardPlanet.Y);

                    mBody.ApplyImpulse(awayFromPlanet);
                }

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
