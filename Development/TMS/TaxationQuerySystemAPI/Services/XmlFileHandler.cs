using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Services
{
    public class XmlFileHandler<T> where T : class, new()
    {
        public T xmlObjects = new T();
        private readonly string xmlFilePath = null;
        public IHostingEnvironment _environment { get; }
        private string root { get; }

        public XmlFileHandler(IHostingEnvironment environment, string root, string xmlFile)
        {
            _environment = environment;
            this.xmlFilePath = Path.Combine(_environment.WebRootPath, xmlFile);
            this.root = root;
        }
        public void SaveXml()
        {
            StreamWriter writer = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(this.root));
                writer = new StreamWriter(xmlFilePath);
                serializer.Serialize(writer, xmlObjects);
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            finally
            {
                writer?.Flush();
                writer?.Close();
                writer?.Dispose();
            }

        }

        public void LoadXml()
        {
            StreamReader reader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(this.root));
                reader = new StreamReader(xmlFilePath);
                object menuObjects = serializer.Deserialize(reader);
                xmlObjects = menuObjects == null ? new T() : (T)menuObjects;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            finally
            {
                reader?.Close();
                reader?.Dispose();
            }
        }
    }
}
