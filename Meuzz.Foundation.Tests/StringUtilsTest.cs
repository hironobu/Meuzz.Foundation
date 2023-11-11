using System;
using Xunit;
using Meuzz.Foundation;

namespace Meuzz.Foundation.Tests
{
    public class StringUtilsTest
    {
        [Fact]
        public void TestSnake2Camel()
        {
            Assert.Equal("fooBar", "foo_bar".ToCamel());
            Assert.Equal("FooBar", "foo_bar".ToCamel(true));
            Assert.Equal("fooBar", "FOO_BAR".ToCamel());
            Assert.Equal("FooBar", "FOO_BAR".ToCamel(true));

            Assert.Equal("aFoo", "a_foo".ToCamel());
            Assert.Equal("AFoo", "a_foo".ToCamel(true));

            Assert.Equal("aBC", "a_b_c".ToCamel());
            Assert.Equal("ABC", "a_b_c".ToCamel(true));

            // without "_"
            Assert.Equal("abc", "abc".ToCamel());
            Assert.Equal("Abc", "abc".ToCamel(true));
            Assert.Equal("fooBar", "fooBar".ToCamel());
            Assert.Equal("FooBar", "fooBar".ToCamel(true));

            Assert.Equal("fooBar", "foo__bar".ToCamel());
            Assert.Equal("FooBar", "foo__bar".ToCamel(true));
            Assert.Equal("fooBar", "FOO__BAR".ToCamel());
            Assert.Equal("FooBar", "FOO__BAR".ToCamel(true));

            Assert.Equal("aaBbCc", "aa___bb__cc".ToCamel());
            Assert.Equal("AaBbCc", "aa___bb__cc".ToCamel(true));

            Assert.Equal("", "".ToCamel());
            Assert.Equal("", "".ToCamel(true));

            //var ex = Assert.Throws<ArgumentNullException>(() => StringUtils.ToCamel(null));
            //Assert.Contains("Parameter 's'", ex.Message);
        }

        [Fact]
        public void TestCamel2Snake()
        {
            Assert.Equal("foo_bar", "fooBar".ToSnake());
            Assert.Equal("foo_bar", "FooBar".ToSnake());
            Assert.Equal("FOO_BAR", "fooBar".ToSnake(true));
            Assert.Equal("FOO_BAR", "FooBar".ToSnake(true));

            Assert.Equal("a_b_c", "ABC".ToSnake());
            Assert.Equal("A_B_C", "ABC".ToSnake(true));

            Assert.Equal("", "".ToSnake());
            Assert.Equal("", "".ToSnake(true));

            //var ex = Assert.Throws<ArgumentNullException>(() => StringUtils.ToSnake(null));
            //Assert.Contains("Parameter 's'", ex.Message);
        }
    }
}
