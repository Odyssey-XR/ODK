#nullable enable

namespace OdysseyXR.ODK.Attributes.ECS
{
  using System;

  [AttributeUsage(AttributeTargets.Class)]
  public class AutoBakerAttribute : Attribute
  {
    public Type? ComponentType;
    
    public AutoBakerAttribute(Type? componentType = null)
    {
      ComponentType = componentType;
    }
  }
}