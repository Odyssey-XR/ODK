namespace OdysseyXR.ODK.Behaviours.ECS
{
  using OdysseyXR.ODK.Attributes.ECS;

  [AutoBaker]
  public class MyBaseBehaviour : AuthoredBehaviour
  {
    [BakeAsField]  public int               MyInt;
    [BakeAsEntity] public MyEntityBehaviour MyEntityBehaviour;
  }
}