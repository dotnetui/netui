#if IOS
using Contacts;
using Foundation;
#endif

namespace Net.Essentials.DeviceContacts;

public partial class DeviceContacts
{
    List<DeviceContact> cache = null;

    void Cache(List<DeviceContact> results)
    {
        cache = results;
    }

    public void ClearCache()
    {
        cache = null;
    }

    public async Task<List<DeviceContact>> GetAllAsync(bool preferCached)
    {
        if (preferCached && cache != null && cache.Count > 0)
            return cache.ToList();

        return await GetAllAsync();
    }

    internal static string FixSpaces(string original)
    {
        if (string.IsNullOrWhiteSpace(original)) return original;
        var split = original.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", split);
    }

#if IOS
    public IEnumerable<CNContact> GetAllRaw()
    {
        var values = new[]
        {
                CNContactKey.Birthday,
                CNContactKey.Dates,
                CNContactKey.DepartmentName,
                CNContactKey.EmailAddresses,
                CNContactKey.FamilyName,
                CNContactKey.GivenName,
                CNContactKey.Identifier,
                CNContactKey.ImageData,
                CNContactKey.ImageDataAvailable,
                CNContactKey.InstantMessageAddresses,
                CNContactKey.JobTitle,
                CNContactKey.MiddleName,
                CNContactKey.NamePrefix,
                CNContactKey.NameSuffix,
                CNContactKey.Nickname,
                CNContactKey.NonGregorianBirthday,
                //CNContactKey.Note,
                CNContactKey.OrganizationName,
                CNContactKey.PhoneNumbers,
                CNContactKey.PhoneticFamilyName,
                CNContactKey.PhoneticGivenName,
                CNContactKey.PhoneticMiddleName,
                CNContactKey.PhoneticOrganizationName,
                CNContactKey.PostalAddresses,
                CNContactKey.PreviousFamilyName,
                CNContactKey.Relations,
                CNContactKey.SocialProfiles,
                CNContactKey.ThumbnailImageData,
                CNContactKey.Type,
                CNContactKey.UrlAddresses
            };

        return GetAllRaw(values);
    }

    public IEnumerable<CNContact> GetAllRaw(NSString[] keys)
    {
        List<CNContact> results = new List<CNContact>();
        using (var store = new CNContactStore())
        {
            var allContainers = store.GetContainers(null, out var error);

            if (error != null)
                throw new Exception($"iOS Exception: {error}");

            foreach (var container in allContainers)
            {
                try
                {
                    using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                    {
                        var containerResults = store.GetUnifiedContacts(predicate, keys, out error);
                        results.AddRange(containerResults);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DeviceContacts_iOS Error: {ex}");
                    // ignore missed contacts from errors
                }
            }
        }

        return results;
    }

    public Task<List<DeviceContact>> GetAllAsync()
    {
        var results = GetAllRaw().Select(x => CNContactToDeviceContact(x)).ToList();
        Console.WriteLine($"Results Count: {results.Count}");
        Cache(results);
        return Task.FromResult(results);
    }

    static readonly CNPostalAddressFormatter formatter = new CNPostalAddressFormatter();

    public static DeviceContact CNContactToDeviceContact(CNContact item)
    {
        return new DeviceContact
        {
            EmailAddresses = item.EmailAddresses?.Select(x => x.Value?.ToString()).ToArray(),
            PhoneNumbers = item.PhoneNumbers?.Select(x => x.Value?.StringValue).ToArray(),
            PostalAddresses = item.PostalAddresses?.Select(x => formatter.GetStringFromPostalAddress(x.Value)).ToArray(),
            Name = FixSpaces($"{item.NamePrefix} {item.GivenName} {item.MiddleName} {item.FamilyName} {item.NameSuffix}".Trim()),
            //Birthday = item.Birthday?.ToString(),
            //DepartmentName = item.DepartmentName,
            //FamilyName = item.FamilyName,
            //GivenName = item.GivenName,
            //Identifier = item.Identifier,
            //JobTitle = item.JobTitle,
            //MiddleName = item.MiddleName,
            //NamePrefix = item.NamePrefix,
            //NameSuffix = item.NameSuffix,
            //Nickname = item.Nickname,
            //Note = item.Note,
            //OrganizationName = item.OrganizationName,
            Tag = item
        };
    }
#else
    Task<List<DeviceContact>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
#endif
}