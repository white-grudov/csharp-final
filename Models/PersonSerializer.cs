using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace csharp_final.Models
{
    internal class PersonSerializer
    {
        private const string SAVE_PATH = "/Data/persons.json";

        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            IgnoreReadOnlyProperties = true
        };

        public static async Task SavePersonListAsync(ObservableCollection<Person> persons)
        {
            string applicationPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
            string savePath = applicationPath + SAVE_PATH;


            await using FileStream createStream = File.Create(savePath);
            await JsonSerializer.SerializeAsync(createStream, persons, _options);
        }

        public static async Task<ObservableCollection<Person>> LoadPersonListAsync()
        {
            string applicationPath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
            string loadPath = applicationPath + SAVE_PATH;

            if (!File.Exists(loadPath))
            {
                return PersonGenerator.GeneratePeople(50);
            }

            await using FileStream openStream = File.OpenRead(loadPath);
            return (await JsonSerializer.DeserializeAsync<ObservableCollection<Person>>(openStream))!;
        }
    }
}
