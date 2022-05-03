namespace StrucEngLib.Model
{
    public enum LayerType
    {
        ELEMENT,
        SET
    }

    public interface Layer
    {
        LayerType LayerType { get; }
        string GetName();
        string PrettyPrint();
    }
}