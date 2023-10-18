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

    public struct StructInfo
    {
        public Scope Scope;
        public int SizeInLongs;
    }

    public class Scope
    {
        public readonly Dictionary<string, StructInfo> Structs = new();
        public readonly Dictionary<string, LocalVariableInfo> LocalVariables = new();
        public readonly Dictionary<string, CompiledFunction> Functions = new();
        
        public readonly bool CanAccessParentScope;
        public readonly Scope ParentScope;
        public readonly CTNode ScopeCreator;

        private int m_stackPtrOffset;
        
        public CompiledFunction GetFunction(string name)
        {
            if (Functions.TryGetValue(name, out var fn))
                return fn;

            if (ParentScope != null)
                return ParentScope.GetFunction(name);

            throw new Exception($"Couldn't find function '{name}' during compilation.");
        }

        public Scope(CTCompiler compiler, CTNode creator, Scope parent, bool canAccessParentScope)
        {
            ScopeCreator = creator;
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

        public int RegisterNewVariable(string name, string typeName, int stackSize, int level)
        {
            int stackPtr = m_stackPtrOffset;
            LocalVariables.Add(name, new LocalVariableInfo
            {
                StackPointer = stackPtr,
                StackSize = stackSize,
                ReadCount = 0,
                WriteCount = 0,
                Level = level,
                TypeName = typeName
            });
            
            m_stackPtrOffset += stackSize;
            return stackPtr;
        }

        public LocalVariableInfo ReadVariable(string name, int level)
        {
            if (LocalVariables.TryGetValue(name, out var info) && info.Level <= level)
            {
                info.ReadCount++;
                LocalVariables[name] = info;
                return info;
            }

            if (CanAccessParentScope && ParentScope != null)
            {
                return ParentScope.ReadVariable(name, level);
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
    
    public struct TempFunctionCall
    {
        public string FunctionName;
        public int ExpectedArgumentCount;
        public Scope Scope;
        
        public TempFunctionCall(string functionName, int expectedArgumentCount, Scope scope)
        {
            FunctionName = functionName;
            ExpectedArgumentCount = expectedArgumentCount;
            Scope = scope;
        }
    }
    
    public class CTCompiler
    {
        public readonly HashSet<Scope> AllScopes = new ();

        public readonly List<Instruction> Instructions = new ();

        public readonly Scope GlobalScope;
        
        private readonly CTRoot m_root;
        
        public readonly Dictionary<int, TempFunctionCall> TemporaryFunctionCalls = new ();

        public CTCompiler(CTRoot root)
        {
            m_root = root;
            GlobalScope = new Scope(this, root, null, false);
        }
        
        public CompiledNode CompileNode(Scope scope, CTNode node, int level)
        {
            switch (node)
            {
                case CTExpression expression: 
                    return new CompiledExpression(this, scope, expression, level);
                
                case CTAssignStatement statement:
                    return new CompiledAssignStatement(this, scope, statement, level);
                
                case CTDeclareStatement statement: 
                    return new CompiledDeclareStatement(this, scope, statement, level);
                
                case CTFunction function:
                    var fn = new CompiledFunction(this, scope, function, level);
                    scope.RegisterNewFunction(fn);
                    return fn;
                
                case CTReturnStatement statement:
                    return new CompiledReturnStatement(this, scope, statement, level);
                
                case CTRepeatBlockStatement statement:
                    return new CompiledRepeatStatement(this, scope, statement, level);
                
                case CTSwapStatement statement:
                    return new CompiledSwapStatement(this, scope, statement, level);
                
                case CTIfStatement statement:
                    return new CompiledIfStatement(this, scope, statement, level);
                
                /*case CTStructDefinition definition:
                    return null; // return new CompiledStructDefinition(this, scope, definition, level);*/
                
                default: throw new NotImplementedException($"Instruction {node.GetType().Name} not implemented");
            }
        }
        
        private void FixTempFunctionCalls()
        {
            foreach (var (key, value) in TemporaryFunctionCalls)
            {
                var fn = value.Scope.GetFunction(value.FunctionName);
                
                if (fn.Function.Arguments.Values.Count != value.ExpectedArgumentCount)
                    throw new Exception($"Function {value.FunctionName} expects {fn.Function.Arguments.Values.Count} arguments, but {value.ExpectedArgumentCount} were provided.");
                
                Instructions[key] = new Instruction((Opcodes)Instructions[key].Opcode, fn.FunctionPtr, value.ExpectedArgumentCount);
            }
            
            TemporaryFunctionCalls.Clear();
        }
        
        public Instruction[] Compile()
        {
            Instructions.Clear();

            for (var i = 0; i < m_root.Children.Count; i++)
                CompileNode(GlobalScope, m_root.Children[i], 0);

            Instructions.Add(new Instruction(Opcodes.STOP));
            FixTempFunctionCalls();

            CTOptimizer.Optimize(this, Instructions);
            return Instructions.ToArray();
        }
    }
}