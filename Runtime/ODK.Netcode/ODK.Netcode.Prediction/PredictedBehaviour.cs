#nullable enable

using System.Collections.Generic;
using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Netcode.Prediction
{
  public abstract class PredictedBehaviour<TInput, TState> : NetworkBehaviour
  where TInput : unmanaged, IPredictedInput<TInput>
  where TState : unmanaged, IPredictedState<TState>
  {
    private readonly Queue<TInput> _serverInputQueue = new();
    private readonly Queue<TState> _serverStateQueue = new();
    private readonly Queue<TInput> _serverFixedInputQueue = new();
    private readonly Queue<TState> _serverFixedStateQueue = new();

    private void Update()
    {
      if (IsServer)
        ServerTick();
      if (IsOwner)
        OwnerTick();
    }

    private void FixedUpdate()
    {
      if (IsServer)
        FixedServerTick();
      if (IsOwner)
        FixedOwnerTick();
    }

    [ServerRpc]
    private void SyncStateServerRpc(TInput input, TState state, bool isFixed)
    {
      if (isFixed)
      {
        _serverFixedInputQueue.Enqueue(input);
        _serverFixedStateQueue.Enqueue(state);
      }
      else
      {
        _serverInputQueue.Enqueue(input);
        _serverStateQueue.Enqueue(state);
      }
    }

    [ClientRpc]
    private void SyncStateClientRpc(TState state)
    {
      Reconcile(state);
    }

    private void CheckServerState(Queue<TInput> inputQueue, Queue<TState> stateQueue, bool isFixed)
    {
      TState predictedState = new();
      bool   reconcile      = false;
      while (inputQueue.Count > 0)
      {
        reconcile = true;

        TInput clientInput = inputQueue.Dequeue();
        if (clientInput.ShouldReconcile(clientInput))
        {
          Debug.Log($"{name}: Reconciling input - {clientInput}");
          break;
        }

        TState clientState = stateQueue.Dequeue();
        bool   validState  = false;

        if (isFixed)
          validState = FixedSimulate(clientInput, out predictedState);
        else
          validState = Simulate(clientInput, out predictedState);

        if (!validState || predictedState.ShouldReconcile(clientState, predictedState))
        {
          Debug.Log($"{name}: Reconciling state - {clientState} - {predictedState}");
          break;
        }

        reconcile = false;
      }

      if (!reconcile)
        return;

      inputQueue.Clear();
      stateQueue.Clear();
      SyncStateClientRpc(predictedState);
    }

    private void ServerTick()
    {
      CheckServerState(_serverInputQueue, _serverStateQueue, false);
    }

    private void FixedServerTick()
    {
      CheckServerState(_serverFixedInputQueue, _serverFixedStateQueue, true);
    }

    private void OwnerTick()
    {
      if (!ReadInput(out TInput input) || !Simulate(input, out TState state))
        return;

      SyncStateServerRpc(input, state, false);
    }

    private void FixedOwnerTick()
    {
      if (!ReadInput(out TInput input) || !FixedSimulate(input, out TState state))
        return;

      SyncStateServerRpc(input, state, true);
    }

    protected abstract void Reconcile(TState reconcileState);

    protected abstract bool ReadInput(out TInput input);

    protected virtual bool FixedSimulate(TInput input, out TState state)
    {
      state = default;
      return false;
    }

    protected virtual bool Simulate(TInput input, out TState state)
    {
      state = default;
      return false;
    }
  }
}