using System.Runtime.Serialization;

namespace CrmConsoleApp
{
    //public class IntegrationErrorDataContract
    //{
    //    public string ErrorSource { get; set; }
    //    public string ErrorCode { get; set; }
    //    public string ErrorMessage { get; set; }
    //    public string ErrorStackTrace { get; set; }
    //    public string ReferenceId { get; set; }
    //    public string ReferenceNumber { get; set; }
    //}
    [DataContract]
    public class IntegrationErrorContract<T> where T : class
    {
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public string ServiceType { get; set; }
        [DataMember]
        public string TrackingId { get; set; }
        [DataMember]
        public T Body { get; set; }
    }

    public class IntegrationErrorDataContract
    {
        public string ErrorSource { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStackTrace { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceNumber { get; set; }
    }
}