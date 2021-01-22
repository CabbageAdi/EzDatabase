using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Linq;

namespace EzDatabase.AsyncExtension
{
    /// <summary>
    /// Extensions for creating and reading files asynchronously
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Asynchronously saves or overwrites a json file in this category
        /// </summary>
        /// <param name="category">The base category</param>
        /// <param name="name">The name of the file to create</param>
        /// <param name="data">The object to serialize and store</param>
        public static async Task SaveJsonAsync(this DatabaseCategory category, string name, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            await File.WriteAllTextAsync($"{category.Path}\\{name}.json", json);
        }

        /// <summary>
        /// Asynchronously gets a json file in this category
        /// </summary>
        /// <typeparam name="T">The type of the json file</typeparam>
        /// <param name="category">The base category</param>
        /// <param name="name">The name of the json file</param>
        /// <returns>The deserialized object from the json file</returns>
        public static async Task<T> GetJsonAsync<T>(this DatabaseCategory category, string name)
        {
            var FilePath = $"{category.Path}\\{name}.json";
            var text = await File.ReadAllTextAsync(FilePath);
            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        /// Asynchronously gets a list of deseralized json files in this category
        /// </summary>
        /// <typeparam name="T">The type of the json files</typeparam>
        /// <param name="category">The base category</param>
        /// <returns>A list of deserialized objects</returns>
        public static async Task<IReadOnlyList<T>> GetAllJsonAsync<T>(this DatabaseCategory category)
        {
            var info = new DirectoryInfo(category.Path);
            var files = info.GetFiles();
            List<T> result = Array.Empty<T>().ToList();
            foreach (var file in files)
            {
                if (file.Extension == ".json")
                {
                    var text = await File.ReadAllTextAsync(file.FullName);
                    result.Add(JsonConvert.DeserializeObject<T>(text));
                }
            }
            return result;
        }

        /// <summary>
        /// Asynchronously saves a file in this category with the specified name, extension and data
        /// </summary>
        /// <param name="category">The base category</param>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        /// <param name="data">The data to be stored in the file</param>
        public static async Task SaveFileAsync(this DatabaseCategory category, string name, string extension, string data)
        {
            await File.WriteAllTextAsync($"{category.Path}\\{name}{extension}", data);
        }
        /// <summary>
        /// Asynchronously saves a file in this category with the specified name, extension and data
        /// </summary>
        /// <param name="category">The base category</param>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        /// <param name="data">The data to be stored in the file</param>
        public static async Task SaveFileAsync(this DatabaseCategory category, string name, string extension, byte[] data)
        {
            await File.WriteAllBytesAsync($"{category.Path}\\{name}{extension}", data);
        }
        /// <summary>
        /// Asynchronously saves a file in this category with the specified name, extension and data
        /// </summary>
        /// <param name="category">The base category</param>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        /// <param name="data">The data to be stored in the file</param>
        public static async Task SaveFileAsync(this DatabaseCategory category, string name, string extension, Stream data)
        {
            if (data is MemoryStream)
            {
                await File.WriteAllBytesAsync($"{category.Path}\\{name}{extension}", ((MemoryStream)data).ToArray());
            }
            else
            {
                var ms = new MemoryStream();
                await data.CopyToAsync(ms);
                await File.WriteAllBytesAsync($"{category.Path}\\{name}{extension}", ms.ToArray());
            }
        }
    }
}
