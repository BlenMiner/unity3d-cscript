#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(ADD)				
{
	auto stackData = stack->data;
	stackData[stack->SP + 1] += stackData[stack->SP++];
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_CONST)
{
	auto stackData = stack->data;
	stackData[stack->SP] += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_CONST_TO_SPTR) 
{
	auto stackData = stack->data;
	stackData[stack->SCOPE_SP - context.operand2] += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_SPTR_SPTR_INTO_SPTR)
{
	auto stackData = stack->data;
	auto scope = stack->SCOPE_SP;

	stackData[scope - context.operand3] = stackData[scope - context.operand1] + stackData[scope - context.operand2];
	NEXT_INSTRUCTION;
}
