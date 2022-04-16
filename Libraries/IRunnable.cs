using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
public interface IRunnable
{
  public void Run();
}
public interface IRunnable<in T>
{
  public void Run(T args);
}
public interface IRunnable<in T1,in T2>
{
  public void Run(T1 a, T2 b);
}
public interface IRunnable<in T1,in T2,in T3>
{
  public void Run(T1 a, T2 b, T3 c);
}
public interface IRunnable<in T1,in T2,in T3,in T4>
{
  public void Run(T1 a, T2 b, T3 c, T4 d);
}
public interface IRunnable<in T1,in T2,in T3,in T4,in T5>
{
  public void Run(T1 a,T2 b,T3 c,T4 d,T5 e);
}
public interface IRunnable<in T1,in T2,in T3,in T4,
                          in T5,in T6>
{
  public void Run(T1 a,T2 b,T3 c,T4 d,T5 e,T6 f);
}