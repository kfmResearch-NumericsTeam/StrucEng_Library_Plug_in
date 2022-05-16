namespace StrucEngLib.Model
{
    /// <summary>
    /// Indicates the type of the displacement
    /// </summary>
    public enum SetDisplacementType
    {
        NONE = 0,
        GENERAL = 1,
        PINNED = 2,
        FIXED = 3,
        FIXED_XX = 4,
        FIXED_YY = 5,
        FIXED_ZZ = 6,
        ROLLER_X = 7,
        ROLLER_Y = 8,
        ROLLER_Z = 9,
        ROLLER_XY = 10,
        ROLLER_YZ = 11,
        ROLLER_XZ = 12
    }
}