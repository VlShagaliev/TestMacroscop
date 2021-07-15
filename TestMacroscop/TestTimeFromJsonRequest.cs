using System;
using Xunit;
using System.Net;
using System.Text.Json;

namespace TestMacroscop
{
    public class TestTimeFromJsonRequest
    {
        string link = "http://demo.macroscop.com:8080/command?type=gettime&login=root&password=&responsetype=json";

        [Fact]
        public void TestDifferenceTimeMore15Seconds()
        {
            //Arrange
            DateTime localTimeDate = DateTime.UtcNow.AddSeconds(16);

            //Act
            WebClient client = new();
            string request = client.DownloadString(link);
            int index = request.IndexOf("\r\n\r\n");
            string jsonText = request.Substring(index);
            JsonDocument json = JsonDocument.Parse(jsonText);
            DateTime serverTimeDate = DateTime.Parse(json.RootElement.ToString());
            TimeSpan time = serverTimeDate - localTimeDate;
            int differenceSeconds = Math.Abs(time.Days * 24 * 60 * 60 + time.Hours * 60 * 60 + time.Minutes * 60 + time.Seconds);

            //Assert
            Assert.True(differenceSeconds < 15, $"Разница между временем сервера и локальным = {differenceSeconds} секунд и {Math.Abs(time.Milliseconds)} миллисекунд");

        }

        [Fact]
        public void TestDifferenceTimeLess15Seconds()
        {
            //Arrange
            DateTime localTimeDate = DateTime.UtcNow.AddSeconds(13);

            //Act
            WebClient client = new();
            string request = client.DownloadString(link);
            int index = request.IndexOf("\r\n\r\n");
            string jsonText = request.Substring(index);
            JsonDocument json = JsonDocument.Parse(jsonText);
            DateTime serverTimeDate = DateTime.Parse(json.RootElement.ToString());
            TimeSpan time = serverTimeDate - localTimeDate;

            //Assert
            Assert.True(Math.Abs(time.Seconds) < 15, $"Разница между временем сервера и локальным = {Math.Abs(time.Seconds)} секунд и {Math.Abs(time.Milliseconds)} миллисекунд");

        }

    }
}
