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
        
        public static long GetValueBits(string typeName, string value, bool negateResult)
        {
            var internalType = ResolveType(typeName);
            
            if (negateResult)
                value = $"-{value}";

            value = value.Replace("--", "").Replace("++", "");

            long type;
            
            try
            {
                type = internalType switch
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
                    _ => throw new NotImplementedException()
                };
            }
            catch
            {
                throw new CTLexerException((CToken)default, $"Couldn't parse value '{value}' as type '{typeName}'");
            }

            return type;
        }
        
        public static int GetBuiltinSize(string typeName)
        {
            var internalType = ResolveType(typeName);
            
            int size;
            
            try
            {
                size = internalType switch
                {
                    InternalType.I8 => sizeof(sbyte),
                    InternalType.I16 => sizeof(short),
                    InternalType.I32 => sizeof(int),
                    InternalType.I64 => sizeof(long),
                    InternalType.U8 => sizeof(byte),
                    InternalType.U16 => sizeof(ushort),
                    InternalType.U32 => sizeof(uint),
                    InternalType.U64 => sizeof(ulong),
                    InternalType.F64 => sizeof(double),
                    InternalType.F32 => sizeof(float),
                    _ => throw new NotImplementedException()
                };
            }
            catch
            {
                throw new CTLexerException((CToken)default, $"Couldn't get sizeof builtin type: '{typeName}'");
            }

            return size;
        }
        
        public static void CompilePush(CTCompiler compiler, string type, string value, bool negate)
        {
            var internalType = ResolveType(type);

            if (internalType != InternalType.CustomType)
            {
                var opcode = internalType.InternalTypeToPushOpcode();
                var valueBits = GetValueBits(type, value, negate);
                compiler.Instructions.Add(new Instruction(opcode, valueBits));
                return;
            }
            
            throw new NotImplementedException($"Can't push '{type}' since it's not internal type");
        }
        
        public static void CompilePopToSPTR(CTCompiler compiler, string type, int offset)
        {
            var internalType = ResolveType(type);

            if (internalType != InternalType.CustomType)
            {
                var opcode = internalType.InternalTypeToPopToSPTR();
                compiler.Instructions.Add(new Instruction(opcode, offset));
                return;
            }
            
            throw new NotImplementedException($"Can't pop to SPTR '{type}' since it's not internal type");
        }
        
        public static void CompilePushSPTR(CTCompiler compiler, string type, int offset)
        {
            var internalType = ResolveType(type);

            if (internalType != InternalType.CustomType)
            {
                var opcode = internalType.InternalTypeToPushSPTROpcode();
                compiler.Instructions.Add(new Instruction(opcode, offset));
                return;
            }
            
            throw new NotImplementedException($"Can't push from SPTR as '{type}' since it's not internal type");
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
        
        public static void CompileMinus(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToSubOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"Substraction of {typeA} and {typeB} not implemented");
        }
        
        public static void CompileLessOrEqual(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToLessOrEqualOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"Substraction of {typeA} and {typeB} not implemented");
        }

        public static void CompileMult(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToMultOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"Multiplication of {typeA} and {typeB} not implemented");
        }
        
        public static void CompileDiv(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToDivOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"Division of {typeA} and {typeB} not implemented");
        }
        
        public static void CompileMod(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToModuloOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"Modulo of {typeA} and {typeB} not implemented");
        }
        
        public static void CompileAnd(CTCompiler compiler, string typeA, string typeB)
        {
            var internalTypeA = ResolveType(typeA);
            var internalTypeB = ResolveType(typeB);

            if (internalTypeA == internalTypeB && internalTypeA != InternalType.CustomType)
            {
                var opcode = internalTypeA.InternalTypeToBitAndOpcode();
                compiler.Instructions.Add(new Instruction(opcode));
                return;
            }
            
            throw new NotImplementedException($"And of {typeA} and {typeB} not implemented");
        }
    }
}