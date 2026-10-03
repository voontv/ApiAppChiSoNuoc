using System;

namespace ReadMeter.Api.AutoConfig
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConvertToAttribute : Attribute
    {
        public Type ToType { get; }

        public ConvertToAttribute(Type toType)
        {
            ToType = toType;
        }
    }
}