#nullable enable

namespace OdysseyXR.ODK.Extensions
{
  using System.Collections.Generic;
  using Unity.Entities;
  
  public static class BakerExtensions
  {
    public static DynamicBuffer<T> AddBuffer<T>(this IBaker baker, IEnumerable<T> bufferElements, Entity entity) 
      where T : unmanaged, IBufferElementData
    {
      var dynamicBuffer = baker.AddBuffer<T>(entity);
      foreach (var element in bufferElements)
        dynamicBuffer.Add(element);
      
      return dynamicBuffer;
    }
  }
}