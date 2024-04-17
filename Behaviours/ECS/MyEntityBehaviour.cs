namespace OdysseyXR.ODK.Behaviours.ECS
{
  using OdysseyXR.ODK.Attributes.ECS;

  [AutoBaker]
  public class MyEntityBehaviour : AuthoredBehaviour
  {
    [BakeAsField] public float MyFloat;
  }
}