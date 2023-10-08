#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(ADD)				
{
	auto stack = program->stack;
	stack->data[stack->SP + 1] += stack->data[stack->SP++];
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_CONST)
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];
	stack->data[stack->SP] += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_CONST_TO_SPTR) 
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];
	stack->data[stack->SCOPE_SP - context.operand2] += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_SPTR_SPTR_INTO_SPTR)
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];
	stack->data[stack->SCOPE_SP - context.operand3] = stack->data[stack->SCOPE_SP - context.operand1] + stack->data[stack->SCOPE_SP - context.operand2];
	NEXT_INSTRUCTION;
}
