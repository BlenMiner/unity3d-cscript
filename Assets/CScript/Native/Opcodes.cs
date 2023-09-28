namespace CScript.Native
{
    public enum Opcodes : int
    {
        JMP_IF_TOP_ZERO,
        JMP,
        ADD,
        ADD_CONST,
        ADD_CONST_TO_REG,
        ADD_REG_TO_REG,
        COPY_REG_TO_REG,
        SWAP_REG_REG,
        PUSH_CONST,
        PUSH_REG,
        POP,
        POP_TO_REG,
        DUP,
        REPEAT,
        REPEAT_CONST,
        REPEAT_REG,
        REPEAT_END,
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