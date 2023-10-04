#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(PUSH_CONST)	{ stack->PUSH(context.operand); }
OPCODE_DEFINITION(POP)			{ stack->POP_DISCARD(); }
OPCODE_DEFINITION(DUP) { stack->DUP(); }


OPCODE_DEFINITION(RESERVE) { stack->SP -= context.operand; }
OPCODE_DEFINITION(DISCARD) { stack->SP += context.operand; }

OPCODE_DEFINITION(PUSH_CONST_TO_SPTR)
{
	stack->data[stack->SCOPE_SP - context.operand2 - 1] = context.operand;
}
OPCODE_DEFINITION(PUSH_FROM_SPTR)
{
	stack->PUSH(stack->data[stack->SCOPE_SP - context.operand - 1]);
}
OPCODE_DEFINITION(POP_TO_SPTR)
{
	stack->data[stack->SCOPE_SP - context.operand - 1] = stack->POP();
}

OPCODE_DEFINITION(COPY_FROM_SPTR_TO_SPTR)
{
	stack->data[stack->SCOPE_SP - context.operand2 - 1] = stack->data[stack->SCOPE_SP - context.operand - 1];
}