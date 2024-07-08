#nullable enable

using System;
using System.Collections;
using ODK.Interaction.Controllers.Interfaces;
using UnityEngine;

namespace ODK.Extensions
{
  public static class MonoBehaviourExtensions
  {
    public static void ConnectToDeviceInput(
      this MonoBehaviour monoBehaviour,
      Func<IDeviceInputController?> deviceInputController,
      Action<IDeviceInterfaceInput> listener)
    {
      monoBehaviour.StartCoroutine(monoBehaviour.TryConenctToDeviceInput(deviceInputController, listener));
    }

    public static IEnumerator TryConenctToDeviceInput(
      this MonoBehaviour monoBehaviour,
      Func<IDeviceInputController?> deviceInputController,
      Action<IDeviceInterfaceInput> listener)
    {
      while (deviceInputController.Invoke() is null)
        yield return null;

      deviceInputController.Invoke()?.ConnectToInterfaceInputEventStack(listener);
    }
  }
}