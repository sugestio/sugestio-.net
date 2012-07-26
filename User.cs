using System;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace Sugestio
{
    [XmlType("user")]
    public class User : ISugestioObject
    {

        private string id = null;
        [XmlElement("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string location_latlong = null;
        [XmlElement("location_latlong")]
        public string Location_latlong
        {
            get { return location_latlong; }
            set { location_latlong = value; }
        }

        private string gender = null;
        [XmlElement("gender")]
        public String Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        private string birthday = null;
        [XmlElement("birthday")]
        public String Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        public User()
        {
        }

        public User(string id)
        {
            this.id = id;
        }

        public string GetResource()
        {
            return "/users.xml";
        }

        public string getRootName()
        {
            return "users";
        }
    }
}
