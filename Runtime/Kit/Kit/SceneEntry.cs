using System.Threading.Tasks;
using ODK.Kit.Interfaces;
using UnityEngine;

namespace ODK.Kit
{
  public abstract class SceneEntry : MonoBehaviour
  {
    private async void Awake()
    {
      AppContext context = new();
      await OnSceneLoaded(context);
    }

    protected abstract Task OnSceneLoaded(IAppContext context);
  }
}