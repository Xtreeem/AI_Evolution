using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    public static class Misc
    {
        private static float Scale = 0.5f;
        public static Random Random = new Random();

        public static float NextFloat(this Random rand)
        {
            return (float)rand.NextDouble();
        }
        public static float NextFloat(this Random rand, float min, float max)
        {
            if (max < min)
                throw new ArgumentException("max cannot be less than min");
            return (float)rand.NextDouble() * (max - min) + min;
        }

        public static bool TrueOrFalse()
        {
            if (Random.Next(0, 2) == 1)
                return true;
            else return false;
        }

        public static float GlobalScale { get { return Scale; } }

        public static Vector2 PointToVector(Point P)
        {
            return new Vector2(P.X, P.Y);
        }

        public static bool IsThisNumberEven(float Number)
        {
            if (Number % 2 == 0)
                return true;
            else
                return false;
        }

        public static int DivideByTwo(float Input, bool Rounding_Up)
        {
            int result = new int();
            float temp = Input;
            if (Rounding_Up)
            {
                    result = (int)(temp / 2);
                if (temp % 2 != 0)
                {
                    result += 1;
                }

            }
            else
            {
                temp = Input / 2;
                result = (int)temp;
            }
            return result;
        }

    }


}
