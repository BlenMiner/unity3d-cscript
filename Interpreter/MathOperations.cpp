#include "pch.h"
#include "Opcodes.h"

#define UNIARY_OPERATION(X, Y, Z) OPCODE_DEFINITION(X) {\
	stack->DO_OPERATION_TOP_1<Y>([&] (Y &a) -> Y { return Z ## a;});\
	NEXT_INSTRUCTION;\
}


#define GENERIC_OPERATION(X, Y, Z) OPCODE_DEFINITION(X) {\
	stack->DO_OPERATION_TOP_2<Y>([&] (Y &a, Y &b) -> Y { return b ## Z ## a;});\
	NEXT_INSTRUCTION;\
}

// ### ADD

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, +)
INT_OPCODE(ADD)
#undef OPCODE

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, +)
FLOAT_OPCODE(ADD)
#undef OPCODE

// ### SUB

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, -)
INT_OPCODE(SUB)
#undef OPCODE

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, -)
FLOAT_OPCODE(SUB)
#undef OPCODE

// ### MULT

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, *)
INT_OPCODE(MULT)
#undef OPCODE

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, *)
FLOAT_OPCODE(MULT)
#undef OPCODE

// ### DIV

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, /)
INT_OPCODE(DIV)
#undef OPCODE

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, /)
FLOAT_OPCODE(DIV)
#undef OPCODE

// ### MODULO

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, %)
INT_OPCODE(MODULO)
#undef OPCODE

OPCODE_DEFINITION(MODULO_F32)
{
	stack->DO_OPERATION_TOP_2<float>([&](float& a, float& b) -> float { return fmodf(b, a);});
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(MODULO_F64)
{
	stack->DO_OPERATION_TOP_2<double>([&](double& a, double& b) -> double { return fmod(b, a);});
	NEXT_INSTRUCTION;
}

// ### BIT_AND

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, &)
INT_OPCODE(BIT_AND)
#undef OPCODE

// ### BIT_OR

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, |)
INT_OPCODE(BIT_OR)
#undef OPCODE

// ### BIT_XOR

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, ^)
INT_OPCODE(BIT_XOR)
#undef OPCODE

// ### BIT_SHIFT_LEFT

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, <<)
INT_OPCODE(BIT_SHIFT_LEFT)
#undef OPCODE

// ### BIT_SHIFT_RIGHT

#define OPCODE(X, Y) GENERIC_OPERATION(X, Y, >>)
INT_OPCODE(BIT_SHIFT_RIGHT)
#undef OPCODE

// ### BIT_NOT

#define OPCODE(X, Y) UNIARY_OPERATION(X, Y, ~)
INT_OPCODE(BIT_NOT)
#undef OPCODE

// #####################

OPCODE_DEFINITION(ADD_CONST)
{
	stack->REPLACE<long long>(stack->PEEK<long long>() + context.operand1);
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_CONST_TO_SPTR) 
{
	stack->SET_VAR<long long>(
		context.operand2, 
		stack->GET_VAR<long long>(context.operand2) + context.operand1
	);
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_SPTR_SPTR_INTO_SPTR)
{
	stack->SET_VAR<long long>(
		context.operand3,
		stack->GET_VAR<long long>(context.operand1) + stack->GET_VAR<long long>(context.operand2)
	);
	NEXT_INSTRUCTION;
}