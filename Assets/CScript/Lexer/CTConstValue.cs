using Riten.CScript.Native;

namespace Riten.CScript.Lexer
{
    public class CTConstValue : CTTypedNode
    {
        public readonly CToken Token;
        public readonly string ValueString;

        public CTConstValue(CToken token) : base(CTNodeType.Value)
        {
            ValueString = token.Span.Content;
            
            TypeName = GetTypeFromConstant(ref ValueString);
            Token = token;
        }

        string GetTypeFromConstant(ref string constant)
        {
            for (int i = 0; i < InternalTypeUtils.INTERNAL_TYPES.Length; ++i)
            {
                if (constant.EndsWith(InternalTypeUtils.INTERNAL_TYPES[i]))
                {
                    constant = constant[..^InternalTypeUtils.INTERNAL_TYPES[i].Length];
                    return InternalTypeUtils.INTERNAL_TYPES[i];
                }
            }
            
            return constant.Contains('.') ? "f64" : "i64";
        }
    }

    public class CTNodeEmpty : CTTypedNode
    {
        public CTNodeEmpty() : base(CTNodeType.NULL)
        {
        }
    }

    public class CTOperator : CTTypedNode
    {
        public readonly CToken Operator;
        public readonly CTTypedNode Left;
        public readonly CTTypedNode Right;

        public CTOperator(CToken op, CTTypedNode left, CTTypedNode right) : base(CTNodeType.Operator)
        {
            Operator = op;
            Left = left;
            Right = right;
        }
    }

    public class CTTypedNode : CTNode
    {
        public string TypeName;

        protected CTTypedNode(CTNodeType type) : base(type)
        {
        }
    }
}