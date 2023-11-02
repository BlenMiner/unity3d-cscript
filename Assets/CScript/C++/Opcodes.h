#pragma once

#include "Stack.h"

#define NEXT_INSTRUCTION program->IP++
#define HALT program->IP = program->instructionsCount - 1
#define OPCODE_DEFINITION(name) void name##_IMP(Program* program, Stack* stack, const SaturatedInstruction& context)

#define INT_OPCODE(X) \
OPCODE(X##_I8, char)\
OPCODE(X##_I16, short)\
OPCODE(X##_I32, int)\
OPCODE(X##_I64, long long)

#define FLOAT_OPCODE(X) \
OPCODE(X##_F32, float)\
OPCODE(X##_F64, double)

#define OPCODE_LIST \
    OPCODE(JMP_IF_TOP_ZERO, void)\
    OPCODE(JMP_IF_ZERO, void)\
    OPCODE(JMP, void)\
    OPCODE(CALL, void)\
    OPCODE(CALL_ARGS, void)\
    OPCODE(RETURN, void)\
    OPCODE(STOP, void)\
	\
	INT_OPCODE(ADD)\
	INT_OPCODE(SUB)\
	INT_OPCODE(MULT)\
	INT_OPCODE(DIV)\
	INT_OPCODE(MODULO)\
	\
	FLOAT_OPCODE(ADD)\
	FLOAT_OPCODE(SUB)\
	FLOAT_OPCODE(MULT)\
	FLOAT_OPCODE(DIV)\
	FLOAT_OPCODE(MODULO)\
	\
	INT_OPCODE(BIT_AND)\
	INT_OPCODE(BIT_OR)\
	INT_OPCODE(BIT_NOT)\
	INT_OPCODE(BIT_XOR)\
	INT_OPCODE(BIT_SHIFT_LEFT)\
	INT_OPCODE(BIT_SHIFT_RIGHT)\
    \
	\
	INT_OPCODE(PUSH)\
	FLOAT_OPCODE(PUSH)\
	\
	INT_OPCODE(PUSH_SPTR)\
	FLOAT_OPCODE(PUSH_SPTR)\
    \
	INT_OPCODE(POP_TO_SPTR)\
	FLOAT_OPCODE(POP_TO_SPTR)\
    \
    OPCODE(RESERVE, void) \
    OPCODE(DISCARD, void) \
    \
    OPCODE(DUP, void)\
    OPCODE(SWAP_SPTR_SPTR, void)\
	\
	OPCODE(REPEAT, void)\
	OPCODE(REPEAT_CONST, void)\
	OPCODE(REPEAT_SPTR, void)\
	OPCODE(REPEAT_END, void)\
	\
    OPCODE(COPY_FROM_SPTR_TO_SPTR, void) \
	\
	INT_OPCODE(LESS_OR_EQUAL)\
	FLOAT_OPCODE(LESS_OR_EQUAL)\


#define OPCODE(x, y) x,
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

struct Program;

struct SaturatedInstruction
{
	SaturatedInstruction()
	{
		function = nullptr;
		opcode = 0;
		operand1 = 0;
		operand2 = 0;
		operand3 = 0;
		operand4 = 0;
		safeToExecuteBlindlyCount = 0;
	}

	void InitializeSaturatedInstruction(const Instruction& instruction);

	void (*function)(Program*, Stack*, const SaturatedInstruction&);

	int opcode;
	long long operand1;
	long long operand2;
	long long operand3;
	long long operand4;
	int safeToExecuteBlindlyCount;
};

struct Program
{
	Program(const Instruction* instructions, const int instructionsCount)
	{
		this->stack = new Stack();
		this->instructions = new SaturatedInstruction[instructionsCount];
		this->instructionsCount = instructionsCount;

		for (auto i = 0; i < instructionsCount; i++)
		{
			SaturatedInstruction instruction;
			instruction.InitializeSaturatedInstruction(instructions[i]);
			instruction.safeToExecuteBlindlyCount = GetSafeToExecuteBlindlyCount(instructions, i);

			this->instructions[i] = instruction;
		}

		IP = 0;
	}

	int GetSafeToExecuteBlindlyCount(const Instruction* instructions, int index)
	{
		auto count = 0;

		for (auto i = index; i < instructionsCount; i++)
		{
			Opcodes opcode = (Opcodes)instructions[i].opcode;

			switch (opcode)
			{
				case Opcodes::JMP_IF_TOP_ZERO:
				case Opcodes::JMP_IF_ZERO:
				case Opcodes::JMP:
				case Opcodes::REPEAT:
				case Opcodes::REPEAT_CONST:
				case Opcodes::REPEAT_SPTR:
				case Opcodes::CALL:
				case Opcodes::RETURN:
					return count;

				default:
					count++;
			}

		}

		return count;
	}

	~Program()
	{
		delete this->stack;
		delete[] this->instructions;
	}

	Stack* stack;
	SaturatedInstruction* instructions;
	int instructionsCount;
	int IP;
};

#define OPCODE(x, y) OPCODE_DEFINITION(x);

OPCODE_LIST

#undef OPCODE