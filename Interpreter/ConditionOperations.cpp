#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(LESS_OR_EQUAL)
{
	auto stackData = stack->data;

	stackData[stack->SP + 1] = stackData[stack->SP + 1] <= stackData[stack->SP];
	stack->SP++;

	NEXT_INSTRUCTION;
}