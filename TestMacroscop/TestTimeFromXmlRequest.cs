using System;
using Xunit;
using System.Net;
using System.Xml;

namespace TestMacroscop
{
    public class TestTimeFromXmlRequest
    {
        string link = "http://demo.macroscop.com:8080/command?type=gettime&login=root&password=";

        public TimeSpan DifferenceTime(DateTime localTime)
        {
            WebClient client = new();
            string request = client.DownloadString(link);
            int index = request.IndexOf("<?xml");
            string xml = request.Substring(index);
            XmlDocument xDoc = new();
            xDoc.LoadXml(xml);
            DateTime serverTimeDate = DateTime.Parse(xDoc.InnerText);
            return serverTimeDate - localTime;
        }

        [Fact]
        public void TestDifferenceTimeMore15Seconds()
        {
            //Arrange

            DateTime localTimeDate = DateTime.UtcNow.AddSeconds(16);

            //Act
            
            TimeSpan time = DifferenceTime(localTimeDate);

            //Assert
            Assert.True(Math.Abs(time.TotalSeconds) < 15, $"Разница между временем сервера и локальным = {Math.Abs(time.Seconds)} секунд и {Math.Abs(time.Milliseconds)} миллисекунд");
        }

        [Fact]
        public void TestDifferenceTimeLess15Seconds()
        {
            //Arrange
            DateTime localTimeDate = DateTime.UtcNow.AddSeconds(13);

            //Act
            TimeSpan time = DifferenceTime(localTimeDate);
            
            //Assert
            Assert.True(Math.Abs(time.TotalSeconds) < 15, $"Разница между временем сервера и локальным = {Math.Abs(time.Seconds)} секунд и {Math.Abs(time.Milliseconds)} миллисекунд");
        }
    }
}
