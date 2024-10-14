using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODK.Events.Collections.Interfaces;

namespace ODK.Events.Collections
{
  public class Observable<TEvent> : IObservable
  where TEvent : IEvent
  {
    private List<Subscriber<TEvent>> _observers = new();

    /// <inheritdoc/>
    public IObservable Subscribe<TObserver>(TObserver observer)
    {
      if (observer is not Action<TEvent> action)
        throw new Exception(
          $"Observer of type {typeof(TObserver)} cannot be used to subscribe to observable of type {typeof(TEvent)}.");

      Subscriber<TEvent> subscriber = new(action);
      _observers.Add(subscriber);
      return this;
    }

    /// <inheritdoc/>
    public IObservable Subscribe<TObserver>(TObserver observer, int targetId)
    {
      if (observer is not Action<TEvent> action)
        throw new Exception(
          $"Observer of type {typeof(TObserver)} cannot be used to subscribe to observable of type {typeof(TEvent)}.");

      Subscriber<TEvent> subscriber = new(action, targetId);
      _observers.Add(subscriber);
      return this;
    }

    /// <inheritdoc/>
    public IObservable Unsubscribe<TObserver>(TObserver observer)
    {
      if (observer is not Action<TEvent> eventObserver)
        return this;

      _observers = _observers.Where(_ => !_.Action.Equals(eventObserver)).ToList();
      return this;
    }

    /// <inheritdoc/>
    public async Task Invoke(IEvent @event)
    {
      if (@event is not TEvent concreteEvent)
        throw new Exception(
          $"Cannot invoke observables of type {typeof(TEvent)} with event of type {@event.GetType()}.");

      IEnumerable<Task> tasks = _observers
        .Select(observer => Task.Run(() => observer?.Invoke(concreteEvent)));

      await Task.WhenAll(tasks);
    }
  }
}