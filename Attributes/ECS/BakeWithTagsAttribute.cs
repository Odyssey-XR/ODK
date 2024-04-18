#nullable enable

namespace OdysseyXR.ODK.Attributes.ECS
{
  using System;

  [AttributeUsage(AttributeTargets.Class)]
  public class BakeWithTagsAttribute : Attribute
  {
    public Type[] ComponentTagTypes;

    public BakeWithTagsAttribute(params Type[] componentTagTypes)
    {
      ComponentTagTypes = componentTagTypes;
    }
  }
}