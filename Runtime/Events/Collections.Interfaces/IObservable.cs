using System.Threading.Tasks;

namespace ODK.Events.Collections.Interfaces
{
  public interface IObservable
  {
    /// <summary>
    /// Subscribes to an observable event
    /// </summary>
    /// <param name="observer">
    /// The action to invoke when the event is triggered
    /// </param>
    /// <typeparam name="TObserver">
    /// The type of the <paramref name="observer"/>
    /// </typeparam>
    /// <returns>
    /// The <see cref="IObservable"/>
    /// </returns>
    IObservable Subscribe<TObserver>(TObserver observer);

    /// <summary>
    /// Subscribes to an observable event
    /// </summary>
    /// <param name="observer">
    /// The action to invoke when the event is triggered
    /// </param>
    /// <param name="targetId">
    /// The targetId of the <see cref="IEvent"/>
    /// </param>
    /// <typeparam name="TObserver">
    /// The type of the <paramref name="observer"/>
    /// </typeparam>
    /// <returns>
    /// The <see cref="IObservable"/>
    /// </returns>   
    IObservable Subscribe<TObserver>(TObserver observer, int targetId);

    /// <summary>
    /// Unsubscribes an observer from event invocations
    /// </summary>
    /// <param name="observer">
    /// The action that should be unsubscribed from future invocations
    /// </param>
    /// <typeparam name="TObserver">
    /// The type of the <paramref name="observer"/>
    /// </typeparam>
    /// <returns>
    /// The <see cref="IObservable"/>
    /// </returns>
    IObservable Unsubscribe<TObserver>(TObserver observer);

    /// <summary>
    /// Invokes all observers of the <see cref="IEvent"/>
    /// </summary>
    /// <param name="event">
    /// The <see cref="IEvent"/>
    /// </param>
    /// <returns>
    /// The completed <see cref="Task"/>
    /// </returns>
    Task Invoke(IEvent @event);
  }
}