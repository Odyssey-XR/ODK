#nullable enable

namespace OdysseyXR.Arcadia.Plugins.ODK.Core.Ecs
{
  using System;

  [AttributeUsage(AttributeTargets.Field)]
  public class AuthoredNameAttribute : Attribute
  {
    public string PropertyPath;
    public bool   AllowNull;

    public AuthoredNameAttribute(string propertyPath, bool allowNull = false)
    {
      PropertyPath = propertyPath;
      AllowNull    = allowNull;
    }
  }
}