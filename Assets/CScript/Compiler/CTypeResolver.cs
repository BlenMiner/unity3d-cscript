using System;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public static class CTypeResolver
    {
        static InternalType ResolveType(string typeName)
        {
            switch (typeName)
            {
                case "i8": return InternalType.I8;
                case "i16": return InternalType.I16;
                case "i32": return InternalType.I32;
                case "i64": return InternalType.I64;
                
                case "u8": return InternalType.U8;
                case "u16": return InternalType.U16;
                case "u32": return InternalType.U32;
                case "u64": return InternalType.U64;
                
                case "f32": return InternalType.F32;
                case "f64": return InternalType.F64;
                
                default: return InternalType.CustomType;
            }
        }
        
        public static long GetValueBits(string typeName, string value)
        {
            var internalType = ResolveType(typeName);

            return internalType switch
            {
                InternalType.I8 => sbyte.Parse(value),
                InternalType.I16 => short.Parse(value),
                InternalType.I32 => int.Parse(value),
                InternalType.I64 => long.Parse(value),
                InternalType.U8 => byte.Parse(value),
                InternalType.U16 => ushort.Parse(value),
                InternalType.U32 => uint.Parse(value),
                InternalType.U64 => BitConverter.ToInt64(BitConverter.GetBytes(ulong.Parse(value))),
                InternalType.F64 => BitConverter.DoubleToInt64Bits(double.Parse(value)),
                InternalType.F32 => BitConverter.SingleToInt32Bits(float.Parse(value)),
                _ => throw new NotImplementedException($"Cannot get value bits for type {typeName}")
            };
        }
        
        public static void CompileAdd(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToAddOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"Addition of {typeA} and {typeB} not implemented");
        }
    }
}