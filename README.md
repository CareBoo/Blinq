Blinq
=====

Burst-Compatible, deferred, stack-allocated LINQ extensions for `NativeArray`.

Installation
------------

This project can be installed as a UPM package. The easiest way to install it right now is using the [GitHub Package Registry](https://forum.unity.com/threads/using-github-packages-registry-with-unity-package-manager.861076/). Support for OpenUPM will be available in the future, so check back often!

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

### GroupBy

Blinq is using `NativeList` as an interface for executing queries, so there isn't a safe way to return lists of lists, which affects a subset of the `GroupBy` API. The TL;DR is that the `GroupBy` API that returns `IGrouping` isn't supported at the moment.

Future Updates
--------------

- Lambda support
- Full GroupBy API
- Parallel support
- Executing queries using Job API
- Performance Tests for all APIs
