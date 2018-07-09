using System;
using System.Reflection;

namespace Inspector
{
    // Tested through Method<T> and Constructor<T>
    static class IDelegateFactoryExtensions
    {
        internal static bool TryCreate<TMethodBase, TDelegate>(this IDelegateFactory<TMethodBase> delegateFactory, object target, TMethodBase method, out TDelegate @delegate)
            where TMethodBase : MethodBase
            where TDelegate : Delegate
        {
            if(delegateFactory == null)
                throw new ArgumentNullException(nameof(delegateFactory));

            Delegate d;
            bool created = delegateFactory.TryCreate(typeof(TDelegate), target, method, out d);
            @delegate = (TDelegate)d;
            return created;
        }
    }
}
