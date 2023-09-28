#pragma once

#include "Stack.h"

#define OPCODE_DEFINITION(name) void name##_IMP(Program* program, Stack* stack, Instruction context)

#define OPCODE_LIST \
    OPCODE(JMP_IF_TOP_ZERO)\
    OPCODE(JMP)\
	\
	OPCODE(ADD)\
    OPCODE(ADD_CONST)\
    OPCODE(ADD_CONST_TO_REG)\
    OPCODE(ADD_REG_TO_REG)\
	\
    OPCODE(COPY_REG_TO_REG)\
    OPCODE(SWAP_REG_REG)\
	\
    OPCODE(PUSH_CONST) \
    OPCODE(PUSH_REG) \
    \
    OPCODE(POP) \
    OPCODE(POP_TO_REG) \
    \
    OPCODE(DUP)\
	\
	OPCODE(REPEAT)\
	OPCODE(REPEAT_CONST)\
	OPCODE(REPEAT_REG)\
	OPCODE(REPEAT_END)\

#define OPCODE(x) x,
enum Opcodes : int
{
    OPCODE_LIST
	OPCODES_COUNT
};
#undef OPCODE

enum Registers : int
{
	R0 = 0,
	R1,
	R2,
	R3,
	R4,
	R5,
	R6,
	R7,
	R8,
	R9,
};

struct Instruction
{
	Instruction() 
	{
		opcode = -1;
		operand = 0;
		operand2 = 0;
		reg = Registers::R0;
		reg2 = Registers::R0;
	}

	Instruction(Opcodes instruction)
	{
		this->opcode = instruction;
		this->operand = 0;
		this->operand2 = 0;
		this->reg = Registers::R0;
		this->reg2 = Registers::R0;
	}

	Instruction(Opcodes instruction, long long operand)
	{
		this->opcode = instruction;
		this->operand = operand;
		this->operand2 = 0;
		this->reg = Registers::R0;
		this->reg2 = Registers::R0;
	}

	Instruction(Opcodes instruction, Registers regA, Registers regB)
	{
		this->opcode = instruction;
		this->operand = 0;
		this->operand2 = 0;
		this->reg = regA;
		this->reg2 = regB;
	}

	Instruction(Opcodes instruction, Registers regA, long long operand)
	{
		this->opcode = instruction;
		this->operand = operand;
		this->operand2 = 0;
		this->reg = regA;
		this->reg2 = Registers::R0;
	}

	Instruction(Opcodes instruction, Registers regA)
	{
		this->opcode = instruction;
		this->operand = 0;
		this->operand2 = 0;
		this->reg = regA;
		this->reg2 = Registers::R0;
	}

	Instruction(Opcodes instruction, long long operandA, long long operandB)
	{
		this->opcode = instruction;
		this->operand = operandA;
		this->operand2 = operandB;
		this->reg = Registers::R0;
		this->reg2 = Registers::R0;
	}

	int opcode;
	int reg;
	int reg2;
	long long operand;
	long long operand2;
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