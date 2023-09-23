using NUnit.Framework;

namespace CScript.UnitTests
{
    public class ControlFlowTests
    {
        [Test]
        public void TEST_JMP()
        {
            var program = TestsHelper.GetProgram(out var code);
            
            code.Add(Opcodes.JMP, 5);
            code.Add(Opcodes.PUSH, 67);
            code.Add(Opcodes.PUSH, 68);
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.JMP, 0);
            code.Add(Opcodes.PUSH, 42);
            code.Add(Opcodes.JMP, 3);
            code.Add(Opcodes.PUSH, 48);
            code.Add(Opcodes.PUSH, 49);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(42, (int)program.Stack.Operand);
        }
        
        [Test]
        public void TEST_JMP_IF()
        {
            var program = TestsHelper.GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 0);
            code.Add(Opcodes.MOVE_TO_REGISTER, 0); // Move pushed int to R0 leaving stack empty
            code.Add(Opcodes.PUSH, 10);
            code.Add(Opcodes.DUPLICATE);
            code.Add(Opcodes.PUSH, 0);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.JMP_IF_TRUE, 8); // Out of range = exit
            code.Add(Opcodes.PUSH, 1);
            code.Add(Opcodes.SUB_INT);
            code.Add(Opcodes.LOAD_REGISTER, 0);
            code.Add(Opcodes.PUSH, 2);
            code.Add(Opcodes.ADD);
            code.Add(Opcodes.MOVE_TO_REGISTER, 0);
            code.Add(Opcodes.JMP, -10);
            code.Add(Opcodes.POP);
            code.Add(Opcodes.LOAD_REGISTER, 0);
            code.Add(Opcodes.POP);

            program.RunWithErrorLogging();
            
            Assert.AreEqual(0, program.Stack.StackByteSize);
            Assert.AreEqual(2 * 10, (int)program.Stack.Operand);
        }
    }
}