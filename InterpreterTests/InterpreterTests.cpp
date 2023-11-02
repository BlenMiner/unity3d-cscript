#include "pch.h"
#include "CppUnitTest.h"

#include "../Interpreter/Main.h"
#include "../Interpreter/Stack.h"
#include <iostream>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace InterpreterTests
{
	TEST_CLASS(InterpreterTests)
	{
	public:

		TEST_METHOD(TEST_SUB_INT)
		{
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_I32, 10),
				Instruction(Opcodes::PUSH_I32, 5),
				Instruction(Opcodes::SUB_I32)
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);


			ExecuteProgram(program);

			Assert::AreEqual(4, program->stack->GetPushedSize());

			auto res = program->stack->POP<int>();

			Assert::AreEqual(5, res);

			FreeProgram(program);
		}


		TEST_METHOD(TEST_ADD_FLOAT)
		{
			Instruction instructions[]{
				Instruction(Opcodes::ADD_F32),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->stack->PUSH<float>(69.0f);
			program->stack->PUSH<float>(42.f);

			ExecuteProgram(program);

			Assert::AreEqual(4, program->stack->GetPushedSize());

			auto res = program->stack->POP<float>();

			Assert::AreEqual((float)(69.f + 42.f), res);

			FreeProgram(program);
		}

		TEST_METHOD(TEST_ADD_BYTE_OVERFLOW)
		{
			Instruction instructions[]{

				Instruction(Opcodes::PUSH_I8, 0x10),
				Instruction(Opcodes::PUSH_I8, 0xF1),
				Instruction(Opcodes::ADD_I8),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual(1, program->stack->GetPushedSize());
			Assert::AreEqual((char)(0x10 + 0xF1), program->stack->PEEK<char>());

			FreeProgram(program);
		}

		TEST_METHOD(OPCODE_VALIDATION)
		{
			Assert::AreEqual(ValidateOpcode(Opcodes::PUSH_I64, "PUSH_I64"), 1);
		}
		
		TEST_METHOD(STACK_PUSH_POP_INT)
		{
			Stack* stack = new Stack();

			stack->PUSH<int>(6);
			stack->PUSH<int>(9);

			Assert::AreEqual((int)sizeof(int) * 2, stack->GetPushedSize());

			int a = stack->POP<int>();
			int b = stack->POP<int>();

			Assert::AreEqual(9, a);
			Assert::AreEqual(6, b);

			delete stack;
		}

		TEST_METHOD(STACK_PUSH_POP_CHAR)
		{
			Stack* stack = new Stack();

			stack->PUSH('A');
			stack->PUSH('B');
			stack->PUSH('C');

			Assert::AreEqual((int)sizeof(char) * 3, stack->GetPushedSize());

			char cp = (char)stack->PEEK<char>();
			char c = (char)stack->POP<char>();

			char bp = (char)stack->PEEK<char>();
			char b = (char)stack->POP<char>();

			char ap = (char)stack->PEEK<char>();
			char a = (char)stack->POP<char>();

			Assert::AreEqual('C', cp);
			Assert::AreEqual('B', bp);
			Assert::AreEqual('A', ap);

			Assert::AreEqual('C', c);
			Assert::AreEqual('B', b);
			Assert::AreEqual('A', a);

			delete stack;
		}

		TEST_METHOD(CONDITION_LESS_OR_EQUAL_FALSE)
		{
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::PUSH_I64, 5),
				Instruction(Opcodes::LESS_OR_EQUAL_I64),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual(8, program->stack->GetPushedSize());
			Assert::AreEqual((long long)0, res);

			FreeProgram(program);
		}

		TEST_METHOD(CONDITION_LESS_OR_EQUAL_TRUE)
		{
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_I64, 5),
				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::LESS_OR_EQUAL_I64),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual(8, program->stack->GetPushedSize());
			Assert::AreEqual((long long)1, res);

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FOR_ENHANCED)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::PUSH_I64, 0),
				Instruction(Opcodes::REPEAT_CONST, LOOP),

				Instruction(Opcodes::PUSH_I64, 1),
				Instruction(Opcodes::ADD_I64, 1),

				Instruction(Opcodes::PUSH_I64, 1),
				Instruction(Opcodes::ADD_I64, 1),

				Instruction(Opcodes::REPEAT_END),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual(8, program->stack->GetPushedSize());

			FreeProgram(program);

			Assert::AreEqual((long long)40, res);
		}
		TEST_METHOD(EXECUTE_FUNCTION_2)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 8),

				Instruction(Opcodes::PUSH_I64, 0),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0),

				Instruction(Opcodes::PUSH_I64, 6),
				Instruction(Opcodes::ADD_I64),

				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::ADD_I64),

				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::CALL, 0),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgramWithOffset(program, 8);

			Assert::AreEqual(8, program->stack->GetPushedSize());
			Assert::AreEqual((long long)(6 + 69), program->stack->PEEK<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_ARGS)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 8),
				Instruction(Opcodes::PUSH_I64, 2),
				Instruction(Opcodes::ADD_I64),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::CALL_ARGS, 0, 8),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgramWithOffset(program, 4);

			Assert::AreEqual((8), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(2 + 69), program->stack->POP<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_ARGS_2)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2 * 8),
				Instruction(Opcodes::ADD_I64),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::PUSH_I64, 2),
				Instruction(Opcodes::CALL_ARGS, 0, 2 * 8),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->IP = 3;

			ExecuteProgramWithOffset(program, 3);

			Assert::AreEqual((8), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(2 + 69), program->stack->PEEK<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FIBO_RECCURSIVE_30)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 4, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I32, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_I32, 1, 0, 0, 0),
				Instruction(Opcodes::LESS_OR_EQUAL_I32, 0, 0, 0, 0),
				Instruction(Opcodes::JMP_IF_ZERO, 9, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I32, 0, 0, 0, 0),
				Instruction(Opcodes::POP_TO_SPTR_I32, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::JMP, 9, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I32, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_I32, 1, 0, 0, 0),
				Instruction(Opcodes::SUB_I32, 0, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 4, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I32, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_I32, 2, 0, 0, 0),
				Instruction(Opcodes::SUB_I32, 0, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 4, 0, 0),
				Instruction(Opcodes::ADD_I32, 0, 0, 0, 0),
				Instruction(Opcodes::POP_TO_SPTR_I32, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::DISCARD, 4, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),

				Instruction(Opcodes::PUSH_I32, 30, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 4, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::STOP, 0, 0, 0, 0),

			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->IP = 22;

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4, program->stack->GetPushedSize(), L"Pushing 30");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 + 4, program->stack->GetPushedSize(), L"Called fib(30)");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 3, program->stack->GetPushedSize(), L"Called RESERVE 4");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 4, program->stack->GetPushedSize(), L"Push N to stack");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 5, program->stack->GetPushedSize(), L"Push 1 to stack");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 3 + 1, program->stack->GetPushedSize(), L"Compare N <= 1");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 3, program->stack->GetPushedSize(), L"Jump if zero");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 4, program->stack->GetPushedSize(), L"Push N to stack");

			FreeProgram(program);
		}

		TEST_METHOD(RESERVE_AND_POP_SPTR_TEST)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 8),
				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::PUSH_I64, 10),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual(2 * 8, program->stack->GetPushedSize());
			Assert::AreEqual((long long)(69), program->stack->POP<long long>());
			Assert::AreEqual((long long)(10), program->stack->POP<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(RESERVE_ONLY)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				//Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_I64, 69),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			//Assert::AreEqual((long long)2, program->stack->GetPushedSize());
			Assert::AreEqual((long long)(69), program->stack->POP<long long>());
			//Assert::AreEqual((long long)(0), program->stack->POP());

			FreeProgram(program);

		}

		TEST_METHOD(EXECUTE_FUNCTION_LOCAL_VAR_TEST)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 8),
				Instruction(Opcodes::PUSH_I64, 5),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 1 * 8),
				Instruction(Opcodes::RETURN),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteFunction(program, 0);

			Assert::AreEqual(1 * 8, program->stack->GetPushedSize());

			FreeProgram(program);

			Assert::AreEqual((long long)(5), res);
		}

		TEST_METHOD(TEST_REPEAT)
		{
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_I64, 5),
				Instruction(Opcodes::PUSH_I64, 10),
				Instruction(Opcodes::REPEAT),
				Instruction(Opcodes::PUSH_I64, 1),
				Instruction(Opcodes::ADD_I64),
				Instruction(Opcodes::PUSH_I64, 1),
				Instruction(Opcodes::ADD_I64),
				Instruction(Opcodes::REPEAT_END),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteFunction(program, 0);

			FreeProgram(program);

			Assert::AreEqual((long long)(5 + 20), res);
		}

		TEST_METHOD(COPY_FROM_SPTR)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2 * 8),
				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::POP_TO_SPTR_I64),
				Instruction(Opcodes::COPY_FROM_SPTR_TO_SPTR, 0, 8),
				Instruction(Opcodes::PUSH_SPTR_I64, 0),
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
				Instruction(Opcodes::RESERVE, 2 * sizeof(long long)),
				Instruction(Opcodes::PUSH_I64, 70),
				Instruction(Opcodes::PUSH_I64, 5),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0),
				Instruction(Opcodes::POP_TO_SPTR_I64, 1 * 8),
				Instruction(Opcodes::ADD_I64),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual((unsigned long long)1, program->stack->GetPushedSize() / sizeof(long long));

			Assert::AreEqual((long long)(75), program->stack->POP<long long>());

			Assert::AreEqual(0, program->stack->GetPushedSize());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_ARGS_COMPLEX)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2 * 8),
				Instruction(Opcodes::PUSH_SPTR_I64, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 8),
				Instruction(Opcodes::ADD_I64, 5),
				Instruction(Opcodes::ADD_I64),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0),
				Instruction(Opcodes::DISCARD, 1 * 8),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 2),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::PUSH_I64, 169),
				Instruction(Opcodes::PUSH_I64, 1),
				Instruction(Opcodes::CALL_ARGS, 0, 2 * 8),
				Instruction(Opcodes::RETURN),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteFunction(program, 10);

			Assert::AreEqual((1 * 8), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(169 + 1 + 5), program->stack->PEEK<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(TEST_CONDITIONAL_JUMP_IF_ZERO_TRUE_COND)
		{
			Instruction instructions[]{

				Instruction(Opcodes::PUSH_I64, 2),
				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::LESS_OR_EQUAL_I64),
				Instruction(Opcodes::JMP_IF_ZERO, 5),
				Instruction(Opcodes::PUSH_I64, 42),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual((8), program->stack->GetPushedSize());
			Assert::AreEqual((long long)(42), program->stack->PEEK<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(TEST_CONDITIONAL_JUMP_IF_ZERO_FALSE_COND)
		{
			Instruction instructions[]{

				Instruction(Opcodes::PUSH_I64, 69),
				Instruction(Opcodes::PUSH_I64, 2),
				Instruction(Opcodes::LESS_OR_EQUAL_I64),
				Instruction(Opcodes::JMP_IF_ZERO, 5),
				Instruction(Opcodes::PUSH_I64, 42),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual((0), program->stack->GetPushedSize());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_RECCURSIVE)
		{
			Instruction instructions[]{

				Instruction(Opcodes::RESERVE, 1 * 8, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_I64, 1, 0, 0, 0),
				Instruction(Opcodes::LESS_OR_EQUAL_I64, 0, 0, 0, 0),
				Instruction(Opcodes::JMP_IF_ZERO, 7, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::JMP, 7, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_I64, -1, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1 * 8, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_I64, -2, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1 * 8, 0, 0),
				Instruction(Opcodes::ADD_I64, 0, 0, 0, 0),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::DISCARD, 1 * 8, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),

				Instruction(Opcodes::RESERVE, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_I64, 2, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1 * 8, 0, 0),
				Instruction(Opcodes::STOP, 0, 0, 0, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);


			program->IP = 18;

			Assert::AreEqual(4, program->instructions[0].safeToExecuteBlindlyCount, L"SAFE TO EXECUTE");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((0 * 8), program->stack->GetPushedSize(), L"RESERVE");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((1 * 8), program->stack->GetPushedSize(), L"PUSH_CONST");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual(4 * 2, program->stack->GetPushedSize(), L"CALL");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((4 * 2) + 8, program->stack->GetPushedSize(), L"RESERVE");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((4 * 2) + 8 * 2, program->stack->GetPushedSize(), L"PUSH_SPTR");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((4 * 2) + 8 * 3, program->stack->GetPushedSize(), L"PUSH_CONST");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((4 * 2) + 8 * 2, program->stack->GetPushedSize(), L"LESS_OR_EQUAL");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((4 * 2) + 8, program->stack->GetPushedSize(), L"JMP_IF_ZERO");
			Assert::AreEqual((7), program->IP, L"JMP_IF_ZERO, IP");

			while (program->IP < arrSize)
				ExecuteInstruction(program, program->stack);

			Assert::AreEqual((1 * 8), program->stack->GetPushedSize(), L"RETURN");

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_RECCURSIVE_FREEDOM)
		{
			Instruction instructions[]{

				Instruction(Opcodes::RESERVE, 1 * 8, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_I64, 1, 0, 0, 0),
				Instruction(Opcodes::LESS_OR_EQUAL_I64, 0, 0, 0, 0),
				Instruction(Opcodes::JMP_IF_ZERO, 7, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::JMP, 7, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_I64, -1, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1 * 8, 0, 0),
				Instruction(Opcodes::PUSH_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_I64, -2, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1 * 8, 0, 0),
				Instruction(Opcodes::ADD_I64),
				Instruction(Opcodes::POP_TO_SPTR_I64, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::PUSH_I64, 30, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1 * 8, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),

				Instruction(Opcodes::STOP, 0, 0, 0, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteFunction(program, 16);

			Assert::AreEqual((1 * 8), program->stack->GetPushedSize());
			Assert::AreEqual((long long)(832040), program->stack->PEEK<long long>());

			FreeProgram(program);
		}

		TEST_METHOD(SIMPLE_JMP)
		{
			Instruction instructions[]{

				Instruction(Opcodes::JMP, 1),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((1), program->IP);
			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((arrSize), program->IP);

			FreeProgram(program);
		}
	};
}
