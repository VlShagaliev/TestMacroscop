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
            Thread.Sleep(15 * 1000);
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
                Assert.True(false, $"–азница между временем сервера и локальным = {(serverTimeDate - localTimeDate).Seconds} секунд и {(serverTimeDate - localTimeDate).Milliseconds / 10} миллисекунд");
            }
        }


        [Fact]
        public void DataTimeCheckJson()
        {
            //Arrange
            DateTime localTimeDate = DateTime.UtcNow;

            //Act
            //Thread.Sleep(15 * 1000);
            WebClient client = new();
            string request = client.DownloadString("http://demo.macroscop.com:8080/command?type=gettime&login=root&password=&responsetype=json");
            int index = request.IndexOf("\r\n\r\n");
            Console.WriteLine(index);
            string jsonText = request.Substring(index);
            JsonDocument json = JsonDocument.Parse(jsonText);
            DateTime serverTimeDate = DateTime.Parse(json.RootElement.ToString());

            //Assert
            if ((serverTimeDate - localTimeDate).Seconds <= 15)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false, $"–азница между временем сервера и локальным = {(serverTimeDate - localTimeDate).Seconds}секунд и {(serverTimeDate - localTimeDate).Milliseconds / 10} миллисекунд");
            }

            
        }


        [Fact]

        public void ConfigServerCheck()
        {
            //Arrange
            int quantityChannel = 0;

            //Act
            XmlReader xmlReader = XmlReader.Create("http://demo.macroscop.com:8080/configex?login=root&password=");
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "ChannelInfo":
                            {
                                quantityChannel++;
                                break;
                            }
                        default:
                            break;
                    }
                }
            }

            //Assert
            if (quantityChannel<6)
            {
                Assert.True(false, $" оличество каналов = {quantityChannel}");
            }
            else
            {
                Assert.True(true);
            }

        }
    }
}
