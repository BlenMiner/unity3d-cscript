using Riten.CScript.Native;

namespace Riten.CScript.Lexer
{
    public class CTConstValue : CTTypedNode
    {
        public readonly CToken Token;
        public readonly string ValueString;

        public CTConstValue(CToken token)
        {
            ValueString = token.Span.Content;
            TypeName = GetTypeFromConstant(ref ValueString);
            Token = token;
        }

        static string GetTypeFromConstant(ref string constant)
        {
            for (int i = 0; i < InternalTypeUtils.INTERNAL_TYPES.Length; ++i)
            {
                if (constant.EndsWith(InternalTypeUtils.INTERNAL_TYPES[i]))
                {
                    constant = constant[..^InternalTypeUtils.INTERNAL_TYPES[i].Length];
                    
                    var type = InternalTypeUtils.INTERNAL_TYPES[i];
                    
                    if (type == "f")
                        type = "f32";
                    
                    return type;
                }
            }

            return UNSET_TYPE;
        }
    }

    public class CTNodeEmpty : CTTypedNode {}

    public class CTOperator : CTTypedNode
    {
        public readonly CToken Operator;
        public readonly CTTypedNode Left;
        public readonly CTTypedNode Right;

        public CTOperator(CToken op, CTTypedNode left, CTTypedNode right)
        {
            Operator = op;
            Left = left;
            Right = right;
            
            AddChild(left);
            AddChild(right);
        }
    }
}