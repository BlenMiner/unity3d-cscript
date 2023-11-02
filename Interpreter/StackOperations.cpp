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
