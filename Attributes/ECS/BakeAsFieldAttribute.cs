#nullable enable

namespace OdysseyXR.ODK.Attributes.ECS
{
  using System;

  [AttributeUsage(AttributeTargets.Field)]
  public class BakeAsFieldAttribute : Attribute
  {
    public string? Name;

    public BakeAsFieldAttribute(string? name = null)
    {
      Name = name;
    }
  }
}