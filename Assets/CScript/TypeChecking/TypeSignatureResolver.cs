using System.Collections.Generic;
using UnityEngine;

namespace Riten.CScript.TypeChecker
{
    [System.Serializable]
    public struct NamedType
    {
        public string Name;
        public string Type;
    }
    
    [System.Serializable]
    public struct FunctionSignature
    {
        public string Signature;
        public string Name;
        public NamedType[] Arguments;
        public string ReturnType;
        public int CompilePtr;

        public FunctionSignature(string sig, string name, NamedType[] argumentTypes, string returnType)
        {
            Name = name;
            Signature = sig;
            Arguments = argumentTypes;
            ReturnType = returnType;
            CompilePtr = -1;
        }
    }

    public class TypeScope
    {
        public readonly string TypeName;
        public readonly TypeScope Parent;
        public readonly TypeSignatureResolver SignatureResolver;
        
        public TypeScope(TypeSignatureResolver signatureResolver, string typeName, TypeScope parent)
        {
            SignatureResolver = signatureResolver;
            TypeName = typeName;
            Parent = parent;
        }

        private string GetParentName()
        {
            var parentName = Parent?.GetParentName();
            return string.IsNullOrEmpty(parentName) ? TypeName : $"{parentName}.{TypeName}";
        }
        
        public string GetSignatureFromLocalName(string name)
        {
            var parentName = GetParentName();
            return string.IsNullOrEmpty(parentName) ? name : $"{parentName}.{name}";
        }
    }
    
    [System.Serializable]
    public class TypeSignatureResolver
    {
        [SerializeField] List<FunctionSignature> m_functions = new ();
        
        readonly List<NamedType> m_types = new ();
        
        readonly TypeScope m_rootScope;
        
        public TypeSignatureResolver(CTNode rootNode)
        {
            m_rootScope = new TypeScope(this, string.Empty, null);
            
            for (int i = 0; i < rootNode.Children.Count; i++)
            {
                var child = rootNode.Children[i];
                Resolve(child, m_rootScope);
            }
        }

        private void Resolve(CTNode node, TypeScope scope)
        {
            switch (node)
            {
                case CTFunction function:
                {
                    var localTypeName = function.FunctionName.Span.Content;
                    var typeSignature = scope.GetSignatureFromLocalName(localTypeName);
                    RegisterNewFunction(function, typeSignature);
                    ResolveChildren(node, new TypeScope(this, localTypeName, scope));
                    break;
                }
                
                default: ResolveChildren(node, scope); break;
            }
        }

        private void ResolveChildren(CTNode node, TypeScope scope)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                Resolve(child, scope);
            }
        }
        
        public void RegisterNewFunction(CTFunction function, string typeSignature)
        {
            m_types.Clear();

            for (var i = 0; i < function.Arguments.Values.Count; i++)
            {
                var arg = function.Arguments.Values[i];
                
                m_types.Add(new NamedType
                {
                    Name = arg.ArgumentName.Span.Content,
                    Type = arg.ArgumentType.Span.Content
                });
            }

            m_functions.Add(new FunctionSignature(
                typeSignature,
                function.FunctionName.Span.Content,
                m_types.ToArray(),
                function.ReturnType.Span.Content
            ));
        }

        public void CompleteFunctionData(string functionName, int functionFunctionPtr)
        {
            for (int i = 0; i < m_functions.Count; i++)
            {
                var function = m_functions[i];
                if (function.Name == functionName)
                {
                    function.CompilePtr = functionFunctionPtr;
                    m_functions[i] = function;
                    return;
                }
            }
        }

        public bool TryGetFunctionSignature(string name, out FunctionSignature data)
        {
            for (int i = 0; i < m_functions.Count; i++)
            {
                var function = m_functions[i];
                if (function.Name == name)
                {
                    data = function;
                    return true;
                }
            }
            
            data = default;
            return false;
        }
    }
}