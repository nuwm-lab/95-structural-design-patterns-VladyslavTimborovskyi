using System;
using System.Xml;
using System.Text.Json;

namespace StructuralPatterns
{
    
    public interface IXmlConverter
    {
        string ConvertToXml(string jsonData);
    }

    
    public class JsonToXmlAdapter : IXmlConverter
    {
        public string ConvertToXml(string jsonData)
        {
            try
            {
                
                var jsonDocument = JsonDocument.Parse(jsonData);

                
                XmlDocument xmlDocument = new XmlDocument();
                var rootElement = jsonDocument.RootElement;

                var root = xmlDocument.CreateElement("Root");
                xmlDocument.AppendChild(root);

                ConvertJsonElementToXml(rootElement, xmlDocument, root);

                return xmlDocument.OuterXml;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private void ConvertJsonElementToXml(JsonElement jsonElement, XmlDocument xmlDocument, XmlElement parentElement)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                XmlElement element = xmlDocument.CreateElement(property.Name);

                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    ConvertJsonElementToXml(property.Value, xmlDocument, element);
                }
                else
                {
                    element.InnerText = property.Value.ToString();
                }

                parentElement.AppendChild(element);
            }
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            string json = @"{
                ""Name"": ""John"",
                ""Age"": 30,
                ""Address"": {
                    ""Street"": ""123 Main St"",
                    ""City"": ""New York""
                }
            }";

            IXmlConverter converter = new JsonToXmlAdapter();
            string xml = converter.ConvertToXml(json);

            Console.WriteLine("Converted XML:");
            Console.WriteLine(xml);
        }
    }
}
