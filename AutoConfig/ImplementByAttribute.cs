using System;

namespace ReadMeter.Api.AutoConfig
{
    public class ImplementByAttribute : Attribute
    {
        public Type Type { get; }

        public ImplementByAttribute(Type type)
        {
            Type = type;
        }
    }
}