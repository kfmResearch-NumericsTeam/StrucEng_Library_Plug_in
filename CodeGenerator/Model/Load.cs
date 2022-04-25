using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CodeGenerator.Model
{
    public enum LoadType
    {
        Area,
        Gravity
    }

    public static class LoadTypeMethods
    {
        public static String GetName(this LoadType s1)
        {
            switch (s1)
            {
                case LoadType.Area:
                    return "Area";
                case LoadType.Gravity:
                    return "Gravity";
                default:
                    return "Unknown";
            }
        }
    }

    public interface Load
    {
        List<Layer> Layers { get; set; }
        LoadType GetType();
    }

    public class GravityLoad : Load
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public LoadType GetType() => LoadType.Gravity;
    }

    public class AreaLoad : Load
    {
        public string Z { get; set; } = "0.03";
        public string Axes { get; set; } = "local";
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public LoadType GetType() => LoadType.Area;
    }

    // public enum StrucEngType
    // {
    //     Element, Set, Load, Step
    // }
    //
    // public interface StrucEngObject
    // {
    //     StrucEngType Type { get; }
    //     string Key { get; }
    // }
}