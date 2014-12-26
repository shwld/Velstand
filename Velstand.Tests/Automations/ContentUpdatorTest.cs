using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umbraco.Core.Models;
using Moq;
using Velstand.Automations;
using Velstand.Constants;

namespace Velstand.Tests.Automations
{
    [TestClass]
    public class ContentUpdatorTest
    {
        /*public TestContext TestContext { get; set; }
        [TestCase(null, new DateTime(2000, 1, 1).Date, new DateTime(2100, 1, 10).Date.ToString())]
        [TestCase(null, new DateTime(), new DateTime(2000, 1, 1).Date.ToString())]
        [TestCase(new DateTime(2000, 1, 1).Date, new DateTime(1990, 12, 1).Date, new DateTime(2000, 1, 10).Date.ToString())]*/

        [TestMethod]
        [TestCategory("#ReleaseDate")]
        public void ArgumentReleaseDateIsNull()
        {
            var result = new DateTime(2000, 1, 1).Date;
            var mock = this.mock(null, result, new DateTime(2100, 1, 10).Date.ToString());
            ContentCreator.ReleaseDate(mock.Object).Is(result);
        }

        [TestMethod]
        [TestCategory("#ReleaseDate")]
        public void ArgumentCreateDateIsDefault()
        {
            var result = new DateTime(2000, 1, 1).Date;
            var mock = this.mock(result, new DateTime(1990, 1, 1) ,new DateTime(2100, 1, 10).Date.ToString());
            ContentCreator.ReleaseDate(mock.Object).Is(result);
        }

        [TestMethod]
        [TestCategory("#ReleaseDate")]
        public void ArgumentNormal()
        {
            var result = new DateTime(2000, 1, 1).Date;
            var mock = this.mock(null, new DateTime() ,result.ToString());
            ContentCreator.ReleaseDate(mock.Object).Is(result);
        }

        [TestMethod]
        [TestCategory("#ReleaseDate")]
        public void ArgumentAllBlankOrDefault()
        {
            DateTime? releaseDate = null;
            var mock = new Mock<IContent>();
            mock.Setup(m => m.ReleaseDate).Returns(releaseDate);
            mock.Setup(m => m.CreateDate).Returns(new DateTime());
            mock.Setup(m => m.GetValue<string>(VelstandProperty.ReleaseDate)).Returns(string.Empty);

            ContentCreator.ReleaseDate(mock.Object).Date.Is(DateTime.Now.Date);
        }

        private Mock<IContent> mock(DateTime? releaseDate, DateTime createDate, string releaseDateProperty)
        {
                var mock = new Mock<IContent>();
                mock.Setup(m => m.ReleaseDate).Returns(releaseDate);
                mock.Setup(m => m.CreateDate).Returns(createDate);
                mock.Setup(m => m.GetValue<string>(VelstandProperty.ReleaseDate)).Returns(releaseDateProperty);

                return mock;
        }
    }
}
