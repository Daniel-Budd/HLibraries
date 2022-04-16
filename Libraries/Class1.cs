using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Libraries{
    public readonly struct NullableConsoleColor
    {
      internal NullableConsoleColor(GeneralColor gc, bool notnull)
      {
        color = gc == GeneralColor.Transparent && !notnull ? null : (ConsoleColor)Math.Max((int)gc - 1, 0);
      }
      public readonly ConsoleColor? color { get; init; }
      public ConsoleColor NotNull()
      {
        return color ?? ConsoleColor.Black;
      }
      public ConsoleColor? ToConsoleColor()
      {
        return color;
      }
      public static implicit operator ConsoleColor?(NullableConsoleColor c) => c.color;
      public static explicit operator ConsoleColor(NullableConsoleColor c) => c.NotNull();
    }
    // public static class Comparison
    // {
    //   // public static int 
    // }
    // public readonly struct NullableComparison<t>
    // {
    //   public readonly Type T1 { get => typeof(t); }
    //   public readonly Func<t,t,int>? output { get; init; }
    //   internal NullableComparison(Func<t,t,int>? f)
    //   {
    //     output = f;
    //   }
    //   public Func<t,t,int> NotNull()
    //   {
    //     return output ?? ((a, b) => a == null || b == null ? 0 : a.Equals(b) ? 0 : 1);
    //   }
    // }
    // public sealed class ComparisonDatabase
    // {
    //   private Dictionary<Type,object> comparisonIndex = new Dictionary<Type,object>();
    //   internal ComparisonDatabase()
    //   {
    //     //
    //   }
    //   internal List<Type> types = new List<Type>();
    //   private NullableComparison<T> Pull<T>()
    //   {
    //     try
    //     {
    //       return new NullableComparison<T>((Func<T,T,int>)comparisonIndex[typeof(T)]);
    //     }
    //     catch
    //     {
    //       Console.WriteLine("NO COMPARER LISTED FOR GIVEN TYPE.");
    //       return new NullableComparison<T>(null);
    //     }
    //   }
    //   private 
    // }
    public static class Methods
    {
      public static ThreadStart ToThreadStart(this Action a) => new ThreadStart(a);
      public static NullableConsoleColor ToConsoleColor(this GeneralColor color, [Optional]bool notnull)
      {
        return new NullableConsoleColor(color, notnull);
      }
      ///<summary>Converts the <see cref="ConsoleColor"/> into a <see cref="GeneralColor"/></summary>
      ///<remarks>
      ///<para>Understand that this excludes <see cref="GeneralColor.Transparent"/></para>
      ///</remarks>
      ///<returns>A <see cref="GeneralColor"/> that **NEVER** will have a transparent value</returns>
      public static GeneralColor ToGeneralColor(this ConsoleColor color)
      {
        return (GeneralColor)Math.Min((int)color + 1, (int)GeneralColor.White);
      }
      public static MetaObject<T> GenerateMetaObject<T>(this T target,params MetaDataEntry[] metas) where T : notnull
      {
        return new MetaObject<T>(){ Value = target, metaData = new MetaData(metas)};
      }
      public static void Run(this IRunnable runnable) => runnable.Run();
      public static void Run<T>(this IRunnable<T> runnable, T args) => runnable.Run(args);
      public static void Run<T1,T2>(this IRunnable<T1,T2> runnable, T1 a, T2 b) => runnable.Run(a,b);
      public static void Run<T1,T2,T3>(this IRunnable<T1,T2,T3> runnable, T1 a, T2 b, T3 c) => runnable.Run(a,b,c);
      public static void Run<T1,T2,T3,T4>(this IRunnable<T1,T2,T3,T4> runnable, T1 a, T2 b, T3 c,T4 d) => runnable.Run(a,b,c,d);
      public static void Run<T1,T2,T3,T4,T5>(this IRunnable<T1,T2,T3,T4,T5> runnable, T1 a, T2 b, T3 c,T4 d,T5 e) => runnable.Run(a,b,c,d,e);
      public static void Run<T1,T2,T3,T4,T5,T6>(this IRunnable<T1,T2,T3,T4,T5,T6> runnable, T1 a, T2 b, T3 c,T4 d,T5 e,T6 f) => runnable.Run(a,b,c,d,e,f);
      public static TOutput[] PullForEach<TInput,TOutput>(this TInput[] objects, Func<TInput,TOutput> processor)
      {
        TOutput[] results = new TOutput[objects.Length];
        for(int i = 0; i < results.Length; i++)
        {
          results[i] = processor(objects[i]);
        }
        return results;
      }
      public static void Replace<Key,Value>(this Dictionary<Key,Value> dictionary, Key key, Value newValue) where Key : notnull
      {
        dictionary.Remove(key);
        dictionary.Add(key,newValue);
      }
      public static ValueTracker<T> GetTracker<T>(this T obj, bool enabled = true)
      {
        return new ValueTracker<T>(obj, enabled);
      }
      public static TrackedValue<T> Track<T>(this T obj)
      {
        return new TrackedValue<T>(obj);
      }
      public static ValueTracker<T> GetTracker<T>(this T obj, EventSyntax syntax, bool enabled = true)
      {
        return new ValueTracker<T>(obj, syntax, enabled);
      }
      public static ValueTracker<T> GetTracker<T>(this T obj, bool RetrievalWatched, bool ModificationWatched, bool enabled = true)
      {
        return new ValueTracker<T>(obj, new(Retrieval: RetrievalWatched, Modification: ModificationWatched), enabled);
      }
      public static void Subscribe<T>(this ITracker<T> tracker, ITracked<T> adder)
      {
        adder.AddHandler(tracker);
      }
      public static void Unsubscribe<T>(this ITracker<T> tracker, ITracked<T> adder)
      {
        adder.RemoveHandler(tracker);
      }
      public static bool Invert(this bool b)
      {
        return !b;
      }
      public static bool Invert(this bool b, Func<bool> condition)
      {
        return condition() ? !b : b;
      }
      public static void Toggle(this bool b)
      {
        b = !b;
      }
      public static void Disable(this bool b)
      {
        b = false;
      }
      public static void Enable(this bool b)
      {
        b = true;
      }
    }
}
///<summary>The number</summary>
// public class Number : IComparable<Number>, IEquatable<Number>
// {
//   private double _dValue;
//   public double dValue { get => GetValue(); set => SetValue(value); }
//   public int iValue { get => (int)Math.Round(GetValue()); set => SetValue((double)value); }
//   public string name { get; init; }
//   public Number(double d, bool tracked = false) {_dValue = d; name = "";isTracked = tracked; SetupHandlers();}
//   public Number(string n, double d, bool tracked = false) {_dValue = d; name= n; isTracked = tracked; SetupHandlers();}
//   public Number(bool tracked = false) {_dValue = 0.0; name = ""; isTracked = tracked;SetupHandlers();}
//   public double GetValue()
//   {
//     return 0.0;
//   }
//   private void SetupHandlers()
//   {
//     ValueChangeEvent += (sender, args) => {};
//     ValueRetrieveEvent += (sender, args) => {};
//   }
//   public bool Equals(Number? n) => n != null ? n._dValue == _dValue : false;
//   public void SetValue(double v)
//   {
//     if(ValueChangeEvent != null) ValueChangeEvent(this,new ValueChangeEventArgs<double>(_dValue,v));
//     _dValue = v;
//   }
//   public int CompareTo(Number? n) => n != null ? _dValue.CompareTo(n._dValue) : 1;
//   public event EventHandler<ValueChangeEventArgs<double>> OnValueChanged
//   {
//     add
//     {
//       ValueChangeEvent += value;
//     }
//     remove
//     {
//       ValueChangeEvent -= value;
//     }
//   }
//   event EventHandler<ValueChangeEventArgs<double>> ValueChangeEvent;
//   private bool isTracked;
// }