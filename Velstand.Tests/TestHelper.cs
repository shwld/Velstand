using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Moq;
using Velstand.Constants;

namespace Velstand.Tests
{
    public static class TestHelper
    {
        public static IContent IContentMock(
            DateTime? releaseDate, 
            DateTime createDate, 
            string releaseDateProperty)
        {
            var mock = new Mock<IContent>();
            mock.Setup(m => m.ReleaseDate).Returns(releaseDate);
            mock.Setup(m => m.CreateDate).Returns(createDate);
            mock.Setup(m => m.GetValue<string>(VelstandProperty.ReleaseDate)).Returns(releaseDateProperty);
            return mock.Object;
        }
    }
}
