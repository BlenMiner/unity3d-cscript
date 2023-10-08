#pragma once

#include "Stack.h"

#define NEXT_INSTRUCTION program->IP++
#define OPCODE_DEFINITION(name) void name##_IMP(Program* program)

#define OPCODE_LIST \
    OPCODE(JMP_IF_TOP_ZERO)\
    OPCODE(JMP)\
    OPCODE(CALL)\
    OPCODE(RETURN)\
    OPCODE(STOP)\
	\
	OPCODE(ADD)\
    OPCODE(ADD_CONST)\
    OPCODE(ADD_CONST_TO_SPTR)\
    OPCODE(ADD_SPTR_SPTR_INTO_SPTR)\
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
    OPCODE(SWAP_SPTR_SPTR)\
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
		operand1 = 0;
		operand2 = 0;
		operand3 = 0;
		operand4 = 0;
	}

	Instruction(Opcodes instruction)
	{
		this->opcode = instruction;
		this->operand1 = 0;
		this->operand2 = 0;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operand)
	{
		this->opcode = instruction;
		this->operand1 = operand;
		this->operand2 = 0;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB)
	{
		this->opcode = instruction;
		this->operand1 = operandA;
		this->operand2 = operandB;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB, long long operandC)
	{
		this->opcode = instruction;
		this->operand1 = operandA;
		this->operand2 = operandB;
		this->operand3 = 0;
		this->operand4 = 0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB, long long operandC, long long operandD)
	{
		this->opcode = instruction;
		this->operand1 = operandA;
		this->operand2 = operandB;
		this->operand3 = operandC;
		this->operand4 = operandD;
	}

	int opcode;
	long long operand1;
	long long operand2;
	long long operand3;
	long long operand4;
};

struct Program
{
	Program(const Instruction* instructions, const int instructionsCount)
	{
		this->stack = new Stack();
		this->instructions = new Instruction[instructionsCount];
		this->opcodes = new int[instructionsCount];
		this->instructionsCount = instructionsCount;

		for (auto i = 0; i < instructionsCount; i++)
		{
			this->instructions[i] = instructions[i];
			this->opcodes[i] = instructions[i].opcode;
		}

		IP = 0;
	}

	~Program()
	{
		delete this->stack;
		delete[] this->instructions;
		delete[] this->opcodes;
	}

	Stack* stack;
	Instruction* instructions;
	int* opcodes;
	int instructionsCount;
	long long IP;
};

#define OPCODE(x) OPCODE_DEFINITION(x);

OPCODE_LIST

#undef OPCODE