#include "pch.h"
#include "Opcodes.h"

#define GENERIC_COND(X, Y, Z) OPCODE_DEFINITION(X) {\
	auto a = stack->POP<Y>();\
	auto b = stack->POP<Y>();\
	stack->PUSH<char>(b Z a);\
	NEXT_INSTRUCTION;\
}


// ### LESS_OR_EQUAL

#define OPCODE(X, Y) GENERIC_COND(X, Y, <=)
INT_OPCODE(LESS_OR_EQUAL)
FLOAT_OPCODE(LESS_OR_EQUAL)
#undef OPCODE

// ### GREATER_THAN

#define OPCODE(X, Y) GENERIC_COND(X, Y, >)
INT_OPCODE(GREATER_THAN)
FLOAT_OPCODE(GREATER_THAN)
#undef OPCODE