using UnityEngine;

namespace Runtime.Utilies
{
    public static class UIEaseUtility
    {
        public static float EaseInOut(float t)
        {
            return t * t * (3f - 2f * t);
        }

        public static float EaseIn(float t)
        {
            return t * t;
        }

        public static float EaseOut(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }

        public static float Bounce(float t)
        {
            return Mathf.Abs(Mathf.Sin(6.28f * (t + 1) * (t + 1)) * (1 - t));
        }
    }
}