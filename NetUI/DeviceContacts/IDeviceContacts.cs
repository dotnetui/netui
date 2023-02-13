namespace Net.Essentials.DeviceContacts;

public interface IDeviceContacts
{
    Task<List<DeviceContact>> GetAllAsync();
    Task<List<DeviceContact>> GetAllAsync(bool preferCached);
}