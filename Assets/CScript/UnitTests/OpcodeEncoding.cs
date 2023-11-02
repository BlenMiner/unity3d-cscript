using NUnit.Framework;
using System.Collections.Generic;
using Riten.CScript.Native;
using UnityEngine;

public class OpcodeEncoding
{
    [Test]
    public void OpcodeEncodingSimplePasses()
    {
        var encoding = new OpcodeCompressor();
        
        encoding.AddPop(5, NativeSize.INT, NativePtr.SPTR);
        encoding.AddPush(1, NativeSize.INT, NativePtr.CONST);

        var inst = encoding.Encode();
        List<OpcodeMetadata> decodedValues = new ();
        var count = encoding.Decode(inst, decodedValues);
        
        Assert.AreEqual(2, count, "Decoded count");
        Assert.AreEqual(2, decodedValues.Count, "Actual values");
        
        Assert.AreEqual(5, decodedValues[0].Value, "Decoded value 0");
        Assert.AreEqual(1, decodedValues[1].Value, "Decoded value 1");
    }
    
    [Test]
    public void OpcodeEncodingSimplePasses2()
    {
        var encoding = new OpcodeCompressor();
        
        encoding.AddPop(default, NativeSize.INT, NativePtr.NONE);
        encoding.AddPop(default, NativeSize.INT, NativePtr.NONE);
        encoding.AddPop(5, NativeSize.BYTE, NativePtr.PTR);
        encoding.AddPush(1, NativeSize.LONG, NativePtr.SPTR);

        var inst = encoding.Encode();
        List<OpcodeMetadata> decodedValues = new ();
        var count = encoding.Decode(inst, decodedValues);
        
        Assert.AreEqual(4, count, "Decoded count");
    }
    
    [Test]
    public void OpcodeEncodingSimplePasses3()
    {
        var encoding = new OpcodeCompressor();
        
        encoding.AddPush(69420, NativeSize.INT, NativePtr.CONST);
        encoding.AddPop(0, NativeSize.INT, NativePtr.SPTR);

        var result = encoding.Encode();
        var list = new List<OpcodeMetadata>();
        encoding.Decode(result, list);
        
        Assert.AreEqual(9, list[4].Value);
        Assert.AreEqual(6, list[3].Value);
        Assert.AreEqual(42, list[2].Value);
        Assert.AreEqual(69, list[1].Value);

        Debug.Log($"new Instruction({(Opcodes)result.Opcode}, {result.Operand}, {result.Operand2}, {result.Operand3}, {result.Operand4});");
    }
}
