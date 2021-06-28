using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TeamDev.Redis
{
  public static class RedisExtensions
  {

    public static IDictionary<string, string> ToRedisDictionary<T>(this T item) where T : class
    {
      Dictionary<string, string> dix = new Dictionary<string, string>();
      PropertyInfo[] infos = typeof(T).GetProperties();

      foreach (PropertyInfo info in infos)
      {
        var value = info.GetValue(item, null);
        if (value != null)
          switch (Type.GetTypeCode(info.PropertyType))
          {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64: dix.Add(info.Name, value.ToString()); break;
            case TypeCode.Decimal: dix.Add(info.Name, ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)); break;
            case TypeCode.Double: dix.Add(info.Name, ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)); break;
            case TypeCode.Single: dix.Add(info.Name, ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)); break;
            case TypeCode.DateTime: dix.Add(info.Name, ((DateTime)value).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")); break;
            case TypeCode.String: dix.Add(info.Name, value.ToString()); break;
          }
        else
          dix.Add(info.Name, null);
      }
      return dix;
    }

    public static T FromRedisDictionary<T>(this System.Collections.Generic.KeyValuePair<string, string>[] items) where T : class, new()
    {
      var result = new T();
      var infos = typeof(T).GetProperties().ToDictionary(i => i.Name);
      foreach (var item in items)
      {
        if (infos.TryGetValue(item.Key, out PropertyInfo info))
        {
          switch (Type.GetTypeCode(info.PropertyType))
          {
            case TypeCode.Byte: { if (Byte.TryParse(item.Value, out Byte v)) info.SetValue(result, v); } break;
            case TypeCode.SByte: { if (SByte.TryParse(item.Value, out SByte v)) info.SetValue(result, v); } break;
            case TypeCode.UInt16: { if (UInt16.TryParse(item.Value, out UInt16 v)) info.SetValue(result, v); } break;
            case TypeCode.UInt32: { if (UInt32.TryParse(item.Value, out UInt32 v)) info.SetValue(result, v); } break;
            case TypeCode.UInt64: { if (UInt64.TryParse(item.Value, out UInt64 v)) info.SetValue(result, v); } break;
            case TypeCode.Int16: { if (Int16.TryParse(item.Value, out Int16 v)) info.SetValue(result, v); } break;
            case TypeCode.Int32: { if (Int32.TryParse(item.Value, out Int32 v)) info.SetValue(result, v); } break;
            case TypeCode.Int64: { if (Int64.TryParse(item.Value, out Int64 v)) info.SetValue(result, v); } break;
            case TypeCode.Decimal: { if (Decimal.TryParse(item.Value, out Decimal v)) info.SetValue(result, v); } break;
            case TypeCode.Double: { if (Double.TryParse(item.Value, out Double v)) info.SetValue(result, v); } break;
            case TypeCode.Single: { if (Single.TryParse(item.Value, out Single v)) info.SetValue(result, v); } break;
            case TypeCode.DateTime: { if (DateTime.TryParse(item.Value, out DateTime v)) info.SetValue(result, v); } break;
            case TypeCode.String: info.SetValue(result, item.Value); break;
          }
        }
      }
      return result;
    }
  }
}