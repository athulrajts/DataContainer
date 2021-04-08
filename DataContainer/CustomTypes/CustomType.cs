using System;

namespace KEI.Infrastructure
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
