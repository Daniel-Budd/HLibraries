using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
public class TrackedValue<T> : ITracked<T>, ITracker<T>
{
  private ValueTracker<T> Tracker;
  public TrackedValue(T o)
  {
    Tracker = o.GetTracker();
  }
  public TrackedValue(T o, EventSyntax syntax)
  {
    Tracker = o.GetTracker(syntax);
  }
  public void SetValue(T @value)
  {
    Tracker.SetValue(@value);
  }
  public T Value
  { get => GetValue(); set => SetValue(value);}
  public T GetValue() => Tracker.GetValue();
  public void OnValueChanged(object sender, ValueChangeEventArgs<T> args)
  {
    changeMethod(sender,args);
  }
  public void OnValueRetrieved(object sender, ValueRetrieveEventArgs<T> args)
  {
    retrieveMethod(sender,args);
  }
  public EventHandler<ValueChangeEventArgs<T>> changeMethod = (sender, args) => {};
  public EventHandler<ValueRetrieveEventArgs<T>> retrieveMethod = (sender, args) => {};
  public void AddHandler(EventHandler<ValueChangeEventArgs<T>> handler) {Tracker.AddHandler(handler);}
  public void AddHandler(EventHandler<ValueRetrieveEventArgs<T>> handler) {Tracker.AddHandler(handler);}
  public void RemoveHandler(EventHandler<ValueChangeEventArgs<T>> handler) {Tracker.RemoveHandler(handler);}
  public void RemoveHandler(EventHandler<ValueRetrieveEventArgs<T>> handler) {Tracker.RemoveHandler(handler);}
}