/**
 * Autogenerated by Thrift Compiler (0.7.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;

namespace Bermuda.Core.Thrift
{

  [Serializable]
  public partial class ThriftDatapoint : TBase
  {
    private long _EntityId;
    private string _Text;
    private double _Value;
    private long _Count;
    private long _Timestamp;

    public long EntityId
    {
      get
      {
        return _EntityId;
      }
      set
      {
        __isset.EntityId = true;
        this._EntityId = value;
      }
    }

    public string Text
    {
      get
      {
        return _Text;
      }
      set
      {
        __isset.Text = true;
        this._Text = value;
      }
    }

    public double Value
    {
      get
      {
        return _Value;
      }
      set
      {
        __isset.Value = true;
        this._Value = value;
      }
    }

    public long Count
    {
      get
      {
        return _Count;
      }
      set
      {
        __isset.Count = true;
        this._Count = value;
      }
    }

    public long Timestamp
    {
      get
      {
        return _Timestamp;
      }
      set
      {
        __isset.Timestamp = true;
        this._Timestamp = value;
      }
    }


    public Isset __isset;
    [Serializable]
    public struct Isset {
      public bool EntityId;
      public bool Text;
      public bool Value;
      public bool Count;
      public bool Timestamp;
    }

    public ThriftDatapoint() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I64) {
              EntityId = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Text = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Double) {
              Value = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              Count = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              Timestamp = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("ThriftDatapoint");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.EntityId) {
        field.Name = "EntityId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EntityId);
        oprot.WriteFieldEnd();
      }
      if (Text != null && __isset.Text) {
        field.Name = "Text";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Text);
        oprot.WriteFieldEnd();
      }
      if (__isset.Value) {
        field.Name = "Value";
        field.Type = TType.Double;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Value);
        oprot.WriteFieldEnd();
      }
      if (__isset.Count) {
        field.Name = "Count";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Count);
        oprot.WriteFieldEnd();
      }
      if (__isset.Timestamp) {
        field.Name = "Timestamp";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Timestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ThriftDatapoint(");
      sb.Append("EntityId: ");
      sb.Append(EntityId);
      sb.Append(",Text: ");
      sb.Append(Text);
      sb.Append(",Value: ");
      sb.Append(Value);
      sb.Append(",Count: ");
      sb.Append(Count);
      sb.Append(",Timestamp: ");
      sb.Append(Timestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}