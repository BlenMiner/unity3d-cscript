// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Main.h"

#include <string.h>

#define OPCODE(X) & X##_IMP,

void (*JUMP_TABLE[])(Program*, Stack*, const SaturatedInstruction&)
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

void ExecuteInstruction(Program* program, Stack* stack)
{
	auto inst = program->instructions[program->IP];
	inst.function(program, stack, inst);
	
/*#define OPCODE(x) case x: x##_IMP(program, stack, inst); break;
	switch (inst.opcode)
	{
		OPCODE_LIST
	}
#undef OPCODE*/
}

void ExecuteInstruction(Program* program, Stack* stack, SaturatedInstruction& inst)
{
	inst.function(program, stack, inst);
}

long long ExecuteProgram(Program* program)
{
	const auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = 0;
	stackPtr->ResetSP();

	while (program->IP < length)
	{
		auto inst = program->instructions[program->IP];
		ExecuteInstruction(program, stackPtr, inst);

		/*for (int i = 1; i < inst.safeToExecuteBlindlyCount; i++)
			ExecuteInstruction(program, stackPtr);*/
	}

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}

long long ExecuteProgramWithOffset(Program* program, const int ipOffset)
{
	const auto length = program->instructionsCount;
	Stack* stackPtr = program->stack;

	program->IP = ipOffset;
	stackPtr->ResetSP();

	while (program->IP < length)
		ExecuteInstruction(program, stackPtr);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
}

long long ExecuteFunction(Program* program, const int functionIP)
{
	auto length = (unsigned long long)program->instructionsCount;
	Stack* stackPtr = program->stack;

	stackPtr->ResetSP();

	stackPtr->PUSH(stackPtr->SCOPE_SP);
	stackPtr->PUSH(length - 1);

	program->IP = functionIP;
	stackPtr->SCOPE_SP = stackPtr->SP - 1;

	while (program->IP < length)
		ExecuteInstruction(program, stackPtr);

	return stackPtr->GetPushedSize() > 0 ? stackPtr->PEEK() : 0;
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