using Riten.CScript.Lexer;
using Riten.CScript.Native;
using UnityEngine;

namespace Riten.CScript.Compiler
{
    public struct LocalVariableInfo
    {
        public int StackPointer;
        public int StackSize;
        public int Level;
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
        
        public CompiledFunction(CTCompiler compiler, Scope scope, CTFunction function, int level)
            :base(compiler)
        {
            Function = function;
            FunctionName = function.FunctionName.Span.Content;

            var fnScope = new Scope(compiler, scope, false);
            
            Compile(compiler, fnScope, level);
        }
        
        static void RegisterAllLocalVariables(Scope scope, CTStatement statement, int level)
        {
            switch (statement)
            {
                case CTDeclareStatement declare:
                    scope.RegisterNewVariable(declare.Identifier.Span.Content, 1, level);
                    break;
                case CTRepeatBlockStatement repeatBlock:
                    RegisterAllLocalVariables(scope, repeatBlock.BlockStatement, level + 1);
                    break;
            }
        }

        static void RegisterAllLocalVariables(Scope scope, CTBlockStatement blockStatement, int level = 0)
        {
            foreach (var node in blockStatement.Statements)
            {
                RegisterAllLocalVariables(scope, node, level);
            }
        }

        void Compile(CTCompiler compiler, Scope scope, int level)
        {
            FunctionPtr = compiler.Instructions.Count;

            var args = Function.Arguments.Values;

            for (int i = 0; i < args.Count; ++i)
            {
                var argument = args[i];
                scope.RegisterNewVariable(argument.ArgumentName.Span.Content, 1, level);
            }
            
            RegisterAllLocalVariables(scope, Function.BlockStatement);

            LocalDeclarationsSize = scope.StackSize;
            compiler.Instructions.Add(new Instruction(Opcodes.RESERVE, LocalDeclarationsSize));

            Body = new CompiledBlock(compiler, scope, Function.BlockStatement, level);

            compiler.Instructions.Add(new Instruction(Opcodes.DISCARD, LocalDeclarationsSize));
            compiler.Instructions.Add(new Instruction(Opcodes.RETURN));
        }
    }
}