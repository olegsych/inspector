using System.Reflection;
using Xunit;

namespace Inspector
{
    public class AccessibilityTest
    {
        [Fact]
        public void ElementsMatchFieldAttributes() {
            Assert.Equal((int)FieldAttributes.Private, (int)Accessibility.Private);
            Assert.Equal((int)FieldAttributes.FamANDAssem, (int)Accessibility.PrivateProtected);
            Assert.Equal((int)FieldAttributes.Assembly, (int)Accessibility.Internal);
            Assert.Equal((int)FieldAttributes.Family, (int)Accessibility.Protected);
            Assert.Equal((int)FieldAttributes.FamORAssem, (int)Accessibility.ProtectedInternal);
            Assert.Equal((int)FieldAttributes.Public, (int)Accessibility.Public);
        }

        [Fact]
        public void ElementsMatchMethodAttributes() {
            Assert.Equal((int)MethodAttributes.Private, (int)Accessibility.Private);
            Assert.Equal((int)MethodAttributes.FamANDAssem, (int)Accessibility.PrivateProtected);
            Assert.Equal((int)MethodAttributes.Assembly, (int)Accessibility.Internal);
            Assert.Equal((int)MethodAttributes.Family, (int)Accessibility.Protected);
            Assert.Equal((int)MethodAttributes.FamORAssem, (int)Accessibility.ProtectedInternal);
            Assert.Equal((int)MethodAttributes.Public, (int)Accessibility.Public);
        }
    }
}
