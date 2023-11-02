﻿namespace Riten.CScript.Native
{
    public enum Opcodes : int
    {
        JMP_IF_TOP_ZERO,
        JMP_IF_ZERO,
        JMP,
        CALL,
        CALL_ARGS,
        RETURN,
        STOP,
        ADD_I8,
        ADD_I16,
        ADD_I32,
        ADD_I64,
        SUB_I8,
        SUB_I16,
        SUB_I32,
        SUB_I64,
        MULT_I8,
        MULT_I16,
        MULT_I32,
        MULT_I64,
        DIV_I8,
        DIV_I16,
        DIV_I32,
        DIV_I64,
        MODULO_I8,
        MODULO_I16,
        MODULO_I32,
        MODULO_I64,
        ADD_F32,
        ADD_F64,
        SUB_F32,
        SUB_F64,
        MULT_F32,
        MULT_F64,
        DIV_F32,
        DIV_F64,
        MODULO_F32,
        MODULO_F64,
        BIT_AND_I8,
        BIT_AND_I16,
        BIT_AND_I32,
        BIT_AND_I64,
        BIT_OR_I8,
        BIT_OR_I16,
        BIT_OR_I32,
        BIT_OR_I64,
        BIT_NOT_I8,
        BIT_NOT_I16,
        BIT_NOT_I32,
        BIT_NOT_I64,
        BIT_XOR_I8,
        BIT_XOR_I16,
        BIT_XOR_I32,
        BIT_XOR_I64,
        BIT_SHIFT_LEFT_I8,
        BIT_SHIFT_LEFT_I16,
        BIT_SHIFT_LEFT_I32,
        BIT_SHIFT_LEFT_I64,
        BIT_SHIFT_RIGHT_I8,
        BIT_SHIFT_RIGHT_I16,
        BIT_SHIFT_RIGHT_I32,
        BIT_SHIFT_RIGHT_I64,
        PUSH_I8,
        PUSH_I16,
        PUSH_I32,
        PUSH_I64,
        PUSH_F32,
        PUSH_F64,
        PUSH_SPTR_I8,
        PUSH_SPTR_I16,
        PUSH_SPTR_I32,
        PUSH_SPTR_I64,
        PUSH_SPTR_F32,
        PUSH_SPTR_F64,
        POP_TO_SPTR_I8,
        POP_TO_SPTR_I16,
        POP_TO_SPTR_I32,
        POP_TO_SPTR_I64,
        POP_TO_SPTR_F32,
        POP_TO_SPTR_F64,
        RESERVE,
        DISCARD,
        DUP,
        SWAP_SPTR_SPTR,
        REPEAT,
        REPEAT_CONST,
        REPEAT_SPTR,
        REPEAT_END,
        COPY_FROM_SPTR_TO_SPTR,
        LESS_OR_EQUAL_I8,
        LESS_OR_EQUAL_I16,
        LESS_OR_EQUAL_I32,
        LESS_OR_EQUAL_I64,
        LESS_OR_EQUAL_F32,
        LESS_OR_EQUAL_F64,
    }
}