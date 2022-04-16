using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
public class ValueChangeEventArgs<T> : EventArgs
{
  public T oldValue { get; set; }
  public T newValue { get; set; }
  public ValueChangeEventArgs(T ov, T nv)
  {
    oldValue = ov;
    newValue = nv;
  }
}