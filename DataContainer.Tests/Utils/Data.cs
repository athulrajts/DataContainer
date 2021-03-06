﻿using System.Configuration;
using System;
using System.Collections.ObjectModel;

namespace DataContainer.Utils
{
    [Serializable]
    public class POCO
    {

        public char CharProperty { get; set; } = 'c';
        public int IntProperty { get; set; } = 42;
        public float FloatProeprty { get; set; } = 3.14f;
        public double DoubleProperty { get; set; } = 1.21;
        public string StringProperty { get; set; } = "xUnit";
        public Enum EnumProperty { get; set; } = GCNotificationStatus.Succeeded;

    }

    public class NestedPOCO : POCO
    {
        public POCO ObjectProperty { get; set; } = new POCO();
    }

    public class UnitializedNestedPOCO : NestedPOCO
    {
        public UnitializedNestedPOCO()
        {
            CharProperty = default;
            IntProperty = default;
            FloatProeprty = default;
            DoubleProperty = default;
            StringProperty = default;
            EnumProperty = default;
            ObjectProperty = default;
        }
    }

    public class UninitializedPOCO : POCO
    {
        public UninitializedPOCO()
        {
            CharProperty = default;
            IntProperty = default;
            FloatProeprty = default;
            DoubleProperty = default;
            StringProperty = default;
            EnumProperty = default;
        }
    }

    public class POCOCollection : Collection<POCO> { }

    public class NestedPOCOCollection : Collection<NestedPOCO> { }

    [Serializable]
    public class BindingTestObject : InpcImplementation
    {
        private int intProperty;
        public int IntProperty
        {
            get { return intProperty; }
            set { SetProperty(ref intProperty, value); }
        }
    }

    public class Component
    {
        // Change to enum ?
        public string Type { get; set; }

        public string DisplayableName { get; set; }

        public IPropertyContainer TestParameters { get; set; } = new PropertyContainer
        {
            { "Property1", 1 },
            { "Property2", 2 }
        };
    }

}

