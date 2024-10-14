#nullable enable

using System;
using ODK.Events.Collections.Interfaces;

namespace ODK.Events.Collections
{
  public class Subscriber<TEvent>
  where TEvent : IEvent 
  {
    public Nullable<int> TargetId;
    public Action<TEvent> Action;

    public Subscriber(Action<TEvent> action, Nullable<int> targetId = null)
    {
      Action = action;
    }

    public void Invoke(TEvent @event)
    {
      if (TargetId.HasValue && @event.TargetId != TargetId.Value)
        return;
      
      Action.Invoke(@event);
    }
  }
}