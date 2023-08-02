using UnityEngine;

public static class NormalizationHelper
    {
        public static float MinMax(float oldMin, float oldMax, float newMin, float newMax, float value)
        {
            return ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
        }

        public static double MinMax(double oldMin, double oldMax, double newMin, double newMax, double value)
        {
            return ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
        }

        public static Vector2 MinMax(Vector2 oldMin, Vector2 oldMax, Vector2 newMin, Vector2 newMax, Vector2 value)
        {
            return ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
        }
    }
