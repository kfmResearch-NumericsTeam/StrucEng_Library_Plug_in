using System;
using System.Collections.Generic;

namespace StrucEngLib.Model
{
    public enum LoadType
    {
        Area,
        Gravity,
        Point
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
                case LoadType.Point:
                    return "Point";
                default:
                    return "Unknown";
            }
        }
    }

    public interface Load
    {
        List<Layer> Layers { get; set; }
        LoadType LoadType { get; }
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