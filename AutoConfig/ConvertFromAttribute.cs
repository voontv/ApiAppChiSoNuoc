using System;

namespace ReadMeter.Api.AutoConfig
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConvertFromAttribute : Attribute
    {
        public Type FromType { get; }

        public ConvertFromAttribute(Type fromType)
        {
            FromType = fromType;
        }
    }
}