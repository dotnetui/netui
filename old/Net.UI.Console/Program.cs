// See https://aka.ms/new-console-template for more information
using Net.Essentials;

using Realms;

Console.WriteLine("Hello, World!");
var driver = new RealmDriver(1, "program.realm");
var table = new Table(() => driver.GetRealm());
await table.AddAsync(new MyRecord
{
    Name = "Test"
});
var all = await table.GetAllAsync();
foreach (var item in all)
    Console.WriteLine(item.Name);

public class MyRecord : RealmObject, IRecord
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

public class MyModel
{
    public string? Name { get; set; }
}

public class Table : RealmTable<MyRecord, MyModel, MyRecord>
{
    public Table(Func<Realm> getRealm) : base(getRealm)
    {
    }
}