#include "Main.h"
// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Main.h"

#include <string.h>

#define OPCODE(X, Y) & X##_IMP,

void (*JUMP_TABLE[])(Program*, Stack*, const SaturatedInstruction&)
{
	OPCODE_LIST
};

#undef OPCODE

#define OPCODE(x, y) #x,
const char* OPCODE_NAMES[] = {
	OPCODE_LIST
};
#undef OPCODE

int InitializeJumpTable()
{
    return Opcodes::OPCODES_COUNT;
}

int ValidateOpcode(int index, const char* input)
{
    if (strcmp(input, OPCODE_NAMES[index]) == 0)
		return 1;
    return 0;
}

const char* GetOpcodeName(int index)
{
	return OPCODE_NAMES[index];
}

Program* CreateProgram(const Instruction* arr, const int length)
{
	return new Program(arr, length);
}

void FreeProgram(const Program* program)
{
	delete program;
}

void ExecuteInstruction(Program* program, Stack* stack, SaturatedInstruction& inst)
{
	inst.function(program, stack, inst);
}

void ExecuteInstructionImp(Program* program, SaturatedInstruction* instructions, Stack* stack)
{
	auto inst = instructions[program->IP];
	inst.function(program, stack, inst);
}

void DoExecute(Program* program, const unsigned long long length, Stack* stackPtr)
{
	auto instructions = program->instructions;

	while (program->IP < length)
	{
		auto inst = instructions[program->IP];
		inst.function(program, stackPtr, inst);

		switch (inst.safeToExecuteBlindlyCount)
		{
		case 20: ExecuteInstructionImp(program, instructions, stackPtr);
		case 19: ExecuteInstructionImp(program, instructions, stackPtr);
		case 18: ExecuteInstructionImp(program, instructions, stackPtr);
		case 17: ExecuteInstructionImp(program, instructions, stackPtr);
		case 16: ExecuteInstructionImp(program, instructions, stackPtr);
		case 15: ExecuteInstructionImp(program, instructions, stackPtr);
		case 14: ExecuteInstructionImp(program, instructions, stackPtr);
		case 13: ExecuteInstructionImp(program, instructions, stackPtr);
		case 12: ExecuteInstructionImp(program, instructions, stackPtr);
		case 11: ExecuteInstructionImp(program, instructions, stackPtr);
		case 10: ExecuteInstructionImp(program, instructions, stackPtr);
		case  9: ExecuteInstructionImp(program, instructions, stackPtr);
		case  8: ExecuteInstructionImp(program, instructions, stackPtr);
		case  7: ExecuteInstructionImp(program, instructions, stackPtr);
		case  6: ExecuteInstructionImp(program, instructions, stackPtr);
		case  5: ExecuteInstructionImp(program, instructions, stackPtr);
		case  4: ExecuteInstructionImp(program, instructions, stackPtr);
		case  3: ExecuteInstructionImp(program, instructions, stackPtr);
		case  2: ExecuteInstructionImp(program, instructions, stackPtr);
			break;
		}
	}
}

void ExecuteInstruction(Program* program, Stack* stack)
{
	ExecuteInstructionImp(program, program->instructions, stack);
}

long long ExecuteProgram(Program* program)
{
	const auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = 0;

	DoExecute(program, length, stackPtr);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK<long long>() : 0;
}

long long ExecuteProgramWithOffset(Program* program, const int ipOffset)
{
	const auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = ipOffset;
	stackPtr->ResetSP();

	DoExecute(program, length, stackPtr);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK<long long>() : 0;
}

long long ExecuteFunction(Program* program, const int functionIP)
{
	auto length = (unsigned long long)program->instructionsCount;
	Stack* stackPtr = program->stack;

	stackPtr->ResetSP();

	stackPtr->PUSH<int>(stackPtr->SCOPE_SP);
	stackPtr->PUSH<int>(program->instructionsCount - 1);

	program->IP = functionIP;
	stackPtr->SCOPE_SP = stackPtr->SP;

	DoExecute(program, length, stackPtr);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK<long long>() : 0;
}

void SaturatedInstruction::InitializeSaturatedInstruction(const Instruction& instruction)
{
	this->function = JUMP_TABLE[instruction.opcode];
	this->opcode = instruction.opcode;
	this->operand1 = instruction.operand1;
	this->operand2 = instruction.operand2;
	this->operand3 = instruction.operand3;
	this->operand4 = instruction.operand4;
}