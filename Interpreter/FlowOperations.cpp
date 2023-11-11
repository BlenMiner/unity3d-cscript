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
	if (stack->PEEK<char>())
		 NEXT_INSTRUCTION;
	else program->IP = context.operand1;
}

OPCODE_DEFINITION(JMP_IF_ZERO)
{
	if (stack->POP<char>())
		NEXT_INSTRUCTION;
	else program->IP = context.operand1;
}

OPCODE_DEFINITION(CALL)
{
	stack->PUSH_RETURN_INFO(program->IP, stack->SCOPE_SP);

	program->IP = context.operand1;
	stack->SCOPE_SP = stack->SP;
}

inline void smart_memcpy(void* dst, const void* src, const int len)
{
	switch (len)
	{
		case sizeof(int) :
		{
			*((int*)dst) = *((int*)src);
			break;
		}
		case sizeof(char) :
		{
			*((char*)dst) = *((char*)src);
			break;
		}
		case sizeof(long long) :
		{
			*((long long*)dst) = *((long long*)src);
			break;
		}
		case sizeof(short) :
		{
			*((short*)dst) = *((short*)src);
			break;
		}

		default:
		{
			memcpy(
				dst,
				src,
				len
			);
			break;
		}
	}
}

OPCODE_DEFINITION(CALL_ARGS)
{
	int op2 = (int)context.operand2;
	auto offset = stack->rawData + stack->SP;

	smart_memcpy(
		offset - (sizeof(int) * 2),
		offset,
		op2
	);

	stack->SP += op2;
	stack->PUSH_RETURN_INFO(program->IP, stack->SCOPE_SP);

	program->IP = (int)context.operand1;
	stack->SCOPE_SP = stack->SP;
}

OPCODE_DEFINITION(RETURN)
{
	auto startCopyIdx = stack->SP;
	auto copySize = stack->SCOPE_SP - startCopyIdx;

	stack->SP = stack->SCOPE_SP;
	stack->POP_RETURN_INFO(program->IP, stack->SCOPE_SP);

	if (copySize == 0)
	{
		NEXT_INSTRUCTION;
		return;
	}

	stack->SP -= copySize;

	smart_memcpy(
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

	auto loopStart = program->IP;
	auto loopEnd = loopStart;

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