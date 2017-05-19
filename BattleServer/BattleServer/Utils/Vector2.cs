using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer.Utils
{
    public class Vector2
    {
        private float _x;
        private float _y;
        public Vector2(float x = 0,float y = 0)
        {
            this._x = x;
            this._y = y;
        }

        public float X
        {
            set
            {
                _x = value;
            }
            get
            {
                return _x;
            }
        }

        public float Y
        {
            set
            {
                _y = value;
            }
            get
            {
                return _y;
            }
        }

        public Vector2 Copy()
        {
            return new Vector2(this.X, this.Y);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }


        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, float n)
        {
            return new Vector2(v1.X * n, v1.Y * n);
        }
        public static Vector2 operator *(float n, Vector2 v1)
        {
            return new Vector2(v1.X * n, v1.Y * n);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float x = a.X - b.X;
            float y = a.Y - b.Y;
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static float DistanceSquare(Vector2 a, Vector2 b)
        {
            float x = a.X - b.X;
            float y = a.Y - b.Y;
            return x * x + y * y;
        }
    }
}
