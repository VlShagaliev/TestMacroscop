using System;
using Xunit;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;

namespace TestMacroscop
{
    public class TestTimeFromJsonRequest
    {
        string link = "http://demo.macroscop.com:8080/command?type=gettime&login=root&password=&responsetype=json";

        public TimeSpan DifferenceTime(DateTime localTime)
        {
            WebClient client = new();
            string request = client.DownloadString(link);
            int index = request.IndexOf("\r\n\r\n");
            string jsonText = request.Substring(index);
            JsonDocument json = JsonDocument.Parse(jsonText);
            DateTime serverTimeDate = DateTime.Parse(json.RootElement.ToString());
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
            Assert.True(Math.Abs(time.Seconds) < 15, $"Разница между временем сервера и локальным = {Math.Abs(time.Seconds)} секунд и {Math.Abs(time.Milliseconds)} миллисекунд");

        }

    }
}
