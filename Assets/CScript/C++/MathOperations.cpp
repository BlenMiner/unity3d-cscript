#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(ADD)				
{
	stack->data[stack->SP + 1] += stack->data[stack->SP++];
}

OPCODE_DEFINITION(ADD_CONST)		{ stack->data[stack->SP] += context.operand1; }

OPCODE_DEFINITION(ADD_CONST_TO_SPTR) {
	stack->data[stack->SCOPE_SP - context.operand2 - 1] += context.operand1;
}

OPCODE_DEFINITION(ADD_SPTR_SPTR_INTO_SPTR) {
	stack->data[stack->SCOPE_SP - context.operand3 - 1] = stack->data[stack->SCOPE_SP - context.operand1 - 1] + stack->data[stack->SCOPE_SP - context.operand2 - 1];
}
