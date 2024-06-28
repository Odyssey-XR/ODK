namespace ODK.Shared.Input
{
  using System;
  using ODK.Shared.Player;
  using UnityEngine;

  public class InputCommand
  {
    private readonly Action<IInputEvent> _action;

    public InputCommand(Action<IInputEvent> action)
    {
      _action = action;
    }

    public void Invoke(IInputEvent input)
    {
      _action.Invoke(input);
    }

    public override bool Equals(object obj)
    {
      if (obj is InputCommand other)
        return _action.Method.Equals(other._action.Method) && _action.Target == other._action.Target;
      return false;
    }

    public override int GetHashCode()
    {
      return _action.Method.GetHashCode() ^ (_action.Target?.GetHashCode() ?? 0);
    }
  }
}