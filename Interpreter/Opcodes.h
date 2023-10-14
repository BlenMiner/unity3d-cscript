#pragma once

#include "Stack.h"

#define NEXT_INSTRUCTION program->IP++
#define OPCODE_DEFINITION(name) void name##_IMP(Program* program, Stack* stack, const SaturatedInstruction& context)

#define OPCODE_LIST \
    OPCODE(JMP_IF_TOP_ZERO)\
    OPCODE(JMP_IF_ZERO)\
    OPCODE(JMP)\
    OPCODE(CALL)\
    OPCODE(CALL_ARGS)\
    OPCODE(RETURN)\
    OPCODE(STOP)\
	\
	OPCODE(ADD)\
    OPCODE(ADD_CONST)\
    OPCODE(ADD_CONST_TO_SPTR)\
    OPCODE(ADD_SPTR_SPTR_INTO_SPTR)\
	\
    OPCODE(PUSH_CONST) \
    OPCODE(PUSH_SPTR) \
    OPCODE(PUSH_SPTR_AND_CONST) \
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
	\
    OPCODE(LESS_OR_EQUAL) \


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
			this->instructions[i].InitializeSaturatedInstruction(instructions[i]);
			this->instructions[i].safeToExecuteBlindlyCount = GetSafeToExecuteBlindlyCount(instructions, i);
			
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
	unsigned long long IP;
};

#define OPCODE(x) OPCODE_DEFINITION(x);

OPCODE_LIST

#undef OPCODE