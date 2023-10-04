// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Main.h"

#include <string.h>

#define OPCODE(X) & X##_IMP,

void (*JUMP_TABLE[])(Program*, Stack*, Instruction)
{
	OPCODE_LIST
};

#undef OPCODE

#define OPCODE(x) #x,
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

void ExecuteInstruction(Program* program, Stack* stack, Instruction instruction)
{
#define OPCODE(x) case x: x##_IMP(program, stack, instruction); break;
	switch (instruction.opcode)
	{
		OPCODE_LIST
	}
#undef OPCODE
}

long long ExecuteProgram(Program* program)
{
	auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	stackPtr->IP = 0;
	stackPtr->ResetSP();

	while (stackPtr->IP < length)
	{
		Instruction instruction = program->instructions[stackPtr->IP++];
		ExecuteInstruction(program, stackPtr, instruction);
	}

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}

long long ExecuteFunction(Program* program, int functionIP)
{
	auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	stackPtr->IP = functionIP;
	stackPtr->ResetSP();

	stackPtr->PUSH(0);
	stackPtr->PUSH(length);
	stackPtr->SCOPE_SP = stackPtr->SP;

	while (stackPtr->IP < length)
	{
		Instruction instruction = program->instructions[stackPtr->IP++];
		ExecuteInstruction(program, stackPtr, instruction);
	}

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}