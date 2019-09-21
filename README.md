[![Build](https://img.shields.io/appveyor/ci/olegsych/inspector/master)](https://ci.appveyor.com/project/olegsych/inspector/branch/master)
[![Tests](https://img.shields.io/appveyor/tests/olegsych/inspector/master)](https://ci.appveyor.com/project/olegsych/inspector/branch/master/tests)
[![Nuget](https://img.shields.io/nuget/v/inspector.svg)](https://www.nuget.org/packages/inspector)

# what and why

Inspector is a simple .NET Reflection API for white-box unit testing.

Why use white-box testing? Because building fully-tested .NET code shouldn't require breaking of encapsulation
and _unnecessary_ exposure of internal types as public, wrapping of well-defined external dependencies
or resorting to integration testing.

# install

Add the [inspector](https://www.nuget.org/packages/inspector) package to your .NET project.
```
dotnet add package inspector
```

# import

Import the `Inspector` namespace in your .NET source file.
Most of the Inspector APIs are extension methods of the .NET `Object` and `Type`.
```
using Inspector;
```

# use

Suppose you have the following class that serves as a base for a number of derived types in your system.
```C#
public class MyClass
{
    protected readonly int field;

    protected MyClass(int parameter) {
        if (parameter < 42)
            throw new ArgumentOutOfRangeException(nameof(parameter));
        field = parameter;
    }
}
```

Because the class members are meant to be used only by the derived types, its members have protected
visibility and aren't directly accessible from unit tests. Testing this class can be done through its
derived classes at the cost of duplicating the tests of the base class for each derived type.
Alternatively, you could make members of this class `internal` and use the `[assembly:InternalsVisibleTo()]`
attribute to allow your unit tests access them directly at the cost of breaking the encapsulation and
making the class more accessible than intended. Finally, you could derive a special class that provides
public API for accessing the protected members of its base in your unit tests. While this last option
may be trivial, it is also wasteful and verbose.

With Inspector, you can create a new instance of a class with non-public constructor without redundant typecasting.
```C#
MyClass sut = Type<MyClass>.New(42);
```

You can use Inspector's strongly-typed extension methods to access non-public members.
```C#
int fieldValue = sut.Field<string>();
Assert.Equal("foo", fieldValue);
```

You can verify that exceptions thrown by your class implement the expected contract.
```C#
var thrown = Assert.Throws<ArgumentOutOfRangeException>(() => Type<MyClass>.New(41));
Assert.Equal(sut.Constructor().Parameter().Name, thrown.ParamName);
```

Now suppose that `MyClass` is an external dependency in your code. Pretend that instead of a
simple `int` parameter, it's constructor requires a second-level dependency with complex setup that
would not only be difficult implement, it would also break the [Law of Demeter](https://en.wikipedia.org/wiki/Law_of_Demeter)
and make your unit tests fragile. To deal with this problem, the conventional wisdom requires introduction
of a new, testable abstraction in your code to encapsulate the external dependency and allow testing
your code by mocking or stubbing your own abstraction instead of the dependency. While this option is
straightforward, it could also be wasteful if the external dependency has a well-defined and stable API.

With Inspector, you can create an instance of this class without invoking its constructor and then manipulate
its non-public members to prepare conditions expected by your code.
```C#
MyClass dependency = Type<MyClass>.Uninitialized();
set.Field<int>().Set(41);
```
