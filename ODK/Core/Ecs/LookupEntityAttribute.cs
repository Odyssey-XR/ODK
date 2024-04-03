namespace OdysseyXR.Arcadia.Plugins.ODK.Core.Ecs
{
  using System;

  [AttributeUsage(AttributeTargets.Field)]
  public class LookupEntityAttribute : Attribute
  {
    public string PropertyPath;

    public LookupEntityAttribute(string propertyPath)
    {
      PropertyPath = propertyPath;
    } 
  }
}