namespace ODK.Shared.Player
{
  using System;
  using ODK.Shared.Input;

  /// <summary>
  /// Contract for listening to XR input events
  /// </summary>
  public interface IInputEventer
  {
    /// <summary>
    /// Connects a listener to some event
    /// </summary>
    /// <param name="listener">
    /// The listener <see cref="Action"/>;
    /// </param>
    public void Connect_Server(InputCommand listener);

    /// <summary>
    /// Disconnects a listener from some event
    /// </summary>
    /// <param name="listener">
    /// The listener <see cref="Action"/>;
    /// </param>
    public void Disconnect_Server(InputCommand listener);
  }
}