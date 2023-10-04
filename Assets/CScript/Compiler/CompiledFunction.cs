using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public struct LocalVariableInfo
    {
        public int StackPointer;
        public int StackSize;
        public int ReadCount;
        public int WriteCount;
    }
    
    public class CompiledFunction : CompiledNode
    {
        public readonly CTFunction Function;
        public readonly string FunctionName;

        public CompiledBlock Body;
        
        public int LocalDeclarationsSize;
        public int FunctionPtr;
        
        public CompiledFunction(CTCompiler compiler, Scope scope, CTFunction function)
            :base(compiler)
        {
            Function = function;
            FunctionName = function.FunctionName.Span.Content;

            var fnScope = new Scope(compiler, scope, false);
            
            Compile(compiler, fnScope);
        }

        static void RegisterAllLocalVariables(Scope scope, CTBlockStatement blockStatement)
        {
            foreach (var node in blockStatement.Statements)
            {
                switch (node)
                {
                    case CTDeclareStatement statement:
                        scope.RegisterNewVariable(statement.Identifier.Span.Content, 1);
                        break;
                    case CTBlockStatement childBlock:
                        RegisterAllLocalVariables(scope, childBlock);
                        break;
                }
            }
        }

        void Compile(CTCompiler compiler, Scope scope)
        {
            FunctionPtr = compiler.Instructions.Count;
            
            RegisterAllLocalVariables(scope, Function.BlockStatement);

            LocalDeclarationsSize = scope.StackSize;
            compiler.Instructions.Add(new Instruction(Opcodes.RESERVE, LocalDeclarationsSize));

            /*for (var i = 0; i < Function.Block.Statements.Length; i++)
            {
                var node = Function.Block.Statements[i];
                CompiledBody.Add(compiler.CompileNode(scope, node));
            }*/
            
            Body = new CompiledBlock(compiler, scope, Function.BlockStatement);

            compiler.Instructions.Add(new Instruction(Opcodes.DISCARD, LocalDeclarationsSize));
            compiler.Instructions.Add(new Instruction(Opcodes.RETURN));
        }
    }
}