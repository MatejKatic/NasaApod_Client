using Apod;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NasaAPOD_Client
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/RestServis.Models")]
    public class ApodModel
    {
        public ApodModel(string copyright, DateTime date, string explanation, string hdurl, MediaType media_type, string service_version, string title, string url)
        {
            this.copyright = copyright;
            this.date = date;
            this.explanation = explanation;
            this.hdurl = hdurl;
            this.media_type = media_type;
            this.service_version = service_version;
            this.title = title;
            this.url = url;
        }

        [DataMember(Order = 0)]
        public string copyright { get; set; }

        [DataMember(Order = 1)]
        public DateTime date { get; set; }

        [DataMember(Order = 2)]
        public string explanation { get; set; }

        [DataMember(Order = 3)]
        public string hdurl { get; set; }

        [DataMember(Order = 4)]
        public MediaType media_type { get; set; }

        [DataMember(Order = 5)]
        public string service_version { get; set; }

        [DataMember(Order = 6)]
        public string title { get; set; }

        [DataMember(Order = 7)]
        public string url { get; set; }
    }
}
