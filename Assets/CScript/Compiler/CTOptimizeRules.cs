using System;
using System.Collections.Generic;
using Riten.CScript.Native;

namespace Riten.CScript.Runtime
{
    public readonly struct CTOptimizerRule
    {
        public readonly Opcodes[] Pattern;

        readonly Action<List<Instruction>, int> Resolver;
            
        public CTOptimizerRule(Action<List<Instruction>, int> resolver, params Opcodes[] pattern)
        {
            Pattern = pattern;
            Resolver = resolver;
        }

        public void Apply(List<Instruction> program, int index)
        {
            Resolver(program, index);
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