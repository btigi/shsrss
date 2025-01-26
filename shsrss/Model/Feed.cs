using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace shsrss.Model
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class rss
    {
        public rssChannel channel { get; set; }

        [XmlAttribute]
        public decimal version { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rssChannel
    {
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string language { get; set; }

        [XmlElement("item")]
        public rssChannelItem[] item { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rssChannelItem
    {
        public string title { get; set; }
        public string link { get; set; }
        [XmlIgnore]
        public string Description { get; set; }
        [XmlElement("description")]
        public XmlCDataSection DescriptionCData
        {
            get
            {
                var doc = new XmlDocument();
                return doc.CreateCDataSection(Description);
            }
            set
            {
                Description = value.Value;
            }
        }
        public rssChannelItemGuid guid { get; set; }
        public string pubDate { get; set; }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class rssChannelItemGuid
    {
        [XmlAttribute]
        public bool isPermaLink { get; set; }
        
        [XmlText]
        public ushort Value { get; set; }
    }
}
