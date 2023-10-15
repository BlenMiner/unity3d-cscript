#include "pch.h"
#include "Opcodes.h"
#include "Main.h"
#include <vector>

OPCODE_DEFINITION(JMP)				
{
	program->IP = context.operand1;
}

OPCODE_DEFINITION(JMP_IF_TOP_ZERO)	
{ 
	if (stack->PEEK() == 0)
		 program->IP = context.operand1;
	else NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(JMP_IF_ZERO)
{
	if (stack->POP() == 0)
		program->IP = context.operand1;
	else NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(CALL)
{
	auto stackData = stack->data;
	auto SP = stack->SP;

	stackData[--SP] = stack->SCOPE_SP;
	stackData[--SP] = program->IP;

	program->IP = context.operand1;
	stack->SCOPE_SP = SP - 1;
	stack->SP = SP;
}

OPCODE_DEFINITION(CALL_ARGS)
{
	auto stackData = stack->data;
	auto SP = stack->SP;

	// move memory to two spaces ahead
	memmove(stackData + SP - 2, stackData + SP, context.operand2 * sizeof(long long));
	SP += context.operand2;

	stackData[--SP] = stack->SCOPE_SP;
	stackData[--SP] = program->IP;

	program->IP = context.operand1;
	stack->SCOPE_SP = SP - 1;
	stack->SP = SP;
}

OPCODE_DEFINITION(RETURN)
{
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

	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(STOP)
{
	program->IP = program->instructionsCount;
}

void REPEAT_COUNT(Program* program, Stack* stack, const long long loopTimes)
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
		while (true)
		{
			ExecuteInstruction(program, stack);

			if (program->IP == loopEnd)
			{
				program->IP = loopStart;
				break;
			}
		}
	}

	program->IP = loopEnd + 1;
}

OPCODE_DEFINITION(REPEAT_CONST)
{
	REPEAT_COUNT(program, stack, context.operand1);
}

OPCODE_DEFINITION(REPEAT)
{
	REPEAT_COUNT(program, stack, stack->POP());
}

OPCODE_DEFINITION(REPEAT_END) {}

OPCODE_DEFINITION(REPEAT_SPTR)
{
	REPEAT_COUNT(program, stack, stack->data[stack->SCOPE_SP + context.operand1]);
}