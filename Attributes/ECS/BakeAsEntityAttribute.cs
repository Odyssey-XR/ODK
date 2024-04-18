#nullable enable

namespace OdysseyXR.ODK.Attributes.ECS
{
  using System;

  [AttributeUsage(AttributeTargets.Field)]
  public class BakeAsEntityAttribute : Attribute
  {
    public string? Name;

    public BakeAsEntityAttribute(string? name = null)
    {
      Name = name;
    }
  }
}