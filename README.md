Blinq
=====

Burst Compatible, deferred, stack-allocated LINQ extensions for `NativeArray`.

Installation
------------

This project can be installed as a UPM package. The easiest way to install it right now is using the [OpenUPM](https://openupm.com/packages/com.careboo.blinq/).

Currently, support for the Github Package Registry is broken. See this thread [here](https://forum.unity.com/threads/unable-to-publish-upm-packages-to-github-package-repo-as-of-07-10-2020.985268/#post-6409311) for more information.

Differences with Linq
---------------------

### Delegates

The Burst compiler doesn't support C# delegates. To get around this issue, Blinq requires you to create structs that implement the `IFunc` interface. The [Burst.Delegates](https://github.com/CareBoo/Burst.Delegates) project has other useful tools to help you implement the `IFunc` interface.

```cs
/*--- Using Linq ---*/
var selected = myArray.Select(val => val.Item);

/*--- Using Blinq ---*/

// Must define a method that can be used as FunctionPointer
[BurstCompile]
public static int SelectItem(MyVal val) => val.Item

public static readonly BurstFunc<MyVal, int> SelectItemFunc = BustFunc.Compile(SelectItem);

// Now we can finally call ``Select``
var selected = myArray.Select(SelectItemFunc);
```

