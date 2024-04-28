#nullable enable

namespace OdysseyXR.ODK.Attributes.ECS
{
  using System;

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class BakeAsEntityAttribute : Attribute
  {
    public string? Name;

    public BakeAsEntityAttribute(string? name = null)
    {
      Name = name;
    }
  }
}