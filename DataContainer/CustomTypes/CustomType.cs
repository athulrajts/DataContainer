using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace KEI.Infrastructure
{
    public class FolderPath { }
    public class FilePath { }

    public interface ICustomTypeProvider
    {
        Type GetCustomType();
    }
}
