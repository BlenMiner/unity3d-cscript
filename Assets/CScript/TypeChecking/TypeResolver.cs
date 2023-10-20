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
        
        public TypeScope(string typeName, TypeScope parent)
        {
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
    public class TypeResolver
    {
        [SerializeField] List<FunctionSignature> m_functions = new ();
        
        List<NamedType> m_types = new ();
        
        TypeScope m_rootScope = new (string.Empty, null);
        
        public TypeResolver(CTNode rootNode)
        {
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
                    ResolveChildren(node, new TypeScope(localTypeName, scope));
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
    }
}