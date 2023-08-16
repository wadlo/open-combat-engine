using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Math and vector utility functions.
    /// </summary>
    static class GSAIUtils
    {
        /// <summary>
        /// Returns the `vector` with its length capped to `limit`.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static Vector3 ClampedV3(Vector3 vector, float limit)
        {
            var length_squared = vector.LengthSquared();
            var limit_squared = limit * limit;
            if (length_squared > limit_squared)
                vector *= Mathf.Sqrt(limit_squared / length_squared);
            return vector;
        }

        /// <summary>
        /// Returns an angle in radians between the positive X axis and the `vector`.
        /// This assumes orientation for 3D agents that are upright and rotate around the Y axis.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Vector3ToAngle(Vector3 vector)
        {
            return Mathf.Atan2(vector.X, vector.Z);
        }

        /// <summary>
        /// Returns an angle in radians between the positive X axis and the `vector`.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Vector2ToAngle(Vector2 vector)
        {
            return Mathf.Atan2(vector.X, -vector.Y);
        }

        /// <summary>
        /// Returns a directional vector from the given orientation angle.
        /// This assumes orientation for 2D agents or 3D agents that are upright and rotate around the Y axis.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 AngleToVector2(float angle)
        {
            return new Vector2(Mathf.Sin(-angle), Mathf.Cos(angle));
        }

        /// <summary>
        /// Returns a vector2 with `vector`'s x and y components.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Returns a vector3 with `vector`'s x and y components and 0 in z.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }
    }
}
