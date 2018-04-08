using System;

namespace Inspector
{
    public static class IScopeExtensions
    {
        #region Accessibility

        public static IScope Internal(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Internal);

        public static IScope Private(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Private);

        public static IScope Protected(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Protected);

        public static IScope Public(this IScope scope) =>
            new AccessibilityScope(scope, Accessibility.Public);

        #endregion
    }
}
