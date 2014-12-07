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
        [TestMethod]
        public void ReleaseDateTest_Normal()
        {
            var actual = DateTime.Now.AddDays(10);
            IContent expect = TestHelper.IContentMock(
                                               releaseDate: actual, 
                                               createDate: DateTime.Now.AddDays(-10), 
                                               releaseDateProperty: DateTime.Now.AddDays(-20).ToString()
                                           );

            Assert.AreEqual(ContentCreator.ReleaseDate(expect), actual);
        }

        [TestMethod]
        public void ReleaseDateTest_IsNull()
        {
            var actual = DateTime.Now.AddDays(10);
            IContent expect = TestHelper.IContentMock(
                                               releaseDate: null,
                                               createDate: actual,
                                               releaseDateProperty: DateTime.Now.AddDays(-20).ToString()
                                           );

            Assert.AreEqual(ContentCreator.ReleaseDate(expect), actual);
        }

        [TestMethod]
        public void ReleaseDateTest_IsDefaultDate()
        {
            var actual = DateTime.Now.AddDays(10);
            IContent expect = TestHelper.IContentMock(
                                               releaseDate: null,
                                               createDate: new DateTime(),
                                               releaseDateProperty: actual.ToString()
                                           );

            Assert.AreEqual(ContentCreator.ReleaseDate(expect).Day, actual.Day);
        }

        [TestMethod]
        public void ReleaseDateTest_IsDefaultDate2()
        {
            var actual = DateTime.Now;
            IContent expect = TestHelper.IContentMock(
                                               releaseDate: null,
                                               createDate: new DateTime(),
                                               releaseDateProperty: string.Empty
                                           );

            Assert.AreEqual(ContentCreator.ReleaseDate(expect).Day, actual.Day);
        }
    }
}
