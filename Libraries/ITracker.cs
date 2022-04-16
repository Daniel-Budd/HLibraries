#nullable disable
using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
///<summary>Subscribes to <see cref="ITracked{T}"/></summary>
public interface ITracker<T>
{
  public void OnValueChanged(object sender, ValueChangeEventArgs<T> args);
  public void OnValueRetrieved(object sender, ValueRetrieveEventArgs<T> args);
}
///<summary>Adds Handlers</summary>
public interface ITracked<T>
{
  ///<summary>Sets the value for the <see cref="ITracked{T}"/></summary>
  ///<param name="nv">The new Value for the <see cref="ITracked{T}"/></param>
  ///<remarks>
  ///<para>dfdfd</para>
  ///</remarks>
  public void SetValue(T nv);
  public T GetValue();
  public void AddHandler(EventHandler<ValueChangeEventArgs<T>> handler);
  public void AddHandler(EventHandler<ValueRetrieveEventArgs<T>> handler);
  public virtual void AddHandler(ITracker<T> tracker)
  {
    if(tracker.OnValueChanged != null) AddHandler(tracker.OnValueChanged);
    if(tracker.OnValueRetrieved != null) AddHandler(tracker.OnValueRetrieved);
  }
  public void RemoveHandler(EventHandler<ValueChangeEventArgs<T>> handler);
  public void RemoveHandler(EventHandler<ValueRetrieveEventArgs<T>> handler);
  public void RemoveHandler(ITracker<T> tracker)
  {
    if(tracker.OnValueChanged != null) RemoveHandler(tracker.OnValueChanged);
    if(tracker.OnValueRetrieved != null) RemoveHandler(tracker.OnValueRetrieved);
  }
}