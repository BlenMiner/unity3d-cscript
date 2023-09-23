using NUnit.Framework;

namespace CScript.UnitTests
{
    public class EqualityTests
    {
        static CScriptProgram GetProgram(out CScriptCompiled code)
        {
            var stack = new CScriptStack();
            code = new CScriptCompiled();
            return new CScriptProgram(code, stack);
        }
        
        [Test]
        public void TEST_EQUAL_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_BYTE, 69);
            code.Add(Opcodes.PUSH_BYTE, 69);
            code.Add(Opcodes.EQUAL_BYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SBYTE, -69);
            code.Add(Opcodes.PUSH_SBYTE, -69);
            code.Add(Opcodes.EQUAL_SBYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SHORT, 32000);
            code.Add(Opcodes.PUSH_SHORT, 32000);
            code.Add(Opcodes.EQUAL_SHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_USHORT, 32000);
            code.Add(Opcodes.PUSH_USHORT, 32000);
            code.Add(Opcodes.EQUAL_USHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_INT, 12345678);
            code.Add(Opcodes.PUSH_INT, 12345678);
            code.Add(Opcodes.EQUAL_INT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_UINT, 12345678);
            code.Add(Opcodes.PUSH_UINT, 12345678);
            code.Add(Opcodes.EQUAL_UINT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_LONG, 123456789012345L);
            code.Add(Opcodes.PUSH_LONG, 123456789012345L);
            code.Add(Opcodes.EQUAL_LONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_ULONG, 123456789012345UL);
            code.Add(Opcodes.PUSH_ULONG, 123456789012345UL);
            code.Add(Opcodes.EQUAL_ULONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_FLOAT, 3.14f);
            code.Add(Opcodes.PUSH_FLOAT, 3.14f);
            code.Add(Opcodes.EQUAL_FLOAT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_EQUAL_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_DOUBLE, 3.141592653589793);
            code.Add(Opcodes.PUSH_DOUBLE, 3.141592653589793);
            code.Add(Opcodes.EQUAL_DOUBLE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_BYTE, 69);
            code.Add(Opcodes.PUSH_BYTE, 70);
            code.Add(Opcodes.NOT_EQUAL_BYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SBYTE, -69);
            code.Add(Opcodes.PUSH_SBYTE, 69);
            code.Add(Opcodes.NOT_EQUAL_SBYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SHORT, 32000);
            code.Add(Opcodes.PUSH_SHORT, -32000);
            code.Add(Opcodes.NOT_EQUAL_SHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_USHORT, 32000);
            code.Add(Opcodes.PUSH_USHORT, 32001);
            code.Add(Opcodes.NOT_EQUAL_USHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_INT, 12345678);
            code.Add(Opcodes.PUSH_INT, 87654321);
            code.Add(Opcodes.NOT_EQUAL_INT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_UINT, 12345678);
            code.Add(Opcodes.PUSH_UINT, 87654321);
            code.Add(Opcodes.NOT_EQUAL_UINT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_LONG, 123456789012345L);
            code.Add(Opcodes.PUSH_LONG, 543210987654321L);
            code.Add(Opcodes.NOT_EQUAL_LONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_ULONG, 123456789012345UL);
            code.Add(Opcodes.PUSH_ULONG, 543210987654321UL);
            code.Add(Opcodes.NOT_EQUAL_ULONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_FLOAT, 3.14f);
            code.Add(Opcodes.PUSH_FLOAT, 2.71f);
            code.Add(Opcodes.NOT_EQUAL_FLOAT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_NOT_EQUAL_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_DOUBLE, 3.141592653589793);
            code.Add(Opcodes.PUSH_DOUBLE, 2.718281828459045);
            code.Add(Opcodes.NOT_EQUAL_DOUBLE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_BYTE, 70);
            code.Add(Opcodes.PUSH_BYTE, 69);
            code.Add(Opcodes.GREATER_THAN_BYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SBYTE, 1);
            code.Add(Opcodes.PUSH_SBYTE, -1);
            code.Add(Opcodes.GREATER_THAN_SBYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SHORT, 32000);
            code.Add(Opcodes.PUSH_SHORT, 31000);
            code.Add(Opcodes.GREATER_THAN_SHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_USHORT, 65000);
            code.Add(Opcodes.PUSH_USHORT, 64000);
            code.Add(Opcodes.GREATER_THAN_USHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_INT, 2000000000);
            code.Add(Opcodes.PUSH_INT, 1000000000);
            code.Add(Opcodes.GREATER_THAN_INT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_UINT, 3000000000);
            code.Add(Opcodes.PUSH_UINT, 2500000000);
            code.Add(Opcodes.GREATER_THAN_UINT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_LONG, 5000000000000000000);
            code.Add(Opcodes.PUSH_LONG, 4000000000000000000);
            code.Add(Opcodes.GREATER_THAN_LONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_ULONG, 9000000000000000000);
            code.Add(Opcodes.PUSH_ULONG, 8000000000000000000);
            code.Add(Opcodes.GREATER_THAN_ULONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_FLOAT, 3.14f);
            code.Add(Opcodes.PUSH_FLOAT, 2.71f);
            code.Add(Opcodes.GREATER_THAN_FLOAT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_GREATER_THAN_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_DOUBLE, 3.141592653589793);
            code.Add(Opcodes.PUSH_DOUBLE, 2.718281828459045);
            code.Add(Opcodes.GREATER_THAN_DOUBLE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_BYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_BYTE, 68);
            code.Add(Opcodes.PUSH_BYTE, 69);
            code.Add(Opcodes.LESS_BYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_SBYTE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SBYTE, -2);
            code.Add(Opcodes.PUSH_SBYTE, -1);
            code.Add(Opcodes.LESS_SBYTE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_SHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_SHORT, 31000);
            code.Add(Opcodes.PUSH_SHORT, 32000);
            code.Add(Opcodes.LESS_SHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_USHORT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_USHORT, 64000);
            code.Add(Opcodes.PUSH_USHORT, 65000);
            code.Add(Opcodes.LESS_USHORT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_INT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_INT, 1000000000);
            code.Add(Opcodes.PUSH_INT, 2000000000);
            code.Add(Opcodes.LESS_INT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_UINT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_UINT, 2500000000);
            code.Add(Opcodes.PUSH_UINT, 3000000000);
            code.Add(Opcodes.LESS_UINT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_LONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_LONG, 4000000000000000000);
            code.Add(Opcodes.PUSH_LONG, 5000000000000000000);
            code.Add(Opcodes.LESS_LONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_ULONG()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_ULONG, 8000000000000000000);
            code.Add(Opcodes.PUSH_ULONG, 9000000000000000000);
            code.Add(Opcodes.LESS_ULONG);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_FLOAT()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_FLOAT, 2.71f);
            code.Add(Opcodes.PUSH_FLOAT, 3.14f);
            code.Add(Opcodes.LESS_FLOAT);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

        [Test]
        public void TEST_LESS_DOUBLE()
        {
            var program = GetProgram(out var code);
            
            code.Add(Opcodes.PUSH_DOUBLE, 2.718281828459045);
            code.Add(Opcodes.PUSH_DOUBLE, 3.141592653589793);
            code.Add(Opcodes.LESS_DOUBLE);
            code.Add(Opcodes.POP_BYTE);

            program.Run();
            
            Assert.AreEqual(1, (byte)program.Stack.R0);
        }

    }
}