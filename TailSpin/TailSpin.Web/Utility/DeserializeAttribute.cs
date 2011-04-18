




 




namespace TailSpin.Web.Utility 
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class DeserializeAttribute : CustomModelBinderAttribute
    {
        private const SerializationMode DefaultSerializationMode = SerializationMode.Plaintext;

        public DeserializeAttribute() : this(DefaultSerializationMode) 
        {
        }

        public DeserializeAttribute(SerializationMode mode) 
        {
            this.Mode = mode;
        }

        public SerializationMode Mode { get; private set; }

        public override IModelBinder GetBinder()
        {
            return new DeserializingModelBinder(this.Mode);
        }

        private sealed class DeserializingModelBinder : IModelBinder 
        {
            private readonly SerializationMode mode;

            public DeserializingModelBinder(SerializationMode mode) 
            {
                this.mode = mode;
            }

            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) 
            {
                if (bindingContext == null) 
                {
                    throw new ArgumentNullException("bindingContext");
                }

                ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (result == null) 
                {
                    // nothing found
                    return null;
                }

                MvcSerializer serializer = new MvcSerializer();
                string serializedValue = (string)result.ConvertTo(typeof(string), CultureInfo.InvariantCulture);
                return serializer.Deserialize(serializedValue, this.mode);
            }
        }
    }
}
