#include "pch.h"
#include "Opcodes.h"

#define UNIARY_OPERATION(X, Y, Z) OPCODE_DEFINITION(X) {\
	auto stackData = stack->data;\
	Y res = (Y)stackData[stack->SP];\
	stackData[stack->SP] = Z##res;\
	NEXT_INSTRUCTION;\
}

#define INT_OPERATION(X, Y, Z) OPCODE_DEFINITION(X) {\
	auto stackData = stack->data; auto sp = stack->SP;\
	stackData[sp + 1] = (Y)((Y)stackData[sp + 1] Z (Y)stackData[sp++]);\
	stack->SP = sp; NEXT_INSTRUCTION;\
}

#define FLOAT_OPERATION(X, Y, Z) OPCODE_DEFINITION(X) {\
	auto stackData = stack->data; auto sp = stack->SP;\
	Y res = *(Y*)(stackData + sp + 1) Z *(Y*)(stackData + sp);\
	stackData[++sp] = *(long long*)&res;\
	stack->SP = sp; NEXT_INSTRUCTION;\
}

// ### ADD

#define OPCODE(X, Y) INT_OPERATION(X, Y, +)
INT_OPCODE(ADD)
#undef OPCODE

#define OPCODE(X, Y) FLOAT_OPERATION(X, Y, +)
FLOAT_OPCODE(ADD)
#undef OPCODE

// ### SUB

#define OPCODE(X, Y) INT_OPERATION(X, Y, -)
INT_OPCODE(SUB)
#undef OPCODE

#define OPCODE(X, Y) FLOAT_OPERATION(X, Y, -)
FLOAT_OPCODE(SUB)
#undef OPCODE

// ### MULT

#define OPCODE(X, Y) INT_OPERATION(X, Y, *)
INT_OPCODE(MULT)
#undef OPCODE

#define OPCODE(X, Y) FLOAT_OPERATION(X, Y, *)
FLOAT_OPCODE(MULT)
#undef OPCODE

// ### DIV

#define OPCODE(X, Y) INT_OPERATION(X, Y, /)
INT_OPCODE(DIV)
#undef OPCODE

#define OPCODE(X, Y) FLOAT_OPERATION(X, Y, /)
FLOAT_OPCODE(DIV)
#undef OPCODE

// ### MODULO

#define OPCODE(X, Y) INT_OPERATION(X, Y, %)
INT_OPCODE(MODULO)
#undef OPCODE

OPCODE_DEFINITION(MODULO_F32)
{
	auto stackData = stack->data;
	auto sp = stack->SP;
	float res = fmodf(*(float*)(stackData + sp + 1), *(float*)(stackData + sp));
	stackData[++sp] = *(long long*)&res;
	stack->SP = sp;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(MODULO_F64)
{
	auto stackData = stack->data;
	auto sp = stack->SP;
	float res = fmod(*(double*)(stackData + sp + 1), *(double*)(stackData + sp));
	stackData[++sp] = *(long long*)&res;
	stack->SP = sp;
	NEXT_INSTRUCTION;
}

// ### BIT_AND

#define OPCODE(X, Y) INT_OPERATION(X, Y, &)
INT_OPCODE(BIT_AND)
#undef OPCODE

// ### BIT_OR

#define OPCODE(X, Y) INT_OPERATION(X, Y, |)
INT_OPCODE(BIT_OR)
#undef OPCODE

// ### BIT_XOR

#define OPCODE(X, Y) INT_OPERATION(X, Y, ^)
INT_OPCODE(BIT_XOR)
#undef OPCODE

// ### BIT_SHIFT_LEFT

#define OPCODE(X, Y) INT_OPERATION(X, Y, <<)
INT_OPCODE(BIT_SHIFT_LEFT)
#undef OPCODE

// ### BIT_SHIFT_RIGHT

#define OPCODE(X, Y) INT_OPERATION(X, Y, >>)
INT_OPCODE(BIT_SHIFT_RIGHT)
#undef OPCODE

// ### BIT_NOT

#define OPCODE(X, Y) UNIARY_OPERATION(X, Y, ~)
INT_OPCODE(BIT_NOT)
#undef OPCODE

// #####################

OPCODE_DEFINITION(ADD_CONST)
{
	auto stackData = stack->data;
	stackData[stack->SP] += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_CONST_TO_SPTR) 
{
	auto stackData = stack->data;
	stackData[stack->SCOPE_SP - context.operand2] += context.operand1;
	NEXT_INSTRUCTION;
}

OPCODE_DEFINITION(ADD_SPTR_SPTR_INTO_SPTR)
{
	auto stackData = stack->data;
	auto scope = stack->SCOPE_SP;

	stackData[scope - context.operand3] = stackData[scope - context.operand1] + stackData[scope - context.operand2];
	NEXT_INSTRUCTION;
}