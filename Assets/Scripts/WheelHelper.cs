using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public static class WheelHelper
    {
        public static Dictionary<int, float> TwoXAnglesDict = new Dictionary<int, float>
        {
            [1] = 60,
            [2] = 160,
            [3] = 220,
            [4] = 340
        };

        public static Dictionary<int, float> ThreeXAnglesDict = new Dictionary<int, float>
        {
            [1] = 20,
            [2] = 80,
            [3] = 240
        };

        public static Dictionary<int, float> FourXAnglesDict = new Dictionary<int, float>
        {
            [1] = 40,
            [2] = 120,
            [3] = 260
        };

        public static Dictionary<int, float> FiveXAnglesDict = new Dictionary<int, float>
        {
            [1] = 100,
            [2] = 280
        };

        public static Dictionary<int, float> SixXAnglesDict = new Dictionary<int, float>
        {
            [1] = 200,
            [2] = 300
        };

        public static Dictionary<int, float> EightXAnglesDict = new Dictionary<int, float>
        {
            [1] = 140,
            [2] = 320
        };

        public static Dictionary<int, float> TenXAnglesDict = new Dictionary<int, float>
        {
            [1] = 0,
            [2] = 360
        };

        private static Dictionary<int, float> GetAngleDictForMultiplier(int multiplier)
        {
            Dictionary<int, float> angleDict = new Dictionary<int, float>();
            switch (multiplier)
            {
                case 2:
                    angleDict = TwoXAnglesDict;
                    break;
                case 3:
                    angleDict = ThreeXAnglesDict;
                    break;
                case 4:
                    angleDict = FourXAnglesDict;
                    break;
                case 5:
                    angleDict = FiveXAnglesDict;
                    break;
                case 6:
                    angleDict = SixXAnglesDict;
                    break;
                case 8:
                    angleDict = EightXAnglesDict;
                    break;
                case 10:
                    angleDict = TenXAnglesDict;
                    break;
                default:
                    angleDict = TwoXAnglesDict;
                    break; // in case multiplier anything other than the ones mentioned int the wheel -- defaulting it to 2x the lowest
            }

            return angleDict;
        }

        private static float GetRandomAngleForMultiplier(float currentAngle, int multiplier)
        {
            float targetAngle = 0;
            // Get all possible angles for a multiplier and select one at random
            Dictionary<int, float> angleDictionary = GetAngleDictForMultiplier(multiplier);
            int randomAngleId = Random.Range(1, angleDictionary.Count);
            return angleDictionary.ElementAt(randomAngleId).Value;
        }

        public static float GetAngleForMultiplier(float currentAngle, int multiplier)
        {
            return GetRandomAngleForMultiplier(currentAngle, multiplier); // there might be multiple angles for a multiplier, get one randomly
        }
    }
}