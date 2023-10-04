namespace Riten.CScript.Native
{
    public enum Opcodes : int
    {
        JMP_IF_TOP_ZERO,
        JMP,
        CALL,
        RETURN,
        ADD,
        ADD_CONST,
        ADD_CONST_TO_SPTR,
        PUSH_CONST,
        PUSH_FROM_SPTR,
        PUSH_CONST_TO_SPTR,
        POP,
        POP_TO_SPTR,
        RESERVE,
        DISCARD,
        DUP,
        REPEAT,
        REPEAT_CONST,
        REPEAT_SPTR,
        REPEAT_END,
        COPY_FROM_SPTR_TO_SPTR,
    }
    
    public enum Registers : int
    {
        R0 = 0,
        R1,
        R2,
        R3,
        R4,
        R5,
        R6,
        R7,
        R8,
        R9,
    };
}