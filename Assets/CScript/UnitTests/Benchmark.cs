using UnityEditor;
using UnityEngine;

namespace CScript.UnitTests
{
    public static class Benchmark
    {
        [MenuItem("CScript/Benchmark/Compare Simple For Loop")]
        public static void CompareForLoop()
        {
            var program = TestsHelper.GetProgram(out var code);
            
            const int LOOP_COUNT = 10000;
            
            code.Add(Opcodes.PUSH, 0);
            code.Add(Opcodes.MOVE_TO_REGISTER, 0); // Move pushed int to R0 leaving stack empty
            code.Add(Opcodes.PUSH, LOOP_COUNT);
            code.Add(Opcodes.DUPLICATE);
            code.Add(Opcodes.PUSH, 0);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.JMP_IF_TRUE, 8); // Out of range = exit
            code.Add(Opcodes.PUSH, 1);
            code.Add(Opcodes.SUB_INT);
            code.Add(Opcodes.LOAD_REGISTER, 0);
            code.Add(Opcodes.PUSH, 2);
            code.Add(Opcodes.ADD);
            code.Add(Opcodes.MOVE_TO_REGISTER, 0);
            code.Add(Opcodes.JMP, -10);
            code.Add(Opcodes.POP);
            code.Add(Opcodes.LOAD_REGISTER, 0);
            code.Add(Opcodes.POP);

            var watch = new System.Diagnostics.Stopwatch();

            watch.Reset();
            watch.Start();
            program.Run();
            watch.Stop();

            Debug.Log($"CScript, res: {(int)program.Stack.Operand}, time: {watch.ElapsedMilliseconds} ms");

            watch.Reset();
            watch.Start();

            int acc = 0;
            for (var i = 0; i < LOOP_COUNT; i++)
            {
                acc += 2;
            }
            watch.Stop();

            Debug.Log($"C# code, res: {acc}, time: {watch.ElapsedMilliseconds} ms");
        }
    }
}