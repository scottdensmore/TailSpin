




 




namespace TailSpin.Web.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Web;
    using System.Web.UI;

    public class MvcSerializer
    {
        private const SerializationMode DefaultMode = SerializationMode.Plaintext;

        private static readonly Dictionary<SerializationMode, Func<IStateFormatter>> RegisteredFormatterFactories =
                new Dictionary<SerializationMode, Func<IStateFormatter>>
                {
                    { SerializationMode.Plaintext, () => new ObjectStateFormatter() },
                    { SerializationMode.Encrypted, LazilyGetFormatterGenerator(true /* encrypt */, false /* sign */) },
                    { SerializationMode.Signed, LazilyGetFormatterGenerator(false /* encrypt */, true /* sign */) },
                    { SerializationMode.EncryptedAndSigned, LazilyGetFormatterGenerator(true /* encrypt */, true /* sign */) }
                };

        public object Deserialize(string serializedValue) 
        {
            return this.Deserialize(serializedValue, DefaultMode);
        }

        public virtual object Deserialize(string serializedValue, SerializationMode mode) 
        {
            if (String.IsNullOrEmpty(serializedValue)) 
            {
                throw new ArgumentException("Argument must not be null or empty", "serializedValue");
            }

            IStateFormatter formatter = GetFormatter(mode);
            try 
            {
                object deserializedValue = formatter.Deserialize(serializedValue);
                return deserializedValue;
            }
            catch (Exception ex) 
            {
                throw CreateSerializationException(ex);
            }
        }

        public string Serialize(object state)
        {
            return this.Serialize(state, DefaultMode);
        }

        public virtual string Serialize(object state, SerializationMode mode)
        {
            IStateFormatter formatter = GetFormatter(mode);
            string serializedValue = formatter.Serialize(state);
            return serializedValue;
        }

        private static IStateFormatter GetFormatter(SerializationMode mode) 
        {
            Func<IStateFormatter> formatterFactory;
            if (!RegisteredFormatterFactories.TryGetValue(mode, out formatterFactory)) 
            {
                throw new ArgumentOutOfRangeException("mode", "The provided SerializationMode is invalid.");
            }

            return formatterFactory();
        }

        private static Func<IStateFormatter> LazilyGetFormatterGenerator(bool encrypt, bool sign) 
        {
            var generatorFactory = new Lazy<Func<IStateFormatter>>(
                () => TokenPersister.CreateFormatterGenerator(encrypt, sign));

            return () =>
            {
                Func<IStateFormatter> generator = generatorFactory.Eval();
                return generator();
            };
        }

        private static SerializationException CreateSerializationException(Exception innerException)
        {
            return new SerializationException("Deserialization failed. Verify that the data is being deserialized using the same SerializationMode with which it was serialized. Otherwise see the inner exception.", innerException);
        }

        // This type is very difficult to unit-test because Page.ProcessRequest() requires mocking
        // much of the hosting environment. For now, we can perform functional tests of this feature.
        private sealed class TokenPersister : PageStatePersister 
        {
            private TokenPersister(Page page) : base(page) 
            {
            }

            public static Func<IStateFormatter> CreateFormatterGenerator(bool encrypt, bool sign) 
            {
                // This code instantiates a page and tricks it into thinking that it's servicing
                // a postback scenario with encrypted ViewState, which is required to make the
                // StateFormatter properly decrypt data. Specifically, this code sets the
                // internal Page.ContainsEncryptedViewState flag.
                TextWriter writer = TextWriter.Null;
                HttpResponse response = new HttpResponse(writer);
                HttpRequest request = new HttpRequest("DummyFile.aspx", HttpContext.Current.Request.Url.ToString(), "__EVENTTARGET=true" + (encrypt ? "&__VIEWSTATEENCRYPTED=true" : null));
                HttpContext context = new HttpContext(request, response);

                Page page = new Page
                {
                    EnableViewStateMac = sign,
                    ViewStateEncryptionMode = encrypt ? ViewStateEncryptionMode.Always : ViewStateEncryptionMode.Never
                };
                page.ProcessRequest(context);

                return () => new TokenPersister(page).StateFormatter;
            }

            public override void Load() 
            {
                throw new NotImplementedException();
            }

            public override void Save() 
            {
                throw new NotImplementedException();
            }
        }
    }
}
