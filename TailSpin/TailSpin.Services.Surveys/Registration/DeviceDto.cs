




 




namespace TailSpin.Services.Surveys.Registration
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeviceDto
    {
        [DataMember]
        public string Uri { get; set; }

        [DataMember]
        public bool RecieveNotifications { get; set; }
    }
}