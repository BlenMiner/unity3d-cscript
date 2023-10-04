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

		TEST_METHOD(EXECUTE_FUNCTION)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 0),
				Instruction(Opcodes::ADD_CONST, 6),
				Instruction(Opcodes::ADD_CONST, 69),
				Instruction(Opcodes::RETURN),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteFunction(program, 0);

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());

			FreeProgram(program);

			Assert::AreEqual((long long)(6 +69), res);
		}

		TEST_METHOD(RESERVE_AND_POP_SPTR_TEST)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::PUSH_CONST, 10),
				Instruction(Opcodes::POP_TO_SPTR, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual((long long)2, program->stack->GetPushedSize());
			Assert::AreEqual((long long)(69), program->stack->POP());
			Assert::AreEqual((long long)(10), program->stack->POP());

			FreeProgram(program);

		}
		TEST_METHOD(EXECUTE_FUNCTION_LOCAL_VAR_TEST)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::POP_TO_SPTR, 0),
				Instruction(Opcodes::PUSH_FROM_SPTR, 0),
				Instruction(Opcodes::POP_TO_SPTR, 6),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 1),
				Instruction(Opcodes::RETURN),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteFunction(program, 0);

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());

			FreeProgram(program);

			Assert::AreEqual((long long)(5), res);
		}

		TEST_METHOD(TEST_REPEAT)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::PUSH_CONST, 10),
				Instruction(Opcodes::REPEAT),
				Instruction(Opcodes::ADD_CONST, 1),
				Instruction(Opcodes::REPEAT_END),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteFunction(program, 0);

			FreeProgram(program);

			Assert::AreEqual((long long)(5 + 10), res);
		}

		TEST_METHOD(COPY_FROM_SPTR)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 69, 0),
				Instruction(Opcodes::COPY_FROM_SPTR_TO_SPTR, 0, 1),
				Instruction(Opcodes::PUSH_FROM_SPTR, 1),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteFunction(program, 0);

			FreeProgram(program);

			Assert::AreEqual((long long)(69), res);
		}

		TEST_METHOD(COPY_FROM_SPTR_2)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 5, 0),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 70, 1),
				Instruction(Opcodes::ADD),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());

			FreeProgram(program);

			Assert::AreEqual((long long)(75), res);
		}
	};
}
