#include "pch.h"
#include "Opcodes.h"
#include "Main.h"
#include <vector>

OPCODE_DEFINITION(JMP)				{ stack->IP += context.operand1 - 1; }
OPCODE_DEFINITION(JMP_IF_TOP_ZERO)	{ if (stack->PEEK() == 0) stack->IP += context.operand1 - 1; }

OPCODE_DEFINITION(CALL)
{
	stack->PUSH(stack->SCOPE_SP);
	stack->PUSH(stack->IP);

	stack->IP = context.operand1;
	stack->SCOPE_SP = stack->SP;
}

OPCODE_DEFINITION(RETURN)
{
	long long returnValueSize = stack->SCOPE_SP - stack->SP;
	long long indexOfReturnValue = stack->SCOPE_SP - returnValueSize + 1;

	stack->SP = stack->SCOPE_SP;
	stack->IP = stack->POP();
	stack->SCOPE_SP = stack->POP();

	// Copy return value to the top of the stack
	// memcpy(stack->data + stack->SP, stack->data + stack->SP + indexOfReturnValue, returnValueSize);
	memcpy(stack->data + stack->SP - returnValueSize, stack->data + indexOfReturnValue - returnValueSize, returnValueSize * sizeof(long long));

	stack->SP -= returnValueSize;
}



OPCODE_DEFINITION(REPEAT_CONST)
{
	long long loopTimes = context.operand1;

	long long loopStart = stack->IP;
	long long loopEnd = loopStart + 1;

	while (loopEnd < program->instructionsCount)
	{
		if (program->instructions[loopEnd].opcode == REPEAT_END)
		{
			--loopEnd;
			break;
		}

		loopEnd++;
	}

	if (loopTimes == 0)
	{
		stack->IP = loopEnd + 2;
		return;
	}

	for (auto i = 0; i < loopTimes; ++i)
	{
		while (stack->IP <= loopEnd) ExecuteInstruction(program, stack, program->instructions[stack->IP++]);

		if (stack->IP == loopEnd + 1)
			stack->IP = loopStart;
		else return;
	}

	stack->IP = loopEnd + 1;
}

OPCODE_DEFINITION(REPEAT)
{
	REPEAT_CONST_IMP(program, stack, Instruction{ REPEAT_CONST, stack->POP() });
}

OPCODE_DEFINITION(REPEAT_END) {}

OPCODE_DEFINITION(REPEAT_SPTR)
{
	REPEAT_CONST_IMP(program, stack, Instruction{ REPEAT_CONST, stack->data[stack->SCOPE_SP + context.operand1] });
}