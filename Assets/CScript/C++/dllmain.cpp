// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Main.h"

#include <string.h>

#define OPCODE(X) & X##_IMP,

void (*JUMP_TABLE[])(Program*)
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

void ExecuteInstruction(Program* program)
{
	JUMP_TABLE[program->opcodes[program->IP]](program);
}

long long ExecuteProgram(Program* program)
{
	const auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = 0;
	stackPtr->ResetSP();

	while (program->IP < length)
		ExecuteInstruction(program);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}

long long ExecuteProgramWithOffset(Program* program, const int ipOffset)
{
	const auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = ipOffset;
	stackPtr->ResetSP();

	while (program->IP < length)
		ExecuteInstruction(program);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}

long long ExecuteFunction(Program* program, const int functionIP)
{
	auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = functionIP;

	stackPtr->ResetSP();
	stackPtr->PUSH(program->stack->SCOPE_SP);
	stackPtr->PUSH(length);
	stackPtr->SCOPE_SP = stackPtr->SP - 1;

	while (program->IP < length)
		ExecuteInstruction(program);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}