using System.Collections.Generic;

public abstract class CTStatement : CTNode { }

public abstract class CTNode
{
    public readonly List<CTNode> Children = new();
    
    public void AddChild(CTNode node)
    {
        Children.Add(node);
    }
}

public class CTTypedNode : CTNode
{
    public const string UNSET_TYPE = "?";
    public string TypeName = "?";
}

public class CTDefinition : CTNode
{
    public string Signature { get; private set; }
    
    public void SetSignature(string signature)
    {
        Signature = signature;
    }
}