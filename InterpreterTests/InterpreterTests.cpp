#include "pch.h"
#include "CppUnitTest.h"

#include "../Interpreter/Main.h"
#include "../Interpreter/Stack.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace InterpreterTests
{
	TEST_CLASS(InterpreterTests)
	{
	public:

		TEST_METHOD(OPCODE_VALIDATION)
		{
			Assert::AreEqual(ValidateOpcode(Opcodes::POP, "POP"), 1);
			Assert::AreEqual(ValidateOpcode(Opcodes::POP, "POPS"), 0);
			Assert::AreEqual(ValidateOpcode(Opcodes::POP, "PO"), 0);
			Assert::AreEqual(ValidateOpcode(Opcodes::PUSH_CONST, "PUSH_CONST"), 1);
		}
		
		TEST_METHOD(STACK_PUSH_POP_INT)
		{
			Stack* stack = new Stack();

			stack->PUSH(5);

			Assert::AreEqual((long long)sizeof(char), stack->GetPushedSize());

			long long popped = stack->POP();

			Assert::AreEqual((long long)5, popped);

			delete stack;
		}

		TEST_METHOD(STACK_PUSH_POP_CHAR)
		{
			Stack* stack = new Stack();

			stack->PUSH('A');
			stack->PUSH('B');
			stack->PUSH('C');

			Assert::AreEqual((long long)sizeof(char) * 3, stack->GetPushedSize());

			char c = (char)stack->POP();
			char b = (char)stack->POP();
			char a = (char)stack->POP();

			Assert::AreEqual('C', c);
			Assert::AreEqual('B', b);
			Assert::AreEqual('A', a);

			delete stack;
		}
		TEST_METHOD(EXECUTE_FLOW_TEST_2)
		{
			const int LOOP = 1000000;

			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 0),
				Instruction(Opcodes::POP_TO_REG, Registers::R0),

				Instruction(Opcodes::PUSH_CONST, LOOP),
				Instruction(Opcodes::JMP_IF_TOP_ZERO, 4),

				Instruction(Opcodes::ADD_CONST, -1),
				Instruction(Opcodes::ADD_CONST_TO_REG, Registers::R0, 2),

				Instruction(Opcodes::JMP, -3),

				Instruction(Opcodes::POP),
				Instruction(Opcodes::PUSH_REG, Registers::R0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);

			auto program = CreateProgram(instructions, arrSize);

			long long res = ExecuteProgram(program);

			FreeProgram(program);

			Assert::AreEqual((long long)(LOOP * 2), res);
		}

		TEST_METHOD(EXECUTE_FOR_ENHANCED)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 0),
				Instruction(Opcodes::REPEAT_CONST, LOOP),

				Instruction(Opcodes::ADD_CONST, 1),
				Instruction(Opcodes::ADD_CONST, 1),

				Instruction(Opcodes::REPEAT_END),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);
			FreeProgram(program);

			Assert::AreEqual((long long)40, res);
		}
	};
}
