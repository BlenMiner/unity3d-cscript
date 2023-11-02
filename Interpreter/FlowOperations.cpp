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
	if (stack->PEEK<char>() == 0)
		 program->IP = context.operand1;
	else NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(JMP_IF_ZERO)
{
	if (stack->POP<char>() == 0)
		program->IP = context.operand1;
	else NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(CALL)
{
	stack->PUSH<int>(stack->SCOPE_SP);
	stack->PUSH<int>(program->IP);

	program->IP = context.operand1;
	stack->SCOPE_SP = stack->SP;
}

OPCODE_DEFINITION(CALL_ARGS)
{
	auto SP = stack->SP;

	// move memory to two spaces ahead
	memmove(
		stack->rawData + SP - (sizeof(int) * 2),
		stack->rawData + SP,
		context.operand2
	);

	stack->SP = SP + context.operand2;

	stack->PUSH<int>(stack->SCOPE_SP);
	stack->PUSH<int>(program->IP);

	program->IP = context.operand1;
	stack->SCOPE_SP = stack->SP;
}

OPCODE_DEFINITION(RETURN)
{
	auto scope = stack->SCOPE_SP;
	auto startCopyIdx = stack->SP;
	auto copySize = scope - startCopyIdx;

	stack->SP = scope;
	program->IP =		stack->POP<int>();
	stack->SCOPE_SP =	stack->POP<int>();

	if (copySize == 0)
	{
		NEXT_INSTRUCTION;
		return;
	}

	stack->SP -= copySize;

	memcpy(
		stack->rawData + stack->SP,
		stack->rawData + startCopyIdx,
		copySize
	);

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
	REPEAT_COUNT(program, stack, stack->POP<long long>());
}

OPCODE_DEFINITION(REPEAT_END) {}

OPCODE_DEFINITION(REPEAT_SPTR)
{
	REPEAT_COUNT(program, stack, stack->GET_VAR<long long>(context.operand1));
}