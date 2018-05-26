using System;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ParameterInfoMatcherTest
    {
        public class Match
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenFirstParameterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => ParameterInfoMatcher.Match(null, Substitute.For<ParameterInfo>()));
                Assert.Equal("p1", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenSecondParameterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => ParameterInfoMatcher.Match(Substitute.For<ParameterInfo>(), null));
                Assert.Equal("p2", thrown.ParamName);
            }

            class P1 { }
            class P1User
            {
                public void Use(P1 p) { }
            }

            class P1User2
            {
                public void Use(P1 p) { }
            }

            [Fact]
            public void ReturnsTrueWhenParameterTypesAreSame() {
                ParameterInfo p1 = typeof(P1User).GetMethod(nameof(P1User.Use)).GetParameters().Single();
                ParameterInfo p2 = typeof(P1User2).GetMethod(nameof(P1User2.Use)).GetParameters().Single();
                Assert.True(ParameterInfoMatcher.Match(p1, p2));
            }

            class P2 { }
            class P2User
            {
                public void Use(P2 p) { }
            }

            [Fact]
            public void ReturnsFalseWhenParameterTypesAreDifferent() {
                ParameterInfo p1 = typeof(P1User).GetMethod(nameof(P1User.Use)).GetParameters().Single();
                ParameterInfo p2 = typeof(P2User).GetMethod(nameof(P2User.Use)).GetParameters().Single();
                Assert.False(ParameterInfoMatcher.Match(p1, p2));
            }

            class P1InUser
            {
                public void Use(in P1 p) { }
            }

            [Fact]
            public void ReturnsFalseWhenInAttributeDoesntMatch() {
                ParameterInfo p1 = typeof(P1User).GetMethod(nameof(P1User.Use)).GetParameters().Single();
                ParameterInfo p2 = typeof(P1InUser).GetMethod(nameof(P1InUser.Use)).GetParameters().Single();
                Assert.False(ParameterInfoMatcher.Match(p1, p2));
            }

            class P1OutUser
            {
                public void Use(out P1 p) { p = default; }
            }

            [Fact]
            public void ReturnsFalseWhenOutAttributeDoesntMatch() {
                ParameterInfo p1 = typeof(P1User).GetMethod(nameof(P1User.Use)).GetParameters().Single();
                ParameterInfo p2 = typeof(P1OutUser).GetMethod(nameof(P1OutUser.Use)).GetParameters().Single();
                Assert.False(ParameterInfoMatcher.Match(p1, p2));
            }

            static ParameterInfo Parameter(Delegate d) =>
                d.Method.GetParameters().Single();
        }
    }
}
