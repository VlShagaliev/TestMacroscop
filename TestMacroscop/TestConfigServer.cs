using Xunit;
using System.Xml;

namespace TestMacroscop
{
   public class TestConfigServer
    {
        [Fact]

        public void ConfigServerCheck()
        {
            //Arrange
            int quantityChannel = 0;

            //Act
            XmlReader xmlReader = XmlReader.Create("http://demo.macroscop.com:8080/configex?login=root&password="); 
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name.Equals("ChannelInfo")))
                {
                    quantityChannel++;
                }
            }

            //Assert
            Assert.True(quantityChannel >= 6, $"Количество каналов = {quantityChannel}");


        }
    }
}
