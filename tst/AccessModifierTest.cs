using System.Reflection;
using Xunit;

namespace Inspector
{
    public class AccessModifierTest
    {
        [Fact]
        public void ElementsMatchFieldAttributes() {
            Assert.Equal((int)FieldAttributes.Private, (int)AccessModifier.Private);
            Assert.Equal((int)FieldAttributes.FamANDAssem, (int)AccessModifier.PrivateProtected);
            Assert.Equal((int)FieldAttributes.Assembly, (int)AccessModifier.Internal);
            Assert.Equal((int)FieldAttributes.Family, (int)AccessModifier.Protected);
            Assert.Equal((int)FieldAttributes.FamORAssem, (int)AccessModifier.ProtectedInternal);
            Assert.Equal((int)FieldAttributes.Public, (int)AccessModifier.Public);
        }

        [Fact]
        public void ElementsMatchMethodAttributes() {
            Assert.Equal((int)MethodAttributes.Private, (int)AccessModifier.Private);
            Assert.Equal((int)MethodAttributes.FamANDAssem, (int)AccessModifier.PrivateProtected);
            Assert.Equal((int)MethodAttributes.Assembly, (int)AccessModifier.Internal);
            Assert.Equal((int)MethodAttributes.Family, (int)AccessModifier.Protected);
            Assert.Equal((int)MethodAttributes.FamORAssem, (int)AccessModifier.ProtectedInternal);
            Assert.Equal((int)MethodAttributes.Public, (int)AccessModifier.Public);
        }
    }
}
