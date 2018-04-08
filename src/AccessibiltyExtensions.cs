using System;

namespace Inspector
{
    public static class AccessibiltyExtensions
    {
        #region IScope

        public static IScope Internal(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Internal);

        public static IScope Private(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Private);

        public static IScope Protected(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Protected);

        public static IScope Public(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Public);

        #endregion

        #region Object

        public static IScope Internal(this object instance) =>
            new InstanceScope(instance).Internal();

        public static IScope Private(this object instance) =>
            new InstanceScope(instance).Private();

        public static IScope Protected(this object instance) =>
            new InstanceScope(instance).Protected();

        public static IScope Public(this object instance) =>
            new InstanceScope(instance).Public();

        #endregion

        #region Type

        public static IScope Internal(this Type type) =>
            new StaticScope(type).Internal();

        public static IScope Private(this Type type) =>
            new StaticScope(type).Private();

        public static IScope Protected(this Type type) =>
            new StaticScope(type).Protected();

        public static IScope Public(this Type type) =>
            new StaticScope(type).Public();

        #endregion
    }
}
