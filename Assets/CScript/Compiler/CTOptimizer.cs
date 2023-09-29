using System.Collections.Generic;
using Riten.CScript.Native;

namespace Riten.CScript.Runtime
{
    public static class CTOptimizer
    {
        static readonly CTOptimizerRule[] s_rules = {
            new (OptimizePushConst, Opcodes.PUSH_CONST, Opcodes.ADD),
            new (OptimizePushConstAddConst, Opcodes.PUSH_CONST, Opcodes.ADD_CONST)
        };

        static void OptimizePushConst(List<Instruction> program, int index)
        {
            long value = program[index].Operand;
            program.RemoveRange(index, 2);
            program.Insert(index, new Instruction(Opcodes.ADD_CONST, value));
        }
        
        static void OptimizePushConstAddConst(List<Instruction> program, int index)
        {
            long valueA = program[index].Operand;
            long valueB = program[index + 1].Operand;
            
            program.RemoveRange(index, 2);
            program.Insert(index, new Instruction(Opcodes.PUSH_CONST, valueA + valueB));
        }

        public static void Optimize(List<Instruction> instruction)
        {
            int whileCount = 0;
            while (true)
            {
                int matchCount = 0;
                for (int i = 0; i < instruction.Count; i++)
                {
                    for (var j = 0; j < s_rules.Length; j++)
                    {
                        var rule = s_rules[j];
                        if (CTOptimizerRule.Match(instruction, i, rule.Pattern))
                        {
                            rule.Apply(instruction, i);
                            ++matchCount;
                            --i;
                            break;
                        }
                    }
                }
                
                if (matchCount == 0) break;

                ++whileCount;

                if (whileCount > 10000)
                    break;
            }
        }
    }
}