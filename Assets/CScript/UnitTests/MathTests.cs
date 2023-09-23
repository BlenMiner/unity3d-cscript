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

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.ADD_INT, 5);

            program.RunWithErrorLogging();

            int result = (int)stack.Pop();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.ADD_UINT, 5);

            program.RunWithErrorLogging();

            uint result = (uint)stack.Pop();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.ADD_LONG, 5);

            program.RunWithErrorLogging();

            long result = stack.Pop();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void ADD_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.ADD_ULONG, 5);

            program.RunWithErrorLogging();

            ulong result = (ulong)stack.Pop();
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public unsafe void ADD_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.ADD_FLOAT, 5.0f);

            program.RunWithErrorLogging();

            var r = stack.Pop();
            float result = *(float*)&r;
            Assert.AreEqual(10.0f, result);
        }
        
        [Test]
        public unsafe void ADD_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.ADD_DOUBLE, 5.0);

            program.RunWithErrorLogging();

            var r = stack.Pop();
            double result = *(double*)&r;
            Assert.AreEqual(10.0, result);
        }
        
        [Test]
        public void SUB_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 1);
            compiled.Add(Opcodes.SUB_INT);

            program.RunWithErrorLogging();

            int result = (int)stack.Pop();
            Assert.AreEqual(4, result);
        }
        
        [Test]
        public void SUB_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.SUB_UINT, 5);

            program.RunWithErrorLogging();

            uint result = (uint)stack.Pop();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void SUB_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.SUB_LONG, 5);

            program.RunWithErrorLogging();

            long result = stack.Pop();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void SUB_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.SUB_ULONG, 5);

            program.RunWithErrorLogging();

            ulong result = (ulong)stack.Pop();
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public unsafe void SUB_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.SUB_FLOAT, 5.0f);
            compiled.Add(Opcodes.POP);

            program.RunWithErrorLogging();

            var res = stack.Operand;
            Assert.AreEqual(0.0f, *(float*)&res);
        }
        
        [Test]
        public unsafe void SUB_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.SUB_DOUBLE, 5.0);

            program.RunWithErrorLogging();

            var r = stack.Pop();
            double result = *(double*)&r;
            Assert.AreEqual(0.0, result);
        }
        
        [Test]
        public void MUL_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.MUL_INT, 5);

            program.RunWithErrorLogging();

            int result = (int)stack.Pop();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.MUL_UINT, 5);

            program.RunWithErrorLogging();

            uint result = (uint)stack.Pop();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);

            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.MUL_LONG, 5);

            program.RunWithErrorLogging();

            long result = stack.Pop();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public void MUL_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.MUL_ULONG, 5);

            program.RunWithErrorLogging();
            
            ulong result = (ulong)stack.Pop();
            Assert.AreEqual(25, result);
        }
        
        [Test]
        public unsafe void MUL_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.MUL_FLOAT, 5.0f);

            program.RunWithErrorLogging();
            
            var r = stack.Pop();
            float result = *(float*)&r;
            Assert.AreEqual(5.0f * 5.0f, result);
        }
        
        [Test]
        public unsafe void MUL_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.MUL_DOUBLE, 5.0);

            program.RunWithErrorLogging();
            
            var r = stack.Pop();
            double result = *(double*)&r;
            Assert.AreEqual(5.0 * 5.0, result);
        }
        
        [Test]
        public void DIV_INT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.DIV_INT, 5);

            program.RunWithErrorLogging();
            
            int result = (int)stack.Pop();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_UINT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);

            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.DIV_UINT, 5);

            program.RunWithErrorLogging();
            
            uint result = (uint)stack.Pop();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_INT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.DIV_LONG, 5);

            program.RunWithErrorLogging();
            
            long result = stack.Pop();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public void DIV_UINT64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.PUSH, 5);
            compiled.Add(Opcodes.DIV_ULONG, 5);

            program.RunWithErrorLogging();
            
            ulong result = (ulong)stack.Pop();
            Assert.AreEqual(1, result);
        }
        
        [Test]
        public unsafe void DIV_FLOAT32()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.PUSH, 5.0f);
            compiled.Add(Opcodes.DIV_FLOAT, 5.0f);

            program.RunWithErrorLogging();
            
            var r = stack.Pop();
            float result = *(float*)&r;
            Assert.AreEqual(1.0f, result);
        }
        
        [Test]
        public unsafe void DIV_DOUBLE64()
        {
            var stack = new CScriptStack();
            var compiled = new CScriptCompiled();
            var program = new CScriptProgram(compiled, stack);
            Assert.AreEqual(0, stack.StackByteSize);
            
            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.PUSH, 5.0);
            compiled.Add(Opcodes.DIV_DOUBLE, 5.0);

            program.RunWithErrorLogging();
            
            var r = stack.Pop();
            double result = *(double*)&r;
            Assert.AreEqual(1.0, result);
        }
    }
}