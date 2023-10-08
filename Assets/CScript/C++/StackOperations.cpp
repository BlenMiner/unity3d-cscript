#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(PUSH_CONST)	
{ 
	program->stack->PUSH(program->instructions[program->IP].operand1);
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(POP)			
{
	program->stack->POP_DISCARD();
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(DUP) 
{ 
	program->stack->DUP(); 
	NEXT_INSTRUCTION;
}


OPCODE_DEFINITION(RESERVE)
{ 
	program->stack->SP -= program->instructions[program->IP].operand1; 
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(DISCARD) 
{
	program->stack->SP += program->instructions[program->IP].operand1; 
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(PUSH_CONST_TO_SPTR)
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];
	stack->data[stack->SCOPE_SP - context.operand2] = context.operand1;
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(PUSH_FROM_SPTR)
{
	auto stack = program->stack;
	stack->PUSH(stack->data[stack->SCOPE_SP - program->instructions[program->IP].operand1]);
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(POP_TO_SPTR)
{
	auto stack = program->stack;
	program->stack->data[stack->SCOPE_SP - program->instructions[program->IP].operand1] = stack->POP();
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(COPY_FROM_SPTR_TO_SPTR)
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];
	stack->data[stack->SCOPE_SP - context.operand2] = stack->data[stack->SCOPE_SP - context.operand1];
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(SWAP_SPTR_SPTR)
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];

	auto a = stack->SCOPE_SP - context.operand1;
	auto b = stack->SCOPE_SP - context.operand2;

	auto t = stack->data[b];
	stack->data[b] = stack->data[a];
	stack->data[a] = t;

	NEXT_INSTRUCTION;
}
