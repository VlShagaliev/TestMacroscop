using System;
using Xunit;
using System.Xml;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace TestMacroscop
{
    public class UnitTest1
    {
        [Fact]
        public void DataTimeCheckXml()
        {   
            //Arrange
            DateTime localTimeDate = DateTime.UtcNow;
            
            //Act
            //Thread.Sleep(20 * 1000);
            WebClient client = new();
            string request = client.DownloadString("http://demo.macroscop.com:8080/command?type=gettime&login=root&password=");
            int index = request.IndexOf("<?xml");
            string xml = request.Substring(index);
            XmlDocument xDoc = new();
            xDoc.LoadXml(xml);
            DateTime serverTimeDate = DateTime.Parse(xDoc.InnerText);

            //Assert
            if ((serverTimeDate - localTimeDate).Seconds <= 15 )
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }
        }


        [Fact]
        public void DataTimeCheckJson()
        {
            //Arrange
            DateTime localTimeDate = DateTime.UtcNow;

            //Act
            //Thread.Sleep(20 * 1000);
            WebClient client = new();
            string request = client.DownloadString("http://demo.macroscop.com:8080/command?type=gettime&login=root&password=&responsetype=json");
            int index = request.IndexOf("\r\n\r\n");
            Console.WriteLine(index);
            string xml = request.Substring(index);
            JsonDocument json = JsonDocument.Parse(xml);
            DateTime serverTimeDate = DateTime.Parse(json.RootElement.ToString());

            //Assert
            if ((serverTimeDate - localTimeDate).Seconds <= 15)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }
        }
    }
}
