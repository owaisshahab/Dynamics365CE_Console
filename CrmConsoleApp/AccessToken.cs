using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CrmConsoleApp
{
    [DataContract]
    class AccessToken
    {
        [DataMember]
        public string access_token
        { get; set; }
    }
}
