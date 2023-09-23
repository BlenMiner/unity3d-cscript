using NUnit.Framework;

namespace CScript.UnitTests
{
    public static class TestsHelper
    {
        public static CScriptProgram GetProgram(out CScriptCompiled code)
        {
            var stack = new CScriptStack();
            code = new CScriptCompiled();
            return new CScriptProgram(code, stack);
        }
    }
    public class EqualityTests
    {
        static CScriptProgram GetProgram(out CScriptCompiled code)
        {
            return TestsHelper.GetProgram(out code);
        }
        
        [Test]
        public void TEST_EQUAL_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, -69);
            code.Add(Opcodes.PUSH, -69);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 12345678);
            code.Add(Opcodes.PUSH, 12345678);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 12345678);
            code.Add(Opcodes.PUSH, 12345678);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 123456789012345L);
            code.Add(Opcodes.PUSH, 123456789012345L);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 123456789012345UL);
            code.Add(Opcodes.PUSH, 123456789012345UL);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3.14f);
            code.Add(Opcodes.PUSH, 3.14f);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_EQUAL_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3.141592653589793);
            code.Add(Opcodes.PUSH, 3.141592653589793);
            code.Add(Opcodes.EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.PUSH, 70);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, -69);
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.PUSH, -32000);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.PUSH, 32001);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 12345678);
            code.Add(Opcodes.PUSH, 87654321);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 12345678);
            code.Add(Opcodes.PUSH, 87654321);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 123456789012345L);
            code.Add(Opcodes.PUSH, 543210987654321L);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 123456789012345UL);
            code.Add(Opcodes.PUSH, 543210987654321UL);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3.14f);
            code.Add(Opcodes.PUSH, 2.71f);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_NOT_EQUAL_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3.141592653589793);
            code.Add(Opcodes.PUSH, 2.718281828459045);
            code.Add(Opcodes.NOT_EQUAL);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 70);
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.GREATER_THAN_BYTE);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 1);
            code.Add(Opcodes.PUSH, -1);
            code.Add(Opcodes.GREATER_THAN_SBYTE);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.PUSH, 31000);
            code.Add(Opcodes.GREATER_THAN_SHORT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 65000);
            code.Add(Opcodes.PUSH, 64000);
            code.Add(Opcodes.GREATER_THAN_USHORT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 2000000000);
            code.Add(Opcodes.PUSH, 1000000000);
            code.Add(Opcodes.GREATER_THAN_INT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3000000000);
            code.Add(Opcodes.PUSH, 2500000000);
            code.Add(Opcodes.GREATER_THAN_UINT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 5000000000000000000);
            code.Add(Opcodes.PUSH, 4000000000000000000);
            code.Add(Opcodes.GREATER_THAN_LONG);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 9000000000000000000);
            code.Add(Opcodes.PUSH, 8000000000000000000);
            code.Add(Opcodes.GREATER_THAN_ULONG);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3.14f);
            code.Add(Opcodes.PUSH, 2.71f);
            code.Add(Opcodes.GREATER_THAN_FLOAT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_GREATER_THAN_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 3.141592653589793);
            code.Add(Opcodes.PUSH, 2.718281828459045);
            code.Add(Opcodes.GREATER_THAN_DOUBLE);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 68);
            code.Add(Opcodes.PUSH, 69);
            code.Add(Opcodes.LESS_BYTE);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, -2);
            code.Add(Opcodes.PUSH, -1);
            code.Add(Opcodes.LESS_SBYTE);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 31000);
            code.Add(Opcodes.PUSH, 32000);
            code.Add(Opcodes.LESS_SHORT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 64000);
            code.Add(Opcodes.PUSH, 65000);
            code.Add(Opcodes.LESS_USHORT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 1000000000);
            code.Add(Opcodes.PUSH, 2000000000);
            code.Add(Opcodes.LESS_INT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 2500000000);
            code.Add(Opcodes.PUSH, 3000000000);
            code.Add(Opcodes.LESS_UINT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 4000000000000000000);
            code.Add(Opcodes.PUSH, 5000000000000000000);
            code.Add(Opcodes.LESS_LONG);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 8000000000000000000);
            code.Add(Opcodes.PUSH, 9000000000000000000);
            code.Add(Opcodes.LESS_ULONG);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 2.71f);
            code.Add(Opcodes.PUSH, 3.14f);
            code.Add(Opcodes.LESS_FLOAT);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

        [Test]
        public void TEST_LESS_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH, 2.718281828459045);
            code.Add(Opcodes.PUSH, 3.141592653589793);
            code.Add(Opcodes.LESS_DOUBLE);
            code.Add(Opcodes.POP);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.Operand);
        }

    }
}