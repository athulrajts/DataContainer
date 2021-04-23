namespace System.Configuration
{
    public class FolderPath { }
    public class FilePath { }

    public interface ICustomTypeProvider
    {
        Type GetCustomType();
    }

    public interface IEditorValueProvider
    {
        object GetEditorValue();
    }
}
