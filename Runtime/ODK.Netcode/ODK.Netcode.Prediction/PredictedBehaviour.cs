#nullable enable

using System.Collections.Generic;
using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;

namespace ODK.Netcode.Prediction
{
  public abstract class PredictedBehaviour<TInput, TState> : NetworkBehaviour
    where TInput : unmanaged, IPredictedInput<TInput>
    where TState : unmanaged, IPredictedState<TState>
  {
    private readonly Queue<TInput> _serverInputQueue = new();
    private readonly Queue<TState> _serverStateQueue = new();

    private void FixedUpdate()
    {
      if (IsServer)
        ServerTick();
      if (IsOwner)
        OwnerTick();
    }

    private void ServerTick()
    {
      TState predictedState = new();
      while (_serverInputQueue.Count > 0)
      {
        TInput clientInput = _serverInputQueue.Dequeue();
        if (clientInput.ShouldReconcile(clientInput))
          break;

        TState clientState = _serverStateQueue.Dequeue();
        predictedState = Simulate(clientInput);
        if (predictedState.ShouldReconcile(clientState, predictedState))
          break;

        if (_serverInputQueue.Count == 0)
          return;
      }
      
      _serverInputQueue.Clear();
      _serverStateQueue.Clear();
      SyncStateClientRpc(predictedState);
    }

    private void OwnerTick()
    {
      TInput input = ReadInput();
      TState state = Simulate(input);

      SyncStateServerRpc(input, state);
    }

    [ServerRpc]
    private void SyncStateServerRpc(TInput input, TState state)
    {
      _serverInputQueue.Enqueue(input);
      _serverStateQueue.Enqueue(state);
    }

    [ClientRpc]
    private void SyncStateClientRpc(TState state)
    {
      Reconcile(state);
    }

    protected abstract TInput ReadInput();
    protected abstract TState Simulate(TInput input);
    protected abstract void Reconcile(TState reconcileState);
  }
}