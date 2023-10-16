# unity3d-cscript
This language will be capable of interpretation (compiled to opcodes and then interpreted) with a future vision to allow transpiling to native C++ (IL2CPP platform) or C#.
Main motivation is to avoid long compile times, allow for easy multi-threading in WebGL environment, potentially better performance than C# with native C++ transpiling for IL2CPP platforms (not in the managed world),
ease the process of modding even for exclusive AOT platforms (like WebGL) and overall just making working with Unity less of a pain.

Example of currently valid code (this is bound to change):

``` C#
long fib(long n)
{
    if (n <= 1)
    {
        return n;
    }
 
    return fib(n - 1) + fib(n - 2);
}
 
long test()
{
    return fib(30);
}
```
