#nullable enable

using System;
using System.Threading.Tasks;
using ODK.Events.Collections.Interfaces;

namespace ODK.Events.Managers.Interfaces
{
  public interface IEventManager
  {
    /// <summary>
    /// Subscribes to an observable event
    /// </summary>
    /// <param name="observer">
    /// The action to invoke when the event is triggered
    /// </param>
    /// <typeparam name="TEvent">
    /// The type of <see cref="IEvent"/>
    /// </typeparam>
    /// <returns>
    /// The <see cref="IObservable"/>
    /// </returns>
    IObservable Subscribe<TEvent>(Action<TEvent> observer)
    where TEvent : IEvent;

    /// <summary>
    /// Subscribes to an observable event
    /// </summary>
    /// <param name="observer">
    /// The action to invoke when the event is triggered
    /// </param>
    /// <param name="targetId">
    /// The targetId of the <see cref="IEvent"/>
    /// </param>
    /// <typeparam name="TEvent">
    /// The type of <see cref="IEvent"/>
    /// </typeparam>
    /// <returns>
    /// The <see cref="IObservable"/>
    /// </returns>
    IObservable Subscribe<TEvent>(Action<TEvent> observer, int targetId)
    where TEvent : IEvent;

    /// <summary>
    /// Unsubscribes from an observable event
    /// </summary>
    /// <param name="observer">
    /// The action that should be unsubscribed from future invocations
    /// </param>
    /// <typeparam name="TEvent">
    /// The type of <see cref="IEvent"/>
    /// </typeparam>
    /// <returns>
    /// The <see cref="IObservable"/>
    /// </returns>
    IObservable? Unsubscribe<TEvent>(Action<TEvent> observer)
    where TEvent : IEvent;

    /// <summary>
    /// Invokes all observers of the <see cref="IEvent"/>
    /// </summary>
    /// <param name="event">
    /// The <see cref="IEvent"/>
    /// </param>
    /// <typeparam name="TEvent">
    /// The type of <see cref="IEvent"/>
    /// </typeparam>
    /// <returns>
    /// The completed <see cref="Task"/>
    /// </returns>
    Task Invoke<TEvent>(TEvent @event)
    where TEvent : IEvent;
  }
}