#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(PUSH_CONST)	{ stack->PUSH(context.operand); }
OPCODE_DEFINITION(PUSH_REG)		{ stack->PUSH(stack->GET_REGISTER(context.reg)); }
OPCODE_DEFINITION(POP)			{ stack->POP_DISCARD(); }
OPCODE_DEFINITION(POP_TO_REG)	{ stack->SET_REGISTER(context.reg, stack->POP()); }
OPCODE_DEFINITION(DUP) { stack->DUP(); }

OPCODE_DEFINITION(COPY_REG_TO_REG)	{ stack->registers[context.reg2] = stack->registers[context.reg]; }
OPCODE_DEFINITION(SWAP_REG_REG)		
{ 
	auto t = stack->registers[context.reg];
	stack->registers[context.reg] = stack->registers[context.reg2];
	stack->registers[context.reg2] = t;
}
