#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ODK.Events.Collections;
using ODK.Events.Collections.Interfaces;
using ODK.Events.Managers.Interfaces;

namespace ODK.Events.Managers
{
  public class EventManager : IEventManager
  {
    private Dictionary<Type, IObservable> _observers = new();

    /// <inheritdoc/>
    public IObservable Subscribe<TEvent>(Action<TEvent> observer)
    where TEvent : IEvent
    {
      if (!_observers.TryGetValue(typeof(TEvent), out IObservable? observable))
        observable = new Observable<TEvent>();
      _observers[typeof(TEvent)] = observable;

      return observable.Subscribe(observer);
    }

    /// <inheritdoc/>   
    public IObservable Subscribe<TEvent>(Action<TEvent> observer, int targetId)
    where TEvent : IEvent
    {
      IObservable observable = _observers[typeof(TEvent)] ?? new Observable<TEvent>();
      _observers[typeof(TEvent)] = observable;

      return observable.Subscribe(observer, targetId);
    }

    /// <inheritdoc/>
    public IObservable? Unsubscribe<TEvent>(Action<TEvent> observer)
    where TEvent : IEvent
    {
      IObservable? observable = _observers[typeof(TEvent)];
      return observable?.Unsubscribe(observer);
    }

    /// <inheritdoc/>
    public async Task Invoke<TEvent>(TEvent @event)
    where TEvent : IEvent
    {
      IObservable? observable = _observers[typeof(TEvent)];
      if (observable is not null)
        await observable.Invoke(@event);
    }
  }
}