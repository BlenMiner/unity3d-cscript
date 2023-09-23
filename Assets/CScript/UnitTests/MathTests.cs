using NUnit.Framework;

namespace CScript.UnitTests
{
    public class MathTests
    {
        [Test]
        public void ADD_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.ADD_INT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            int result = stack.PopInt();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.ADD_UINT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            uint result = stack.PopUInt();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.ADD_LONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            long result = stack.PopLong();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.ADD_ULONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            ulong result = stack.PopULong();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.ADD_FLOAT, 5.0f);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            float result = stack.PopFloat();
            Assert.AreEqual(10.0f, result);
        }
        
        [Test]
        public void ADD_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.ADD_DOUBLE, 5.0);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            double result = stack.PopDouble();
            Assert.AreEqual(10.0, result);
        }
        
        [Test]
        public void SUB_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.SUB_INT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            int result = stack.PopInt();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void SUB_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.SUB_UINT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            uint result = stack.PopUInt();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void SUB_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.SUB_LONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            long result = stack.PopLong();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void SUB_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.SUB_ULONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            ulong result = stack.PopULong();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public unsafe void SUB_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.SUB_FLOAT, 5.0f);
            compiled.Add(Opcodes.POP_FLOAT);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(0, stack.StackByteSize);

            var res = stack.R0;
            Assert.AreEqual(0.0f, *(float*)&res);
        }
        
        [Test]
        public void SUB_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.SUB_DOUBLE, 5.0);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            double result = stack.PopDouble();
            Assert.AreEqual(0.0, result);
        }
        
        [Test]
        public void MUL_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.MUL_INT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            int result = stack.PopInt();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.MUL_UINT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);

            uint result = stack.PopUInt();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.MUL_LONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);

            long result = stack.PopLong();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.MUL_ULONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            ulong result = stack.PopULong();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.MUL_FLOAT, 5.0f);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            float result = stack.PopFloat();
            Assert.AreEqual(5.0f * 5.0f, result);
        }
        
        [Test]
        public void MUL_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.MUL_DOUBLE, 5.0);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            double result = stack.PopDouble();
            Assert.AreEqual(5.0 * 5.0, result);
        }
        
        [Test]
        public void DIV_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.PUSH_INT, 5);
            compiled.Add(Opcodes.DIV_INT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            int result = stack.PopInt();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.PUSH_UINT, 5);
            compiled.Add(Opcodes.DIV_UINT, 5);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            uint result = stack.PopUInt();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.PUSH_LONG, 5);
            compiled.Add(Opcodes.DIV_LONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            long result = stack.PopLong();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.PUSH_ULONG, 5);
            compiled.Add(Opcodes.DIV_ULONG, 5);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            ulong result = stack.PopULong();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.PUSH_FLOAT, 5.0f);
            compiled.Add(Opcodes.DIV_FLOAT, 5.0f);

            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(4, stack.StackByteSize);
            
            float result = stack.PopFloat();
            Assert.AreEqual(1.0f, result);
        }
        
        [Test]
        public void DIV_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.PUSH_DOUBLE, 5.0);
            compiled.Add(Opcodes.DIV_DOUBLE, 5.0);

            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(16, stack.StackByteSize);
            
            program.Step();
            Assert.AreEqual(8, stack.StackByteSize);
            
            double result = stack.PopDouble();
            Assert.AreEqual(1.0, result);
        }
    }
}