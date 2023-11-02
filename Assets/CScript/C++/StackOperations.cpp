#include "pch.h"
#include "Opcodes.h"

#define PUSH_GENERIC(X, Y) OPCODE_DEFINITION(X) {\
	stack->PUSH<Y>(*((Y*)&context.operand1));\
	NEXT_INSTRUCTION;\
}

#define POP_GENERIC(X, Y) OPCODE_DEFINITION(X) {\
	stack->DISCARD(sizeof(Y));\
	NEXT_INSTRUCTION;\
}

#define PUSH_SPTR_GENERIC(X, Y) OPCODE_DEFINITION(X) {\
	stack->PUSH(stack->GET_VAR<Y>(context.operand1));\
	NEXT_INSTRUCTION;\
}

#define POP_SPTR_GENERIC(X, Y) OPCODE_DEFINITION(X) {\
	stack->SET_VAR(context.operand1, stack->POP<Y>());\
	NEXT_INSTRUCTION;\
}

// ### PUSH

#define OPCODE(X, Y) PUSH_GENERIC(X, Y)
INT_OPCODE(PUSH)
FLOAT_OPCODE(PUSH)
#undef OPCODE

// ### PUSH_SPTR

#define OPCODE(X, Y) PUSH_SPTR_GENERIC(X, Y)
INT_OPCODE(PUSH_SPTR)
FLOAT_OPCODE(PUSH_SPTR)
#undef OPCODE

// ### POP_TO_SPTR

#define OPCODE(X, Y) POP_SPTR_GENERIC(X, Y)
INT_OPCODE(POP_TO_SPTR)
FLOAT_OPCODE(POP_TO_SPTR)
#undef OPCODE

OPCODE_DEFINITION(DUP) 
{ 
	stack->PUSH(stack->PEEK<long long>());
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

OPCODE_DEFINITION(COPY_FROM_SPTR_TO_SPTR)
{
	stack->SET_VAR(context.operand2, stack->GET_VAR<long long>(context.operand1));
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(SWAP_SPTR_SPTR)
{
	auto a = stack->GET_VAR<long long>(context.operand1);
	auto b = stack->GET_VAR<long long>(context.operand2);

	stack->SET_VAR(context.operand1, b);
	stack->SET_VAR(context.operand2, a);

	NEXT_INSTRUCTION;
}

enum NativePtr : int
{
	CONST = 0,
	SPTR,
	PTR,
	NONE
};

void PUSH_CONST(const int size, char* data, int& byteOffset, Stack* stack)
{
	switch (size)
	{
	case 0:
		stack->PUSH(*(data + byteOffset));
		byteOffset += sizeof(char);
		break;
	case 1:
		stack->PUSH(*(short*)(data + byteOffset));
		byteOffset += sizeof(short);
		break;
	case 2:
		stack->PUSH(*(int*)(data + byteOffset));
		byteOffset += sizeof(int);
		break;
	case 3:
		stack->PUSH(*(long long*)(data + byteOffset));
		byteOffset += sizeof(long long);
		break;
	}
}

void PUSH_FROM_SPTR(int size, char* data, int& byteOffset, Stack* stack)
{
	int sptrOffset = *(short*)(data + byteOffset);
	byteOffset += sizeof(short);

	switch (size)
	{
	case 0:
		stack->PUSH(stack->GET_VAR<char>(sptrOffset));
		break;
	case 1:
		stack->PUSH(stack->GET_VAR<short>(sptrOffset));
		break;
	case 2:
		stack->PUSH(stack->GET_VAR<int>(sptrOffset));
		break;
	case 3:
		stack->PUSH(stack->GET_VAR<long long>(sptrOffset));
		break;
	}
}


OPCODE_DEFINITION(BATCHED_STACK_OP)
{
	long long packed = context.operand1;
	int byteOffset = 0;

	const int count = packed & 0b1111;
	char* data = (char*) & context.operand2;

	packed >>= 4;

	for (int i = 0; i < count; i++)
	{
		NativePtr type = (NativePtr)(packed & 0b11);
		packed >>= 2;

		int size = packed & 0b11;
		packed >>= 2;

		bool isPush = (packed & 0b1) == 0;
		packed >>= 1;

		if (isPush)
		{
			switch (type)
			{
				case CONST:
					PUSH_CONST(size, data, byteOffset, stack);
					break;
				case SPTR:
					PUSH_FROM_SPTR(size, data, byteOffset, stack);
					break;
			}
		}
	}

	NEXT_INSTRUCTION;
}
