using System;
using System.Collections.Generic;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public readonly struct CTOptimizerRule
    {
        public readonly Opcodes[] Pattern;

        readonly Action<CTCompiler, List<Instruction>, int> Resolver;
            
        public CTOptimizerRule(Action<CTCompiler, List<Instruction>, int> resolver, params Opcodes[] pattern)
        {
            Pattern = pattern;
            Resolver = resolver;
        }

        public void Apply(CTCompiler compiler, List<Instruction> program, int index)
        {
            Resolver(compiler, program, index);
        }
        
        public static bool Match(IReadOnlyList<Instruction> program, int index, params Opcodes[] pattern)
        {
            if (index + pattern.Length > program.Count)
                return false;

            for (int i = 0; i < pattern.Length; i++)
            {
                if (program[index + i].Opcode != (int)pattern[i])
                    return false;
            }

            return true;
        }
    }
}