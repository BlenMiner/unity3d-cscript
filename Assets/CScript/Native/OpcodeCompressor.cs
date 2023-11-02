using System;
using System.Collections.Generic;
using UnityEngine;

namespace Riten.CScript.Native
{
    public enum NativeSize : int
    {
        BYTE,
        SHORT,
        INT,
        LONG
    }
    
    public enum NativePtr
    {
        CONST,
        SPTR,
        PTR,
        NONE
    }
    
    public struct OpcodeMetadata
    {
        public bool IsPush;
        public NativeSize Size;
        public NativePtr Type;
        public long Value;
    }
    
    public class OpcodeCompressor
    {
        readonly List<OpcodeMetadata> m_metadata = new ();
        
        private int m_predictedSize = 0;
        
        public void Clear()
        {
            m_metadata.Clear();
            m_predictedSize = 0;
        }
        
        public bool AddPush(long value, NativeSize size, NativePtr ptrType)
        {
            m_metadata.Add(new OpcodeMetadata
            {
                IsPush = true,
                Size = size,
                Type = ptrType,
                Value = value
            });
            
            m_predictedSize += 1 << (int)size;
            
            return m_predictedSize <= 8 * 3;
        }
        
        public bool AddPop(long value, NativeSize size, NativePtr ptrType)
        {
            m_metadata.Add(new OpcodeMetadata
            {
                IsPush = false,
                Size = size,
                Type = ptrType,
                Value = value
            });

            switch (ptrType)
            {
                case NativePtr.PTR:
                    m_predictedSize += sizeof(long);
                    break;
                case NativePtr.SPTR:
                    m_predictedSize += sizeof(int);
                    break;
                
                case NativePtr.CONST:
                    throw new NotImplementedException();
            }

            return m_predictedSize <= 8 * 3;
        }

        public Instruction Encode()
        {
            Instruction result = default;

            result.Opcode = (int)Opcodes.BATCHED_STACK_OP;
            
            List<byte> bytes = new ();

            long metadataPacked = 0;

            if (m_metadata.Count > 12)
                throw new Exception($"Can't encode more than 12 opcodes. Got {m_metadata.Count}.");

            for (var i = m_metadata.Count - 1; i >= 0; i--)
            {
                var metadata = m_metadata[i];

                long data = (metadata.IsPush ? 0 : 1) << 2;
                data |= (long)metadata.Size;
                data <<= 2;
                data |= (long)metadata.Type;
                
                metadataPacked <<= 5;
                metadataPacked |= data;
            }
            
            metadataPacked <<= 4;
            metadataPacked |= (uint)m_metadata.Count & 0b1111;

            result.Operand = metadataPacked;

            for (var i = 0; i < m_metadata.Count; i++)
            {
                switch (m_metadata[i].Type)
                {
                    case NativePtr.NONE:
                        continue;
                    case NativePtr.SPTR:
                        bytes.AddRange(BitConverter.GetBytes((short)m_metadata[i].Value));
                        break;
                    default:
                        AddValueToList(m_metadata[i], bytes);
                        break;
                }
            }

            float usedLongsFloat = bytes.Count / 8f;
            
            if (usedLongsFloat > 3)
                throw new Exception($"Needs {usedLongsFloat} longs to encode {m_metadata.Count} opcodes. This is too much.");

            for (int i = bytes.Count; i < 3 * 8; ++i)
                bytes.Add(0);

            var bytesArray = bytes.ToArray();
            result.Operand2 = BitConverter.ToInt64(bytesArray, 0);
            result.Operand3 = BitConverter.ToInt64(bytesArray, 8);
            result.Operand4 = BitConverter.ToInt64(bytesArray, 16);

            return result;
        }

        public int Decode(Instruction instruction, List<OpcodeMetadata> values)
        {
            var metadataPacked = instruction.Operand;
            int count = (int)(metadataPacked & 0b1111);
            
            metadataPacked >>= 4;
            
            for (var i = 0; i < count; i++)
            {
                OpcodeMetadata metadata = default;
                
                metadata.Type = (NativePtr)(metadataPacked & 0b11);
                metadataPacked >>= 2;
                metadata.Size = (NativeSize)(metadataPacked & 0b11);
                metadataPacked >>= 2;
                metadata.IsPush = (metadataPacked & 0b1) == 0;
                metadataPacked >>= 1;
                
                values.Add(metadata);
            }
            
            List<byte> rawData = new ();

            rawData.AddRange(BitConverter.GetBytes(instruction.Operand2));
            rawData.AddRange(BitConverter.GetBytes(instruction.Operand3));
            rawData.AddRange(BitConverter.GetBytes(instruction.Operand4));

            var rawDataBytes = rawData.ToArray();
            
            int dataPtr = 0;
            
            for (var i = 0; i < count; i++)
            {
                var metadata = values[i];
                
                if (metadata.Type == NativePtr.NONE) continue;
                
                if (metadata.Type == NativePtr.SPTR)
                {
                    metadata.Value = BitConverter.ToInt16(rawDataBytes, dataPtr);
                    dataPtr += 2;
                    continue;
                }
                
                switch (metadata.Size)
                {
                    case NativeSize.BYTE:
                        metadata.Value = rawDataBytes[dataPtr];
                        dataPtr += 1;
                        break;
                    case NativeSize.SHORT:
                        metadata.Value = BitConverter.ToInt16(rawDataBytes, dataPtr);
                        dataPtr += 2;
                        break;
                    case NativeSize.INT:
                        metadata.Value = BitConverter.ToInt32(rawDataBytes, dataPtr);
                        dataPtr += 4;
                        break;
                    case NativeSize.LONG:
                        metadata.Value = BitConverter.ToInt64(rawDataBytes, dataPtr);
                        dataPtr += 8;
                        break;
                }

                values[i] = metadata;
            }

            return count;
        }

        private static void AddValueToList(OpcodeMetadata metadata, List<byte> bytes)
        {
            switch (metadata.Size)
            {
                case NativeSize.BYTE:
                    bytes.Add((byte)metadata.Value);
                    break;
                case NativeSize.SHORT:
                    bytes.AddRange(BitConverter.GetBytes((short)metadata.Value));
                    break;
                case NativeSize.INT:
                    bytes.AddRange(BitConverter.GetBytes((int)metadata.Value));
                    break;
                case NativeSize.LONG:
                    bytes.AddRange(BitConverter.GetBytes(metadata.Value));
                    break;
            }
        }
    }
}