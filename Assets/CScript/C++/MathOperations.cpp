#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(ADD)				
{
	stack->data[stack->SP + 1] += stack->data[stack->SP++];
}

OPCODE_DEFINITION(ADD_CONST)		{ stack->data[stack->SP] += context.operand; }
OPCODE_DEFINITION(ADD_CONST_TO_REG) { stack->registers[context.reg] += context.operand; }
OPCODE_DEFINITION(ADD_REG_TO_REG)  { stack->registers[context.reg2] += stack->registers[context.reg]; }
