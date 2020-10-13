Blinq
=====

Burst Compatible, deferred, stack-allocated LINQ extensions for `NativeArray`.

Installation
------------

This project can be installed as a UPM package. The easiest way to install it right now is using the [OpenUPM](https://openupm.com/packages/com.careboo.blinq/).

Currently, support for the Github Package Registry is broken. See this thread [here](https://forum.unity.com/threads/unable-to-publish-upm-packages-to-github-package-repo-as-of-07-10-2020.985268/#post-6409311) for more information.

Differences with Linq
---------------------

Blinq aims to be as similar to Linq as possible, but there are some major differences.

### Delegates

The Burst compiler doesn't support C# delegates. To get around this issue, Blinq requires you to create structs that implement the `IFunc` interface.

```cs
/*--- Using Linq ---*/
var selected = myArray.Select(val => val.Item);

/*--- Using Blinq ---*/

// Must define a struct that implements IFunc first...
public struct MySelector : IFunc<MyVal, int>
{
    public int Invoke(MyVal val) => val.Item;
}

// Then we have to create a ValueFunc referencing our struct
var selector = ValueFunc<MyVal, int>.CreateImpl<MySelector>();

// Now we can finally call ``Select``
var selected = myArray.Select(selector);
```

Current work is being made to allow burstable lambdas in the [Burst.Delegates](https://github.com/CareBoo/Burst.Delegates) project (WIP).

### GroupBy

`GroupBy` API returning `IGrouping` isn't supported at the moment.
