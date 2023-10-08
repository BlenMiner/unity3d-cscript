#include "pch.h"
#include "Opcodes.h"
#include "Main.h"
#include <vector>

OPCODE_DEFINITION(JMP)				
{
	program->IP += program->instructions[program->IP].operand1;
}

OPCODE_DEFINITION(JMP_IF_TOP_ZERO)	
{ 
	if (program->stack->PEEK() == 0)
		 program->IP += program->instructions[program->IP].operand1;
	else NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(CALL)
{
	auto stack = program->stack;
	auto context = program->instructions[program->IP];

	stack->PUSH(stack->SCOPE_SP);
	stack->PUSH(program->IP);

	program->IP = context.operand1;
	stack->SCOPE_SP = stack->SP - 1;
}

OPCODE_DEFINITION(RETURN)
{
	auto stack = program->stack;

	auto oldScope = stack->SCOPE_SP + 1;

	long long returnValueSize = oldScope - stack->SP;
	long long indexOfReturnValue = oldScope - returnValueSize + 1;

	stack->SP = oldScope;
	program->IP = stack->POP();
	stack->SCOPE_SP = stack->POP();

	if (returnValueSize == 0)
		return;

	memcpy(stack->data + stack->SP - returnValueSize, stack->data + indexOfReturnValue - returnValueSize, returnValueSize * sizeof(long long));

	stack->SP -= returnValueSize;
}

OPCODE_DEFINITION(STOP)
{
	program->IP = program->instructionsCount;
}

void REPEAT_COUNT(Program* program, const long long loopTimes)
{
	NEXT_INSTRUCTION;

	long long loopStart = program->IP;
	long long loopEnd = loopStart;

	while (loopEnd < program->instructionsCount && 
		program->instructions[loopEnd].opcode != REPEAT_END)
	{
		loopEnd++;
	}

	if (loopTimes == 0)
	{
		program->IP = loopEnd + 1;
		return;
	}

	for (auto i = 0; i < loopTimes; ++i)
	{
		while (program->IP < loopEnd)
			ExecuteInstruction(program);

		if (program->IP == loopEnd)
			program->IP = loopStart;
		else return;
	}

	program->IP = loopEnd + 1;
}

OPCODE_DEFINITION(REPEAT_CONST)
{
	REPEAT_COUNT(program, program->instructions[program->IP].operand1);
}

OPCODE_DEFINITION(REPEAT)
{
	auto stack = program->stack;
	REPEAT_COUNT(program, stack->POP());
}

OPCODE_DEFINITION(REPEAT_END) {}

OPCODE_DEFINITION(REPEAT_SPTR)
{
	auto stack = program->stack;
	REPEAT_COUNT(program, stack->data[stack->SCOPE_SP + program->instructions[program->IP].operand1]);
}