#pragma once

#include "Opcodes.h"

#if defined(__CYGWIN32__) || defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
#define EXTERN  __declspec(dllexport) 
#elif defined(__MACH__) || defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
#define EXTERN  __attribute__ ((visibility ("default"))) 
#else
#define EXTERN 
#endif

extern "C" {
	EXTERN int __cdecl InitializeJumpTable();

	int EXTERN __cdecl ValidateOpcode(int index, const char* input);

	EXTERN const char* __cdecl GetOpcodeName(int index);

	EXTERN Program* __cdecl CreateProgram(const Instruction* arr, const int length);

	void EXTERN __cdecl FreeProgram(const Program* program);

	long long EXTERN __cdecl ExecuteProgram(Program* program);

	long long EXTERN __cdecl ExecuteProgramWithOffset(Program* program, const int ipOffset);

	long long EXTERN __cdecl ExecuteFunction(Program* program, const int functionIP);

	void EXTERN __cdecl ExecuteInstruction(Program* program, Stack* stack);
}

