using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomaticImageClassification.Utilities
{
    public class XmlFiguresReader
    {
        public static T ReadXml<T>(string file)
        {
            return XmlSerializationExtensions.FromXmlFile<T>(file);
        }
    }

    [Serializable]
    [XmlRoot("Figures")]
    public class Figures
    {
        [XmlElement("Figure")]
        public List<Figure> FigureList = new List<Figure>();

    }

    [Serializable]
    [XmlRoot("Figure")]
    public class Figure
    {
        [XmlElement("ID")]
        public string Id;

        [XmlElement("CLASS")]
        public string Class;

        [XmlElement("TITLE")]
        public string Title;

        [XmlElement("CAPTION")]
        public string Caption;

    }
}
