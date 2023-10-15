#include "pch.h"
#include "Opcodes.h"

OPCODE_DEFINITION(PUSH_CONST)	
{ 
	stack->PUSH(context.operand1);
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(POP)			
{
	stack->SP++;
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(DUP) 
{ 
	stack->DUP();
	NEXT_INSTRUCTION;
}


OPCODE_DEFINITION(RESERVE)
{ 
	stack->SP -= context.operand1;
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(DISCARD) 
{
	stack->SP += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(PUSH_CONST_TO_SPTR)
{
	stack->data[stack->SCOPE_SP - context.operand2] = context.operand1;
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(PUSH_SPTR)
{
	stack->PUSH(stack->data[stack->SCOPE_SP - context.operand1]);
	NEXT_INSTRUCTION;
}
OPCODE_DEFINITION(POP_TO_SPTR)
{
	stack->data[stack->SCOPE_SP - context.operand1] = stack->POP();
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(COPY_FROM_SPTR_TO_SPTR)
{
	auto stackData = stack->data;
	auto scope = stack->SCOPE_SP;
	stackData[scope - context.operand2] = stackData[scope - context.operand1];
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(PUSH_SPTR_AND_CONST)
{
	auto data = stack->data;
	auto SP = stack->SP;

	data[--SP] = data[stack->SCOPE_SP + context.operand1];
	data[--SP] = context.operand2;

	stack->SP = SP;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(SWAP_SPTR_SPTR)
{
	auto scope = stack->SCOPE_SP;
	auto a = scope - context.operand1;
	auto b = scope - context.operand2;

	auto stackData = stack->data;

	auto t = stackData[b];
	stackData[b] = stackData[a];
	stackData[a] = t;

	NEXT_INSTRUCTION;
}
