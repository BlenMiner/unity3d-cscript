#pragma once

#include "Stack.h"

#define OPCODE_DEFINITION(name) void name##_IMP(Program* program, Stack* stack, Instruction context)

#define OPCODE_LIST \
    OPCODE(JMP_IF_TOP_ZERO)\
    OPCODE(JMP)\
    OPCODE(CALL)\
    OPCODE(RETURN)\
	\
	OPCODE(ADD)\
    OPCODE(ADD_CONST)\
    OPCODE(ADD_CONST_TO_SPTR)\
	\
    OPCODE(PUSH_CONST) \
    OPCODE(PUSH_FROM_SPTR) \
    OPCODE(PUSH_CONST_TO_SPTR) \
    \
    OPCODE(POP) \
    OPCODE(POP_TO_SPTR) \
    OPCODE(RESERVE) \
    OPCODE(DISCARD) \
    \
    OPCODE(DUP)\
	\
	OPCODE(REPEAT)\
	OPCODE(REPEAT_CONST)\
	OPCODE(REPEAT_SPTR)\
	OPCODE(REPEAT_END)\
	\
    OPCODE(COPY_FROM_SPTR_TO_SPTR) \


#define OPCODE(x) x,
enum Opcodes : int
{
    OPCODE_LIST
	OPCODES_COUNT
};
#undef OPCODE

struct Instruction
{
	Instruction() 
	{
		opcode = -1;
		operand = 0;
		operand2 = 0;
		operand3 = 0;
		operand4 = 0;
	}

	Instruction(Opcodes instruction)
	{
		this->opcode = instruction;
		this->operand = 0;
		this->operand2 = 0;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operand)
	{
		this->opcode = instruction;
		this->operand = operand;
		this->operand2 = 0;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB)
	{
		this->opcode = instruction;
		this->operand = operandA;
		this->operand2 = operandB;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB, long long operandC)
	{
		this->opcode = instruction;
		this->operand = operandA;
		this->operand2 = operandB;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB, long long operandC, long long operandD)
	{
		this->opcode = instruction;
		this->operand = operandA;
		this->operand2 = operandB;
		this->operand3 = operandC;
		this->operand4 = operandD;
	}

	int opcode;
	long long operand;
	long long operand2;
	long long operand3;
	long long operand4;
};

struct Program
{
	Program(const Instruction* instructions, int instructionsCount)
	{
		this->stack = new Stack();
		this->instructions = new Instruction[instructionsCount];
		this->instructionsCount = instructionsCount;

		for (auto i = 0; i < instructionsCount; i++)
			this->instructions[i] = instructions[i];
	}

	~Program()
	{
		delete this->stack;
		delete[] this->instructions;
	}

	Stack* stack;
	Instruction* instructions;
	int instructionsCount;
};

#define OPCODE(x) OPCODE_DEFINITION(x);

OPCODE_LIST

#undef OPCODE