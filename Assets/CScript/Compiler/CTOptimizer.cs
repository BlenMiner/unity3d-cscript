﻿using System.Collections.Generic;
using Riten.CScript.Native;
using UnityEngine;

namespace Riten.CScript.Compiler
{
    public static class CTOptimizer
    {
        static readonly CTOptimizerRule[] s_rules = {
            new (OptimizePushConst, Opcodes.PUSH_CONST, Opcodes.ADD),
            new (OptimizePushConstAddConst, Opcodes.PUSH_CONST, Opcodes.ADD_CONST),
            new (OptimizePushConstToSPTR, Opcodes.PUSH_CONST, Opcodes.POP_TO_SPTR),
            new (OptimizeCopyFromSPRTToSPTR, Opcodes.PUSH_FROM_SPTR, Opcodes.POP_TO_SPTR)
        };
        
        static void OffsetAllPointers(CTCompiler cmp, int pastValue, int offset)
        {
            foreach (var scope in cmp.AllScopes)
            {
                foreach (var fnc in scope.Functions)
                {
                    if (fnc.Value.FunctionPtr >= pastValue)
                        fnc.Value.FunctionPtr += offset;
                }
            }
            
            /*for (int i = 0; i < cmp.Instructions.Count; i++)
            {
                if (cmp.Instructions[i].Opcode == (int)Opcodes.JMP)
                {
                    if (cmp.Instructions[i].Operand >= pastValue)
                        cmp.Instructions[i].Operand += offset;
                }
            }*/
        }
        
        static void OptimizeCopyFromSPRTToSPTR(CTCompiler cmp, List<Instruction> program, int index)
        {
            long from = program[index].Operand;
            long to = program[index + 1].Operand;
            
            program.RemoveRange(index, 2);
            program.Insert(index, new Instruction(Opcodes.COPY_FROM_SPTR_TO_SPTR, from, to));
            
            OffsetAllPointers(cmp, index, -1);
        }
        
        static void OptimizePushConstToSPTR(CTCompiler cmp,List<Instruction> program, int index)
        {
            long value = program[index].Operand;
            long ptr = program[index + 1].Operand;
            program.RemoveRange(index, 2);
            program.Insert(index, new Instruction(Opcodes.PUSH_CONST_TO_SPTR, value, ptr));
            
            OffsetAllPointers(cmp, index, -1);
        }

        static void OptimizePushConst(CTCompiler cmp, List<Instruction> program, int index)
        {
            long value = program[index].Operand;
            program.RemoveRange(index, 2);
            program.Insert(index, new Instruction(Opcodes.ADD_CONST, value));
            
            OffsetAllPointers(cmp, index, -1);
        }
        
        static void OptimizePushConstAddConst(CTCompiler cmp, List<Instruction> program, int index)
        {
            long valueA = program[index].Operand;
            long valueB = program[index + 1].Operand;
            
            program.RemoveRange(index, 2);
            program.Insert(index, new Instruction(Opcodes.PUSH_CONST, valueA + valueB));
            
            OffsetAllPointers(cmp, index, -1);
        }

        public static void Optimize(CTCompiler compiler, List<Instruction> instruction)
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
                            rule.Apply(compiler, instruction, i);
                            ++matchCount;
                            --i;
                            break;
                        }
                    }
                }
                
                if (matchCount == 0) break;

                ++whileCount;

                if (whileCount > 10000)
                {
                    Debug.LogError("Optimize loop count exceeded");
                    break;
                }
            }
        }
    }
}