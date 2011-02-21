using System;
using System.Collections.Generic;
using System.Text;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Input;

using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace Gravitas
{
    public class GravityHelper
    {
        private const double G = .0000000000667;

        public GravityHelper()
        {

        }

        /* calcGravity calculates the relative gravity between a Body and   *
         * the Player. It gets the distance between the body and the Player *
         * Then, it calculates the freefall acceleration of the Player and  *
         * returns it. It only returns the magnitude of the gravity, not    *
         * direction.                                                       */
        public Vector3 calcGravity(Planet body, Player character)
        {
            float radius;

            //Calculates unit vector with the direction facing the planet.
            Vector3 unitVector = (body.Position - character.Position);
            unitVector.Normalize();

            //Calculates the distance between the two objects.
            //It puts radius ^ 2 into radius to speed up calc.
            radius = Vector3.DistanceSquared(character.Position, body.Position);           

            //If the distance is too far, it doesn't bother to calculate
            //gravity. It returns an empty Vector3 to be ignored. Change this number.
            if (radius > 99)
            {
                return new Vector3(0, 0, 0);
            }

            //Calculates the gravity for the Player in regards to the object.
            //Distance plays a big part.
            return Vector3.Multiply(unitVector, (float)(G * body.Mass / radius));
        }

     }
}
