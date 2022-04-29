namespace CodeGenerator.Model
{
    public enum LayerType
    {
        ELEMENT,
        SET
    }

    public interface Layer
    {
        LayerType GetType();
        string GetName();
        string PrettyPrint();
    }
}