using NUnit.Framework;
using Riten.CScript.Native;

namespace CScript.UnitTests
{
    public class MathTests
    {
        [Test]
        public void ADD()
        {
            Interoperability.Init();

            if (!Interoperability.IsInitialized)
                Assert.Fail("Could not initialize native side.");
            
            var program = new[]
            {
                new Instruction(Opcodes.PUSH_I64, 6),
                new Instruction(Opcodes.ADD_I64, 4),
            };
            

            var programPtr = Interoperability.CreateProgramFunc(program, program.Length);
            var result = Interoperability.ExecuteProgramFunc(programPtr);
            Interoperability.FreeProgramFunc(programPtr);
            
            Interoperability.Dispose();

            Assert.AreEqual(10, result);
        }
    }
}