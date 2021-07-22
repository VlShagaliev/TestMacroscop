using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Xunit;
using System.Xml;

namespace TestMacroscop
{
    public class TestStreetArchiveGetCadr
    {
        public List<String> SearchChannelId(String link)
        {
            List<String> channelIdList = new();
            bool findChannelId = false;
            XmlReader xmlReader = XmlReader.Create(link);
            while (xmlReader.Read() && !findChannelId)
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) &&
                    (xmlReader.Name.Equals("SecObjectInfo") && (xmlReader.HasAttributes)))
                {
                    while (xmlReader.MoveToNextAttribute() && !findChannelId)
                    {
                        if (xmlReader.Name == "Name" && xmlReader.Value == "Street")
                        {
                            while (xmlReader.Read())
                            {
                                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("ChannelId"))
                                {
                                    channelIdList.Add(xmlReader.ReadString());
                                }
                            }

                            findChannelId = true;
                        }
                    }
                }

            }

            return channelIdList;
        }
        
        [Fact]
        public void TestGetCadrTrue()
        {
            //Arrange
            bool findChannelId = false;
            List<String> channelIdList = new();
            String linkServerInfo = "http://demo.macroscop.com:8080/configex?login=root&password=";
            channelIdList = SearchChannelId(linkServerInfo);
            
            //Act
            bool checkjpeg = true;
            foreach (var VARIABLE in channelIdList)
            {
                WebClient client = new();
                DateTime localDateTime = DateTime.UtcNow;
                CultureInfo culture = new CultureInfo("ru-RU");
                string link = "http://demo.macroscop.com/site?login=root&channelid=" + VARIABLE +
                              "&withcontenttype=true&mode=archive&resolutionx=500&resolutiony=500&streamtype=mainvideo&starttime=" +
                              localDateTime.ToString(culture);
                string imageInfo = client.DownloadString(link).ToLower();
                int indexjpeg = imageInfo.IndexOf("jpeg");
                Console.WriteLine(indexjpeg);
                if (indexjpeg == -1)
                {
                    checkjpeg = false;
                }
            }
            //Assert
            Assert.True(checkjpeg, "Среди каналов не было получено кадра");
        }

    }
}