using Inomatic.HalLucinate;
using System;
using Xunit;

namespace HalLucinate.UnitTests
{
    public class LinkTests
    {
        private readonly Link sut;

        public LinkTests()
        {
            sut = new("/");
        }

        [Fact]
        public void Should_SetHref_When_Constructing()
        {
            Assert.Equal("/", sut.Href);
        }

        [Fact]
        public void Should_ReturnRelativeUri_When_AsUri()
        {
            var uri = sut.AsUri();

            Assert.False(uri.IsAbsoluteUri);
        }

        [Fact]
        public void Should_ConvertImplicitFromString()
        {
            Link link = "/";

            Assert.Equal("/", link.Href);
        }
    }
}