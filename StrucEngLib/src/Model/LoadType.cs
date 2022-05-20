namespace StrucEngLib.Model
{
    /// <summary>
    /// Types of a Load
    /// </summary>
    public enum LoadType
    {
        None,
        Area,
        Gravity,
        Point
    }

    public static class LoadTypeMethods
    {
        public static string GetName(this LoadType s1)
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
}