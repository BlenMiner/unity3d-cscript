using System;

namespace Riten.CScript.Native
{
    public enum InternalType
    {
        CustomType,
        I8, U8,
        I16, U16,
        I32, U32,
        I64, U64,
        F32, F64,
    }

    public static class InternalTypeUtils
    {
        public static readonly string[] INTERNAL_TYPES =
        {
            "i8", "i16", "i32", "i64",
            "u8", "u16", "u32", "u64",
            "f32", "f64"
        };
        
        public static Opcodes InternalTypeToAddOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.ADD_I8,
                InternalType.U8 => Opcodes.ADD_I8,
                InternalType.I16 => Opcodes.ADD_I16,
                InternalType.U16 => Opcodes.ADD_I16,
                InternalType.I32 => Opcodes.ADD_I32,
                InternalType.U32 => Opcodes.ADD_I32,
                InternalType.I64 => Opcodes.ADD_I64,
                InternalType.U64 => Opcodes.ADD_I64,
                InternalType.F32 => Opcodes.ADD_F32,
                InternalType.F64 => Opcodes.ADD_F64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get adder.")
            };
        }
        
        public static Opcodes InternalTypeToSubOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.SUB_I8,
                InternalType.U8 => Opcodes.SUB_I8,
                InternalType.I16 => Opcodes.SUB_I16,
                InternalType.U16 => Opcodes.SUB_I16,
                InternalType.I32 => Opcodes.SUB_I32,
                InternalType.U32 => Opcodes.SUB_I32,
                InternalType.I64 => Opcodes.SUB_I64,
                InternalType.U64 => Opcodes.SUB_I64,
                InternalType.F32 => Opcodes.SUB_F32,
                InternalType.F64 => Opcodes.SUB_F64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get subtractor.")
            };
        }
        
        public static Opcodes InternalTypeToMultOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.MULT_I8,
                InternalType.U8 => Opcodes.MULT_I8,
                InternalType.I16 => Opcodes.MULT_I16,
                InternalType.U16 => Opcodes.MULT_I16,
                InternalType.I32 => Opcodes.MULT_I32,
                InternalType.U32 => Opcodes.MULT_I32,
                InternalType.I64 => Opcodes.MULT_I64,
                InternalType.U64 => Opcodes.MULT_I64,
                InternalType.F32 => Opcodes.MULT_F32,
                InternalType.F64 => Opcodes.MULT_F64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get multiplier.")
            };
        }
        
        public static Opcodes InternalTypeToDivOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.DIV_I8,
                InternalType.U8 => Opcodes.DIV_I8,
                InternalType.I16 => Opcodes.DIV_I16,
                InternalType.U16 => Opcodes.DIV_I16,
                InternalType.I32 => Opcodes.DIV_I32,
                InternalType.U32 => Opcodes.DIV_I32,
                InternalType.I64 => Opcodes.DIV_I64,
                InternalType.U64 => Opcodes.DIV_I64,
                InternalType.F32 => Opcodes.DIV_F32,
                InternalType.F64 => Opcodes.DIV_F64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get divider.")
            };
        }
        
        public static Opcodes InternalTypeToModuloOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.MODULO_I8,
                InternalType.U8 => Opcodes.MODULO_I8,
                InternalType.I16 => Opcodes.MODULO_I16,
                InternalType.U16 => Opcodes.MODULO_I16,
                InternalType.I32 => Opcodes.MODULO_I32,
                InternalType.U32 => Opcodes.MODULO_I32,
                InternalType.I64 => Opcodes.MODULO_I64,
                InternalType.U64 => Opcodes.MODULO_I64,
                InternalType.F32 => Opcodes.MODULO_F32,
                InternalType.F64 => Opcodes.MODULO_F64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get modulo.")
            };
        }
        
        public static Opcodes InternalTypeToBitAndOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.BIT_AND_I8,
                InternalType.U8 => Opcodes.BIT_AND_I8,
                InternalType.I16 => Opcodes.BIT_AND_I16,
                InternalType.U16 => Opcodes.BIT_AND_I16,
                InternalType.I32 => Opcodes.BIT_AND_I32,
                InternalType.U32 => Opcodes.BIT_AND_I32,
                InternalType.I64 => Opcodes.BIT_AND_I64,
                InternalType.U64 => Opcodes.BIT_AND_I64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get bit and.")
            };
        }
        
        public static Opcodes InternalTypeToBitOrOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.BIT_OR_I8,
                InternalType.U8 => Opcodes.BIT_OR_I8,
                InternalType.I16 => Opcodes.BIT_OR_I16,
                InternalType.U16 => Opcodes.BIT_OR_I16,
                InternalType.I32 => Opcodes.BIT_OR_I32,
                InternalType.U32 => Opcodes.BIT_OR_I32,
                InternalType.I64 => Opcodes.BIT_OR_I64,
                InternalType.U64 => Opcodes.BIT_OR_I64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get bit or.")
            };
        }
        
        public static Opcodes InternalTypeToBitNotOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.BIT_NOT_I8,
                InternalType.U8 => Opcodes.BIT_NOT_I8,
                InternalType.I16 => Opcodes.BIT_NOT_I16,
                InternalType.U16 => Opcodes.BIT_NOT_I16,
                InternalType.I32 => Opcodes.BIT_NOT_I32,
                InternalType.U32 => Opcodes.BIT_NOT_I32,
                InternalType.I64 => Opcodes.BIT_NOT_I64,
                InternalType.U64 => Opcodes.BIT_NOT_I64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get bit not.")
            };
        }
        
        public static Opcodes InternalTypeToBitXorOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.BIT_XOR_I8,
                InternalType.U8 => Opcodes.BIT_XOR_I8,
                InternalType.I16 => Opcodes.BIT_XOR_I16,
                InternalType.U16 => Opcodes.BIT_XOR_I16,
                InternalType.I32 => Opcodes.BIT_XOR_I32,
                InternalType.U32 => Opcodes.BIT_XOR_I32,
                InternalType.I64 => Opcodes.BIT_XOR_I64,
                InternalType.U64 => Opcodes.BIT_XOR_I64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get bit xor.")
            };
        }
        
        public static Opcodes InternalTypeToBitShiftLeftOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.BIT_SHIFT_LEFT_I8,
                InternalType.U8 => Opcodes.BIT_SHIFT_LEFT_I8,
                InternalType.I16 => Opcodes.BIT_SHIFT_LEFT_I16,
                InternalType.U16 => Opcodes.BIT_SHIFT_LEFT_I16,
                InternalType.I32 => Opcodes.BIT_SHIFT_LEFT_I32,
                InternalType.U32 => Opcodes.BIT_SHIFT_LEFT_I32,
                InternalType.I64 => Opcodes.BIT_SHIFT_LEFT_I64,
                InternalType.U64 => Opcodes.BIT_SHIFT_LEFT_I64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get bit shift left.")
            };
        }
        
        public static Opcodes InternalTypeToBitShiftRightOpcode(this InternalType type)
        {
            return type switch
            {
                InternalType.I8 => Opcodes.BIT_SHIFT_RIGHT_I8,
                InternalType.U8 => Opcodes.BIT_SHIFT_RIGHT_I8,
                InternalType.I16 => Opcodes.BIT_SHIFT_RIGHT_I16,
                InternalType.U16 => Opcodes.BIT_SHIFT_RIGHT_I16,
                InternalType.I32 => Opcodes.BIT_SHIFT_RIGHT_I32,
                InternalType.U32 => Opcodes.BIT_SHIFT_RIGHT_I32,
                InternalType.I64 => Opcodes.BIT_SHIFT_RIGHT_I64,
                InternalType.U64 => Opcodes.BIT_SHIFT_RIGHT_I64,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type isn't internal, can't get bit shift right.")
            };
        }
    }
}