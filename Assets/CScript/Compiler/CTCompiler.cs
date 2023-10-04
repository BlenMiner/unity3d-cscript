using System;
using System.Collections.Generic;
using Riten.CScript.Lexer;
using Riten.CScript.Native;

namespace Riten.CScript.Compiler
{
    public abstract class CompiledNode
    {
        protected readonly CTCompiler Compiler;

        protected CompiledNode(CTCompiler compiler)
        {
            Compiler = compiler;
        }
    }

    public class Scope
    {
        public readonly Dictionary<string, LocalVariableInfo> LocalVariables = new();
        public readonly Dictionary<string, CompiledFunction> Functions = new();
        
        public readonly bool CanAccessParentScope;
        public readonly Scope ParentScope;

        private int m_stackPtrOffset = 0;
        public readonly CTCompiler Compiler;

        public Scope(CTCompiler compiler, Scope parent, bool canAccessParentScope)
        {
            Compiler = compiler;
            compiler.AllScopes.Add(this);
            ParentScope = parent;
            CanAccessParentScope = canAccessParentScope;
        }

        public int StackSize => m_stackPtrOffset;
        
        public void RegisterNewFunction(CompiledFunction function)
        {
            if (Functions.ContainsKey(function.FunctionName))
                throw new Exception($"Another function with name '{function.FunctionName}' is already declared.");
            
            Functions.Add(function.FunctionName, function);
        }

        public int RegisterNewVariable(string name, int stackSize)
        {
            int stackPtr = m_stackPtrOffset;
            LocalVariables.Add(name, new LocalVariableInfo
            {
                StackPointer = stackPtr,
                StackSize = stackSize,
                ReadCount = 0,
                WriteCount = 0
            });
            
            m_stackPtrOffset += stackSize;
            return stackPtr;
        }

        public LocalVariableInfo ReadVariable(string name)
        {
            if (LocalVariables.TryGetValue(name, out var info))
            {
                info.ReadCount++;
                LocalVariables[name] = info;
                return info;
            }

            if (CanAccessParentScope && ParentScope != null)
            {
                return ParentScope.ReadVariable(name);
            }

            throw new Exception($"Unknown variable {name}");
        }
        
        public void RegisterWrite(string name)
        {
            if (LocalVariables.TryGetValue(name, out var info))
            {
                info.WriteCount++;
                LocalVariables[name] = info;
            }
        }
    }
    
    public class CTCompiler
    {
        public readonly HashSet<Scope> AllScopes = new ();

        public readonly List<Instruction> Instructions = new ();

        public readonly Scope GlobalScope;
        
        private readonly CTRoot m_root;

        public CTCompiler(CTRoot root)
        {
            m_root = root;
            GlobalScope = new Scope(this, null, false);
        }
        
        public CompiledNode CompileNode(Scope scope, CTNode node)
        {
            switch (node)
            {
                case CTExpression expression: 
                    return new CompiledExpression(this, scope, expression);
                
                case CTAssignStatement statement:
                    return new CompiledAssignStatement(this, scope, statement);
                
                case CTDeclareStatement statement: 
                    return new CompiledDeclareStatement(this, scope, statement);
                
                case CTFunction function:
                    var fn = new CompiledFunction(this, scope, function);
                    scope.RegisterNewFunction(fn);
                    return fn;
                
                case CTReturnStatement statement:
                    return new CompiledReturnStatement(this, scope, statement);
                
                case CTRepeatBlockStatement statement:
                    return new CompiledRepeatStatement(this, scope, statement);
                
                default: throw new NotImplementedException($"Instruction {node.GetType().Name} not implemented");
            }
        }
        
        public Instruction[] Compile()
        {
            Instructions.Clear();

            for (var i = 0; i < m_root.Children.Count; i++)
                CompileNode(GlobalScope, m_root.Children[i]);

            CTOptimizer.Optimize(this, Instructions);
            return Instructions.ToArray();
        }
    }
}