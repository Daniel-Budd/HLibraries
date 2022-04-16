using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
public class ValueRetrieveEventArgs<T> : EventArgs
{
  public T retrievedValue { get; init; }
  public ValueRetrieveEventArgs(T v) { retrievedValue = v; }
}