#nullable enable

namespace ODK.Netcode.Collections
{
  public record RelayData
  {
    public string IPV4Address;
    public ushort Port;
    public byte[] AllocationId;
    public byte[] Key;
    public byte[] ConnectionData;
    public byte[]? HostConnectionData;
    public bool IsSecure;

    public RelayData(
      string ipv4Address,
      ushort port,
      byte[] allocationId,
      byte[] key,
      byte[] connectionData,
      byte[]? hostConnectionData = null,
      bool isSecure = false)
    {
      IPV4Address = ipv4Address;
      Port = port;
      AllocationId = allocationId;
      Key = key;
      ConnectionData = connectionData;
      HostConnectionData = hostConnectionData;
      IsSecure = isSecure;
    }
  }
}