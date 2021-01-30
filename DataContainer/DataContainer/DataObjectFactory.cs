using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    public static class DataObjectType
    {
        public const string Boolean = "b";
        public const string Byte = "byte";
        public const string Char = "c";
        public const string Integer = "i";
        public const string Long = "l";
        public const string Short = "short";
        public const string UInteger = "ui";
        public const string ULong = "ul";
        public const string UShort = "ushort";
        public const string Float = "f";
        public const string Enum = "enum";
        public const string Double = "d";
        public const string String = "s";
        public const string File = "file";
        public const string Folder = "folder";
        public const string Selectable = "opt";
        public const string DateTime = "dt";
        public const string TimeSpan = "ts";
        public const string Color = "color";
        public const string Point = "pt";
        public const string Array1D = "array-1";
        public const string Array2D = "array-2";
        public const string Container = "dc";
        public const string Collection = "dcl";
        public const string Xml = "xml";
        public const string Json = "json";
        public const string Password = "pwd";
        public const string NotSupported = "404";
    }


    /// <summary>
    /// Class used internally to create DataObjects
    /// </summary>
    public static class DataObjectFactory
    {
        /// <summary>
        /// Create mapping for <see cref="DataObject.Type"/> to it's implementation
        /// </summary>
        private static readonly Dictionary<string, Type> typeIdDataObjMapping = new Dictionary<string, Type>
        {
            { DataObjectType.Boolean , typeof(BoolDataObject) },
            { DataObjectType.Byte , typeof(ByteDataObject) },
            { DataObjectType.Short, typeof(ShortDataObject) },
            { DataObjectType.Integer , typeof(IntDataObject) },
            { DataObjectType.Long, typeof(LongDataObject)},
            { DataObjectType.UShort, typeof(UnsignedShortDataObject) },
            { DataObjectType.UInteger, typeof(UnsignedIntDataObject) },
            { DataObjectType.ULong, typeof(UnsignedLongDataObject) },
            { DataObjectType.Float , typeof(FloatDataObject) },
            { DataObjectType.Double , typeof(DoubleDataObject) },
            { DataObjectType.String , typeof(StringDataObject) },
            { DataObjectType.Enum , typeof(EnumDataObject) },
            { DataObjectType.Container , typeof(ContainerDataObject) },
            { DataObjectType.Char , typeof(CharDataObject)},
            { DataObjectType.Color , typeof(ColorDataObject)},
            { DataObjectType.Collection , typeof(CollectionDataObject) },
            { DataObjectType.Array1D , typeof(Array1DDataObject)},
            { DataObjectType.Array2D , typeof(Array2DDataObject)},
            { DataObjectType.DateTime , typeof(DateTimeDataObject)},
            { DataObjectType.TimeSpan , typeof(TimeSpanDataObject)},
            { DataObjectType.Point , typeof(PointDataObject)},
            { DataObjectType.Xml , typeof(XmlDataObject) },
            { DataObjectType.Json , typeof(JsonDataObject) },
            { DataObjectType.Password , typeof(PasswordDataObject) }
        };

        /// <summary>
        /// Create mapping for type of data to it's <see cref="DataObject"/> implementation
        /// </summary>
        private static readonly Dictionary<Type, Type> typeDataObjMapping = new Dictionary<Type, Type>
        {
            { typeof(bool) , typeof(BoolDataObject) },
            { typeof(byte) , typeof(ByteDataObject) },
            { typeof(short), typeof(ShortDataObject) },
            { typeof(int) , typeof(IntDataObject) },
            { typeof(long), typeof(LongDataObject) },
            { typeof(ushort), typeof(UnsignedShortDataObject) },
            { typeof(uint), typeof(UnsignedIntDataObject) },
            { typeof(ulong), typeof(UnsignedLongDataObject) },
            { typeof(float) , typeof(FloatDataObject) },
            { typeof(double) , typeof(DoubleDataObject) },
            { typeof(string) , typeof(StringDataObject) },
            { typeof(char), typeof(CharDataObject)},
            { typeof(Color), typeof(ColorDataObject) },
            { typeof(DateTime), typeof(DateTimeDataObject)},
            { typeof(TimeSpan), typeof(TimeSpanDataObject)},
            { typeof(Point), typeof(PointDataObject)}
        };

        /// <summary>
        /// Create mapping for <see cref="DataObject.Type"/> to it's implementation
        /// Used for creating <see cref="PropertyObject"/>
        /// </summary>
        private static readonly Dictionary<string, Type> typeIdPropObjMapping = new Dictionary<string, Type>
        {
            { DataObjectType.Boolean , typeof(BoolPropertyObject) },
            { DataObjectType.Byte , typeof(BytePropertyObject) },
            { DataObjectType.Short, typeof(ShortPropertyObject) },
            { DataObjectType.Integer , typeof(IntPropertyObject) },
            { DataObjectType.Long , typeof(LongPropertyObject) },
            { DataObjectType.UShort, typeof(UnsignedShortPropertyObject) },
            { DataObjectType.UInteger, typeof(UnsignedIntPropertyObject) },
            { DataObjectType.ULong, typeof(UnsignedLongPropertyObject) },
            { DataObjectType.Float , typeof(FloatPropertyObject) },
            { DataObjectType.Double , typeof(DoublePropertyObject) },
            { DataObjectType.String , typeof(StringPropertyObject) },
            { DataObjectType.Enum , typeof(EnumPropertyObject) },
            { DataObjectType.Selectable , typeof(SelectablePropertyObject) },
            { DataObjectType.Container , typeof(ContainerPropertyObject) },
            { DataObjectType.Char , typeof(CharPropertyObject)},
            { DataObjectType.Color , typeof(ColorPropertyObject)},
            { DataObjectType.Collection , typeof(CollectionPropertyObject)},
            { DataObjectType.File , typeof(FilePropertyObject)},
            { DataObjectType.Folder , typeof(FolderPropertyObject)},
            { DataObjectType.Array1D , typeof(Array1DPropertyObject) },
            { DataObjectType.Array2D , typeof(Array2DPropertyObject) },
            { DataObjectType.DateTime , typeof(DateTimePropertyObject) },
            { DataObjectType.TimeSpan , typeof(TimeSpanPropertyObject) },
            { DataObjectType.Point , typeof(PointPropertyObject) },
            { DataObjectType.Xml , typeof(XmlPropertyObject) },
            { DataObjectType.Json , typeof(JsonPropertyObject) },
            { DataObjectType.Password, typeof(PasswordPropertyObject) },
        };


        /// <summary>
        /// Create mapping for type of data to it's <see cref="PropertyObject"/> implementation
        /// </summary>
        private static readonly Dictionary<Type, Type> typePropObjMapping = new Dictionary<Type, Type>
        {
            { typeof(bool) , typeof(BoolPropertyObject) },
            { typeof(byte) , typeof(BytePropertyObject) },
            { typeof(short) , typeof(ShortPropertyObject) },
            { typeof(int) , typeof(IntPropertyObject) },
            { typeof(long) , typeof(LongPropertyObject) },
            { typeof(ushort), typeof(UnsignedShortPropertyObject) },
            { typeof(uint), typeof(UnsignedIntPropertyObject) },
            { typeof(ulong), typeof(UnsignedLongPropertyObject) },
            { typeof(float) , typeof(FloatPropertyObject) },
            { typeof(double) , typeof(DoublePropertyObject) },
            { typeof(string), typeof(StringPropertyObject) },
            { typeof(char), typeof(CharPropertyObject)},
            { typeof(Color), typeof(ColorPropertyObject)},
            { typeof(DateTime), typeof(DateTimePropertyObject) },
            { typeof(TimeSpan), typeof(TimeSpanPropertyObject)},
            { typeof(Point), typeof(PointPropertyObject)}
        };

        /// <summary>
        /// Support for 3rd party implementation for <see cref="DataObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterDataObject<T>()
            where T : DataObject
        {
            var instance = (DataObject)FormatterServices.GetUninitializedObject(typeof(T));

            // Allow replace existing implementation ??
            typeIdDataObjMapping.Add(instance.Type, typeof(T));
            typeDataObjMapping.Add(instance.GetDataType(), typeof(T));
        }

        /// <summary>
        /// Support for 3rd party implementations for <see cref="PropertyObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterPropertyObject<T>()
            where T : PropertyObject
        {
            var instance = (DataObject)FormatterServices.GetUninitializedObject(typeof(T));

            // Allow replace existing implementation ??
            typeIdPropObjMapping.Add(instance.Type, typeof(T));
            typePropObjMapping.Add(instance.GetDataType(), typeof(T));
        }

        /// <summary>
        /// Gets an uninitialized <see cref="DataObject"/> implementation for given type.
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static DataObject GetDataObject(string typeid)
        {
            return typeIdDataObjMapping.ContainsKey(typeid)
                ? (DataObject)FormatterServices.GetUninitializedObject(typeIdDataObjMapping[typeid])
                : new NotSupportedDataObject();
        }

        /// <summary>
        /// Gets an initialized <see cref="DataObject"/> instance of type <paramref name="type"/>
        /// with name <paramref name="name"/> and value <paramref name="value"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DataObject GetDataObject(string type, string name, object value, params object[] args)
        {
            object[] constructorArgs;

            if (args != null && args.Length > 0)
            {
                constructorArgs = new object[args.Length + 2];
                constructorArgs[0] = name;
                constructorArgs[1] = value;

                for (int i = 0; i < args.Length; i++)
                {
                    constructorArgs[i + 2] = args[i];
                }
            }
            else
            {
                constructorArgs = new object[] { name, value };
            }

            return (DataObject)Activator.CreateInstance(typeIdDataObjMapping[type], constructorArgs);
        }

        /// <summary>
        /// Gets an uninitialized <see cref="PropertyObject"/> implementation for given type.
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static DataObject GetPropertyObject(string typeid)
        {
            return typeIdPropObjMapping.ContainsKey(typeid)
                ? (DataObject)FormatterServices.GetUninitializedObject(typeIdPropObjMapping[typeid])
                : new NotSupportedDataObject();
        }

        /// <summary>
        /// Gets an initialized <see cref="PropertyObject"/> instance of type <paramref name="type"/>
        /// with name <paramref name="name"/> and value <paramref name="value"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DataObject GetPropertyObject(string type, string name, object value, params object[] args)
        {
            object[] constructorArgs;

            if (args != null && args.Length > 0)
            {
                constructorArgs = new object[args.Length + 2];
                constructorArgs[0] = name;
                constructorArgs[1] = value;

                for (int i = 0; i < args.Length; i++)
                {
                    constructorArgs[i + 2] = args[i];
                }
            }
            else
            {
                constructorArgs = new object[] { name, value };
            }

            return (DataObject)Activator.CreateInstance(typeIdPropObjMapping[type], constructorArgs);
        }


        /// <summary>
        /// Gets an initialized <see cref="DataObject"/> implementation for given value and name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataObject GetDataObjectFor(string name, object value, params object[] args)
        {
            Type valueType = value.GetType();

            if (typeDataObjMapping.ContainsKey(valueType))
            {
                object[] constructorArgs;

                if (args != null && args.Length > 0)
                {
                    constructorArgs = new object[args.Length + 2];
                    constructorArgs[0] = name;
                    constructorArgs[1] = value;

                    for (int i = 0; i < args.Length; i++)
                    {
                        constructorArgs[i + 2] = args[i];
                    }
                }
                else
                {
                    constructorArgs = new object[] { name, value };
                }

                return (DataObject)Activator.CreateInstance(typeDataObjMapping[valueType], constructorArgs);
            }
            else
            {
                /// special objects which can be used for multiple types
                return GetSpecialDataObject(name, value);
            }
        }

        private static DataObject GetSpecialDataObject(string name, object value)
        {
            if(value is DataObject data)
            {
                return data;
            }
            else if(value is Enum e)
            {
                return new EnumDataObject(name, e);
            }
            else if(value is IDataContainer d)
            {
                return new ContainerDataObject(name, d);
            }
            else if(value is Array a)
            {
                if (a.GetType().GetElementType().IsPrimitive == false)
                {
                    throw new NotSupportedException("Array of non primitive types not supported");
                }

                if(a.Rank == 1)
                {
                    return new Array1DDataObject(name, a);
                }
                else if(a.Rank == 2)
                {
                    return new Array2DDataObject(name, a);
                }
                else
                {
                    throw new NotSupportedException("Array of more than 2 dimensions not supported");
                }
            }
            else if(value is IList l)
            {
                return new CollectionDataObject(name, l);
            }
            else
            {
                return new ContainerDataObject(name, value);
            }

        }

        /// <summary>
        /// Gets an initialized <see cref="PropertyObject"/> implementation for given value and name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PropertyObject GetPropertyObjectFor(string name, object value, params object[] args)
        {
            if (value is null)
            {
                throw new NullReferenceException();
            }

            Type valueType = value.GetType();

            if (typePropObjMapping.ContainsKey(valueType))
            {
                object[] constructorArgs;

                if (args != null && args.Length > 0)
                {
                    constructorArgs = new object[args.Length + 2];
                    constructorArgs[0] = name;
                    constructorArgs[1] = value;

                    for (int i = 0; i < args.Length; i++)
                    {
                        constructorArgs[i + 2] = args[i];
                    }
                }
                else
                {
                    constructorArgs = new object[] { name, value };
                }

                return (PropertyObject)Activator.CreateInstance(typePropObjMapping[valueType], constructorArgs);
            }
            else
            {
                /// special objects which can be used for multiple types
                return GetSpecialPropertyObject(name, value);
            }
        }

        private static PropertyObject GetSpecialPropertyObject(string name, object value)
        {
            if (value is PropertyObject data)
            {
                return data;
            }
            else if (value is Enum e)
            {
                return new EnumPropertyObject(name, e);
            }
            else if (value is IDataContainer d)
            {
                return new ContainerPropertyObject(name, d);
            }
            else if (value is Array a)
            {
                if (a.GetType().GetElementType().IsPrimitive == false)
                {
                    throw new NotSupportedException("Array of non primitive types not supported");
                }

                if (a.Rank == 1)
                {
                    return new Array1DPropertyObject(name, a);
                }
                else if (a.Rank == 2)
                {
                    return new Array2DPropertyObject(name, a);
                }
                else
                {
                    throw new NotSupportedException("Array of more than 2 dimensions not supported");
                }
            }
            else if (value is IList l)
            {
                return new CollectionPropertyObject(name, l);
            }
            else
            {
                return new ContainerPropertyObject(name, value);
            }

        }


    }
}
