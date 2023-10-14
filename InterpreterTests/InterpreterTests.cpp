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

		TEST_METHOD(PUSH_SPTR_AND_CONST)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 42, 0),
				Instruction(Opcodes::PUSH_SPTR_AND_CONST, 0, 69),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 40, 0),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual((long long)3, program->stack->GetPushedSize());
			Assert::AreEqual((long long)69, program->stack->POP());
			Assert::AreEqual((long long)42, program->stack->POP());
			Assert::AreEqual((long long)40, program->stack->POP());

			FreeProgram(program);
		}

		TEST_METHOD(CONDITION_LESS_OR_EQUAL_FALSE)
		{
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::LESS_OR_EQUAL),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());
			Assert::AreEqual((long long)0, res);

			FreeProgram(program);
		}

		TEST_METHOD(CONDITION_LESS_OR_EQUAL_TRUE)
		{
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::LESS_OR_EQUAL),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);
			long long res = ExecuteProgram(program);

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());
			Assert::AreEqual((long long)1, res);

			FreeProgram(program);
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

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());

			FreeProgram(program);

			Assert::AreEqual((long long)40, res);
		}

		TEST_METHOD(EXECUTE_FUNCTION)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 0),
				Instruction(Opcodes::ADD_CONST, 6),
				Instruction(Opcodes::ADD_CONST, 69),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::STOP),
				Instruction(Opcodes::CALL, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->IP = 6;

			ExecuteInstruction(program, program->stack);

			Assert::AreEqual((long long)2, program->stack->GetPushedSize());

			ExecuteInstruction(program, program->stack);

			Assert::AreEqual((long long)3, program->stack->GetPushedSize());

			ExecuteInstruction(program, program->stack);

			Assert::AreEqual((long long)3, program->stack->GetPushedSize());

			ExecuteInstruction(program, program->stack);
			ExecuteInstruction(program, program->stack);
			ExecuteInstruction(program, program->stack);

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());

			Assert::AreEqual((long long)(6 +69), program->stack->PEEK());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_2)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 0),
				Instruction(Opcodes::ADD_CONST, 6),
				Instruction(Opcodes::ADD_CONST, 69),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::CALL, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgramWithOffset(program, 5);

			Assert::AreEqual((long long)1, program->stack->GetPushedSize());
			Assert::AreEqual((long long)(6 + 69), program->stack->PEEK());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_ARGS)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::ADD_CONST_TO_SPTR, 2, 0),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::CALL_ARGS, 0, 1),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->IP = 3;

			ExecuteProgramWithOffset(program, 3);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(2 + 69), program->stack->PEEK());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_ARGS_2)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2),
				Instruction(Opcodes::ADD),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::PUSH_CONST, 2),
				Instruction(Opcodes::CALL_ARGS, 0, 2),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->IP = 3;

			ExecuteProgramWithOffset(program, 3);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(2 + 69), program->stack->PEEK());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_ARGS_3)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 10, 0),
				Instruction(Opcodes::REPEAT_CONST, 10),
				Instruction(Opcodes::PUSH_SPTR, 0),
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::CALL_ARGS, 11, 2),
				Instruction(Opcodes::POP_TO_SPTR, 0),
				Instruction(Opcodes::REPEAT_END),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 1),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::RESERVE, 2),
				Instruction(Opcodes::PUSH_SPTR, 0),
				Instruction(Opcodes::PUSH_SPTR, 1),
				Instruction(Opcodes::ADD_CONST, 5),
				Instruction(Opcodes::ADD),
				Instruction(Opcodes::POP_TO_SPTR, 0),
				Instruction(Opcodes::DISCARD, 1),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 2),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			program->IP = 3;

			ExecuteFunction(program, 0);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(110), program->stack->PEEK());

			FreeProgram(program);
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

		TEST_METHOD(RESERVE_ONLY)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				//Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST, 69),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			//Assert::AreEqual((long long)2, program->stack->GetPushedSize());
			Assert::AreEqual((long long)(69), program->stack->POP());
			//Assert::AreEqual((long long)(0), program->stack->POP());

			FreeProgram(program);

		}

		TEST_METHOD(EXECUTE_FUNCTION_LOCAL_VAR_TEST)
		{
			const int LOOP = 20;

			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 1),
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::POP_TO_SPTR, 0),
				Instruction(Opcodes::PUSH_SPTR, 0),
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
			Instruction instructions[]{
				Instruction(Opcodes::PUSH_CONST, 5),
				Instruction(Opcodes::PUSH_CONST, 10),
				Instruction(Opcodes::REPEAT),
				Instruction(Opcodes::ADD_CONST, 1),
				Instruction(Opcodes::ADD_CONST, 1),
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
				Instruction(Opcodes::RESERVE, 2),
				Instruction(Opcodes::PUSH_CONST_TO_SPTR, 69, 0),
				Instruction(Opcodes::COPY_FROM_SPTR_TO_SPTR, 0, 1),
				Instruction(Opcodes::PUSH_SPTR, 1),
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

		TEST_METHOD(EXECUTE_FUNCTION_ARGS_COMPLEX)
		{
			Instruction instructions[]{
				Instruction(Opcodes::RESERVE, 2),
				Instruction(Opcodes::PUSH_SPTR, 0),
				Instruction(Opcodes::PUSH_SPTR, 1),
				Instruction(Opcodes::ADD_CONST, 5),
				Instruction(Opcodes::ADD),
				Instruction(Opcodes::POP_TO_SPTR, 0),
				Instruction(Opcodes::DISCARD, 1),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 2),
				Instruction(Opcodes::RETURN),

				Instruction(Opcodes::RESERVE, 0),
				Instruction(Opcodes::PUSH_CONST, 169),
				Instruction(Opcodes::PUSH_CONST, 1),
				Instruction(Opcodes::CALL_ARGS, 0, 2),
				Instruction(Opcodes::RETURN),
				Instruction(Opcodes::DISCARD, 0),
				Instruction(Opcodes::RETURN),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteFunction(program, 10);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());

			Assert::AreEqual((long long)(169 + 1 + 5), program->stack->PEEK());

			FreeProgram(program);
		}

		TEST_METHOD(TEST_CONDITIONAL_JUMP_IF_ZERO_TRUE_COND)
		{
			Instruction instructions[]{

				Instruction(Opcodes::PUSH_CONST, 2),
				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::LESS_OR_EQUAL),
				Instruction(Opcodes::JMP_IF_ZERO, 5),
				Instruction(Opcodes::PUSH_CONST, 42),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());
			Assert::AreEqual((long long)(42), program->stack->PEEK());

			FreeProgram(program);
		}

		TEST_METHOD(TEST_CONDITIONAL_JUMP_IF_ZERO_FALSE_COND)
		{
			Instruction instructions[]{

				Instruction(Opcodes::PUSH_CONST, 69),
				Instruction(Opcodes::PUSH_CONST, 2),
				Instruction(Opcodes::LESS_OR_EQUAL),
				Instruction(Opcodes::JMP_IF_ZERO, 5),
				Instruction(Opcodes::PUSH_CONST, 42),
				Instruction(Opcodes::STOP),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteProgram(program);

			Assert::AreEqual((long long)(0), program->stack->GetPushedSize());

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_RECCURSIVE)
		{
			Instruction instructions[]{

				Instruction(Opcodes::RESERVE, 1, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_CONST, 1, 0, 0, 0),
				Instruction(Opcodes::LESS_OR_EQUAL, 0, 0, 0, 0),
				Instruction(Opcodes::JMP_IF_ZERO, 7, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::JMP, 7, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_CONST, -1, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1, 0, 0),
				Instruction(Opcodes::PUSH_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_CONST, -2, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1, 0, 0),
				Instruction(Opcodes::ADD, 0, 0, 0, 0),
				Instruction(Opcodes::POP_TO_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::DISCARD, 1, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),

				Instruction(Opcodes::RESERVE, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_CONST, 2, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1, 0, 0),
				Instruction(Opcodes::STOP, 0, 0, 0, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);


			program->IP = 18;

			Assert::AreEqual(4, program->instructions[0].safeToExecuteBlindlyCount, L"SAFE TO EXECUTE");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(0), program->stack->GetPushedSize(), L"RESERVE");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(1), program->stack->GetPushedSize(), L"PUSH_CONST");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(2), program->stack->GetPushedSize(), L"CALL");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(3), program->stack->GetPushedSize(), L"RESERVE");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(4), program->stack->GetPushedSize(), L"PUSH_SPTR");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(5), program->stack->GetPushedSize(), L"PUSH_CONST");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(4), program->stack->GetPushedSize(), L"LESS_OR_EQUAL");

			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((long long)(3), program->stack->GetPushedSize(), L"JMP_IF_ZERO");
			Assert::AreEqual((unsigned long long)(7), program->IP, L"JMP_IF_ZERO, IP");

			while (program->IP < arrSize)
				ExecuteInstruction(program, program->stack);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize(), L"RETURN");

			FreeProgram(program);
		}

		TEST_METHOD(EXECUTE_FUNCTION_RECCURSIVE_FREEDOM)
		{
			Instruction instructions[]{

				Instruction(Opcodes::RESERVE, 1, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_CONST, 1, 0, 0, 0),
				Instruction(Opcodes::LESS_OR_EQUAL, 0, 0, 0, 0),
				Instruction(Opcodes::JMP_IF_ZERO, 7, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::JMP, 7, 0, 0, 0),
				Instruction(Opcodes::PUSH_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_CONST, -1, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1, 0, 0),
				Instruction(Opcodes::PUSH_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::ADD_CONST, -2, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1, 0, 0),
				Instruction(Opcodes::ADD, 0, 0, 0, 0),
				Instruction(Opcodes::POP_TO_SPTR, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::DISCARD, 1, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),

				Instruction(Opcodes::RESERVE, 0, 0, 0, 0),
				Instruction(Opcodes::PUSH_CONST, 30, 0, 0, 0),
				Instruction(Opcodes::CALL_ARGS, 0, 1, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::DISCARD, 0, 0, 0, 0),
				Instruction(Opcodes::RETURN, 0, 0, 0, 0),
				Instruction(Opcodes::STOP, 0, 0, 0, 0),
			};

			int arrSize = sizeof(instructions) / sizeof(Instruction);
			auto program = CreateProgram(instructions, arrSize);

			ExecuteFunction(program, 18);

			Assert::AreEqual((long long)(1), program->stack->GetPushedSize());
			Assert::AreEqual((long long)(832040), program->stack->PEEK());

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
			Assert::AreEqual((unsigned long long)(1), program->IP);
			ExecuteInstruction(program, program->stack);
			Assert::AreEqual((unsigned long long)(arrSize), program->IP);

			FreeProgram(program);
		}
	};
}
