using System;
using UnityEngine;

namespace OEPFramework.unityEngine.utils
{
    static class ColorUtils
    {
        public static string GetTextColor(Color c)
        {
            var r = (byte)(c.r * 255);
            var g = (byte)(c.g * 255);
            var b = (byte)(c.b * 255);
            var a = (byte)(c.a * 255);
            return "#" + a.ToString("X2") + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }

        public static Color32 GetColor(string color)
        {
            return color.Length > 7 ? GetColor32A(color) : GetColor32(color);
        }

        public static Color32 GetColor32A(string color)
        {
            try
            {
                color = color.Trim('#');
                byte a = Convert.ToByte(color.Substring(0, 2), 16);
                byte r = Convert.ToByte(color.Substring(2, 2), 16);
                byte g = Convert.ToByte(color.Substring(4, 2), 16);
                byte b = Convert.ToByte(color.Substring(6, 2), 16);
                return new Color32(r, g, b, a);
            }
            catch (Exception)
            {
                return Color.black;
            }
        }

        public static Color32 GetColor32(string color)
        {
            try
            {
                color = color.Trim('#');
                byte r = Convert.ToByte(color.Substring(0, 2), 16);
                byte g = Convert.ToByte(color.Substring(2, 2), 16);
                byte b = Convert.ToByte(color.Substring(4, 2), 16);
                return new Color32(r, g, b, 255);
            }
            catch (Exception)
            {
                return Color.black;
            }
        }

        public static Color MoveTowards(Color current, Color target, float maxDistanceDelta)
        {
            var color = Vector4.MoveTowards(ColorToVector(current), ColorToVector(target), maxDistanceDelta);
            return VectorToColor(color);
        }

        private static Vector4 ColorToVector(Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }

        private static Color VectorToColor(Vector4 vector)
        {
            return new Color(vector.x, vector.y, vector.z, vector.w);
        }
    }
}
