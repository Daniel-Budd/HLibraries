using System;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace Libraries;
///<summary>The syntax for The <see cref="ValueTracker{T}"/> elements</summary>
///<param name="Retrieval">Determines whether or not the retrieval of the value triggers a <see cref="ValueRetrieveEventArgs{T}"/></param>
///<param name="Modification">Determines whether or not the modification of the value triggers a <see cref="ValueChangeEventArgs{T}"/></param>
public record struct EventSyntax(bool Retrieval, bool Modification)
{
///<value name="Retrieval">Determines whether or not the retrieval of the value triggers a <see cref="ValueRetrieveEventArgs{T}"/></value>
  public bool Retrieval { get; set; } = Retrieval;
  public bool Modification { get; set; } = Modification;
  public void SetRetrievalStatus(bool b) => Retrieval = b;
  public static implicit operator EventSyntax((bool retrieval, bool modification) tuple) => new EventSyntax(tuple.retrieval, tuple.modification);
}
///<summary>Tracks the value of a value that it holds. can be handled</summary>
///
///<typeparam name="T">The value type that is tracked</typeparam>
public class ValueTracker<T> : ITracked<T>
{
  private T storedValue;
  ///<value>The <see cref="EventSyntax"/> for the <see cref="ValueTracker{T}"/>.</value>
  public EventSyntax TrackingSyntax { get; protected set; }
  ///<value>This determines if the tracker raises any events.</value>
  public bool enabled { get; private set; }
  ///<summary>Tracks the value of a certain object</summary>
  ///<typeparam name="T">The type of object that is tracked.</typeparam>
  ///<param name="obj">The object to track</param>
  ///<param name="b">Determines whether or not the events are initially enabled.</param>
  public ValueTracker(T obj,bool b = true)
  {
    TrackingSyntax = new(Retrieval: true, Modification: true);
    enabled = b;
    storedValue = obj;
    OnValueChanged += (sender, args) => {};
    OnValueRetrieved += (sender, args) => {};
  }
  ///<summary>Tracks the value of a certain object.</summary>
  ///<param name="obj">the object to track</param>
  ///<param name="syntax">the <see cref="EventSyntax"/> that determines the behavior of this tracker</param>
  ///<param name="b"><see cref="enabled"/> enables or disables the behaviors</param>
  public ValueTracker(T obj,EventSyntax syntax, bool b = true)
  {
    TrackingSyntax = syntax;
    enabled = b;
    storedValue = obj;
    OnValueChanged += (sender, args) => {};
    OnValueRetrieved += (sender, args) => {};
  }
  ///<summary>Changes the behavior of the tracker to the selected syntax</summary>
  ///<param name="syntax">The <see cref="EventSyntax"/> that changes the behavior of the tracker</param>
  public void SetTrackerSyntax(EventSyntax syntax)
  {
    TrackingSyntax = syntax;
  }
  ///<summary></summary>
  ///<param name="r">The Retrieval profile of the EventSyntax</param>
  ///<param name="m">The Modification profile of the EventSyntax</param>
  public void SetTrackerSyntax(bool r, bool m)
  {
    TrackingSyntax = new(Retrieval: r, Modification: m);
  }
  public T Value
  { get => GetValue(); set => SetValue(value);}
  ///<summary>Gets the value and handles the retrieval</summary>
  ///<returns>The value of the tracked object</returns>
  public T GetValue()
  {
    HandleRetrieval();
    return storedValue;
  }
  protected void HandleRetrieval()
  {
    EventHandler<ValueRetrieveEventArgs<T>> raiseEvent = OnValueRetrieved;
    if(raiseEvent != null && TrackingSyntax.Retrieval && enabled) { raiseEvent(this,new ValueRetrieveEventArgs<T>(storedValue)); }
  }
  public void SetValue(T nv)
  {
    ProcessValue(nv);
    storedValue = nv;
  }
  private void ProcessValue(T nv)
  {
    if(nv == null) return;
    EventHandler<ValueChangeEventArgs<T>> raiseEvent = OnValueChanged;
    if(!nv.Equals(storedValue) && raiseEvent != null && enabled && TrackingSyntax.Modification) { raiseEvent(this,new ValueChangeEventArgs<T>(storedValue,nv));}
  }
  public event EventHandler<ValueChangeEventArgs<T>> OnValueChanged;
  public event EventHandler<ValueRetrieveEventArgs<T>> OnValueRetrieved;
  public void AddHandler(EventHandler<ValueChangeEventArgs<T>> handler) {OnValueChanged += handler;}
  public void AddHandler(EventHandler<ValueRetrieveEventArgs<T>> handler) {OnValueRetrieved += handler;}
  public void RemoveHandler(EventHandler<ValueChangeEventArgs<T>> handler) {OnValueChanged -= handler;}
  public void RemoveHandler(EventHandler<ValueRetrieveEventArgs<T>> handler) {OnValueRetrieved -= handler;}
}