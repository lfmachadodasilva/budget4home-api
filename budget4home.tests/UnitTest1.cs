using System;
using Xunit;
using budget4home.Models;
using FluentAssertions;

namespace budget4home.tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            LabelModel l = new LabelModel();
            l.Should().BeOfType<LabelModel>();
        }
    }
}
