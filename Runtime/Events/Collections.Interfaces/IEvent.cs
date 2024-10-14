using System;

namespace ODK.Events.Collections.Interfaces
{
 public interface IEvent
 {
   int InitiatorId { get; set; }
   Nullable<int> TargetId { get; set; }
 } 
}