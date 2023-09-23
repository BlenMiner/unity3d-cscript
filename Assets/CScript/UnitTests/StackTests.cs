using NUnit.Framework;

namespace CScript.UnitTests
{
    public class StackTests
    {
        [Test]
        public void PUSH_INT()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            compiled.Add(Opcodes.PUSH_INT, int.MinValue);
            program.Step();
            
            Assert.AreEqual(4, stack.StackByteSize);
            Assert.AreEqual(int.MinValue, stack.PopInt());
        }
        
        [Test]
        public void PUSH_UINT()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_UINT, uint.MaxValue);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            uint val = stack.PopUInt();
            Assert.AreEqual(uint.MaxValue, val);
        }
        
        [Test]
        public void PUSH_LONG()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_LONG, long.MinValue);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            long val = stack.PopLong();
            Assert.AreEqual(long.MinValue, val);
        }
        
        [Test]
        public void PUSH_ULONG()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_ULONG, ulong.MaxValue);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            ulong val = stack.PopULong();
            Assert.AreEqual(ulong.MaxValue, val);
        }
        
        [Test]
        public void PUSH_SHORT()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_SHORT, short.MinValue);

            program.Step();
            Assert.AreEqual(2, stack.StackByteSize);

            short val = stack.PopShort();
            Assert.AreEqual(short.MinValue, val);
        }
        
        [Test]
        public void PUSH_USHORT()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_USHORT, ushort.MaxValue);

            program.Step();
            Assert.AreEqual(2, stack.StackByteSize);

            ushort val = stack.PopUShort();
            Assert.AreEqual(ushort.MaxValue, val);
        }
        
        [Test]
        public void PUSH_BYTE()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_BYTE, byte.MinValue);

            program.Step();
            Assert.AreEqual(1, stack.StackByteSize);

            byte val = stack.PopByte();
            Assert.AreEqual(byte.MinValue, val);
        }
        
        [Test]
        public void PUSH_SBYTE()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
        
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_SBYTE, sbyte.MinValue);

            program.Step();
            Assert.AreEqual(1, stack.StackByteSize);

            sbyte val = stack.PopSByte();
            Assert.AreEqual(sbyte.MinValue, val);
        }
        
        [Test]
        public void PUSH_FLOAT()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            compiled.Add(Opcodes.PUSH_FLOAT, 0.5f);
            program.Step();
            
            Assert.AreEqual(4, stack.StackByteSize);
            Assert.AreEqual(0.5f, stack.PopFloat());
        }
        
        [Test]
        public void PUSH_DOUBLE()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            compiled.Add(Opcodes.PUSH_DOUBLE, 0.5);
            program.Step();
            
            Assert.AreEqual(8, stack.StackByteSize);
            Assert.AreEqual(0.5, stack.PopDouble());
        }
    }
}