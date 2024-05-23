namespace ODK.Shared.XR
{
  using System;

  /// <summary>
  /// Contract for listening to XR input events
  /// </summary>
  public interface IXRInputEventer
  {
    /// <summary>
    /// Connects a listener to some event
    /// </summary>
    /// <param name="listener">
    /// The listener <see cref="Action"/>;
    /// </param>
    public void Connect(Action<IXRInput> listener);

    /// <summary>
    /// Disconnects a listener from some event
    /// </summary>
    /// <param name="listener">
    /// The listener <see cref="Action"/>;
    /// </param>
    public void Disconnect(Action<IXRInput> listener);
  }
}