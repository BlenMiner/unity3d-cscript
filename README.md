# unity3d-cscript

This language will be capable of interpretation (compiled to opcodes and then interpreted) with a future vision to allow transpiling to native C++ (IL2CPP platform) or C#.
Main motivation is to avoid long compile times, allow for easy multi-threading in WebGL environment, potentially better performance than C# with native C++ transpiling for IL2CPP platforms (not in the managed world),
ease the process of modding even for exclusive AOT platforms (like WebGL) and overall just making working with Unity less of a pain.

# Painpoints/Goals

- Faster Compile Times: One of the main aims is to reduce compile times, especially when tinkering within Unity.
- WebGL Multi-threading: I strongly believe that WebGL and potentially WebGPU in the future will be the predominant platforms to deliver experiences (this might take a long time to happen but I feel like it's where games will converge slowly as the performance there starts to match native performance).
- Game Modding: The interpreter plays a pivotal role here. It's intended to simplify game modding, even for platforms that work ahead of time like WebGL.
- Performance: Given that we're delving into games, optimal performance remains a top priority.
- Fun: Lastly, this project isn't just about technicalities. Having fun and enjoying the process is integral to the development.

# Other Plans

- Interoperability: I want to ensure seamless interaction between CScript and C#.
- Transpilation: The idea is to transpile CScript to either C++ or C# to leverage performance gains.

# Biggest Challenges

- Multi-threading: Managing parallel execution and ensuring safety.
- Garbage Collection: Efficient memory management to keep performance optimal.

# Current Architecture

The interpreter is crafted in C++.
The "compiler" is written in C#. Here, by "compiler", I'm referring to a tool designed for a VM, not a genuine native executable. This is the core of the interpretation process.
From a code design perspective, I commenced with a C-style foundation. The plan is to keep evolving and adapting based on the needs and challenges that arise.

# Example

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
