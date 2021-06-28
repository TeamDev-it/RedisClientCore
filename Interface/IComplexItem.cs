using System;
using System.Collections.Generic;

using System.Text;

namespace TeamDev.Redis.Interface
{
  public interface IComplexItem
  {
    string KeyName { get; }
    RedisClient Provider { get; }
  }
}
