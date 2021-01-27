using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;
using EzDatabase.AsyncExtension;

namespace EzDatabase
{
    /// <summary>
    /// Represents a database category
    /// </summary>
    public class DatabaseCategory
    {
        /// <summary>
        /// Gets the name of the category
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the base database this category is in
        /// </summary>
        public Database BaseDatabase { get; internal set; }

        /// <summary>
        /// Gets the full path of this category
        /// </summary>
        public string FullPath { get; internal set; }

        /// <summary>
        /// Gets the directory this category is associated with
        /// </summary>
        public DirectoryInfo BaseDirectory { get; internal set; }

        /// <summary>
        /// If a subcategory, gets the parent category, otherwise returns null
        /// </summary>
        public DatabaseCategory ParentCategory { get; internal set; }

        internal DatabaseCategory(Database database, string name)
        {
            Name = name;
            BaseDatabase = database;
            ParentCategory = null;

            Initailize();
        }

        internal DatabaseCategory(DatabaseCategory category, string name)
        {
            Name = name;
            BaseDatabase = category.BaseDatabase;
            ParentCategory = category;

            InitializeSubCategory();
        }

        internal void Initailize()
        {
            FullPath = $"{BaseDatabase.FullPath}\\{Name}";
            var directory = Directory.CreateDirectory(FullPath);
            FullPath = directory.FullName;
            BaseDirectory = directory;
        }

        internal void InitializeSubCategory()
        {
            FullPath = $"{ParentCategory.FullPath}\\{Name}";
            var directory = Directory.CreateDirectory(FullPath);
            FullPath = directory.FullName;
            BaseDirectory = directory;
        }

        /// <summary>
        /// Creates a subcategory in this category
        /// </summary>
        /// <param name="name">Get name of the category to create</param>
        /// <returns>The category created</returns>
        public DatabaseCategory CreateSubCategory(string name)
        {
            return new DatabaseCategory(this, name);
        }

        /// <summary>
        /// Gets a subcategory in this category, returns null if no category of the name is found
        /// </summary>
        /// <param name="name">The name of the category to get</param>
        /// <returns>The category retrieved, null if no category is found</returns>
        public DatabaseCategory GetSubCategory(string name)
        {
            if (!Directory.Exists($"{FullPath}\\{name}")) return null;
            return new DatabaseCategory(this, name);
        }

        /// <summary>
        /// Gets a list of all the subcategories in this category
        /// </summary>
        /// <returns>A list of subcategories</returns>
        public IReadOnlyList<DatabaseCategory> GetSubCategories()
        {
            var info = new DirectoryInfo(FullPath);
            var directories = info.GetDirectories();
            var result = new List<DatabaseCategory>();
            foreach (var directory in directories)
            {
                result.Add(new DatabaseCategory(this, directory.Name));
            }
            return result;
        }

        /// <summary>
        /// Deletes a subcategory in this category
        /// <para>WARNING: This irreversibly deletes the category and all the data inside of it</para>
        /// </summary>
        /// <param name="name">The name of the subcategory to delete</param>
        public void DeleteSubCategory(string name)
        {
            Directory.Delete($"{FullPath}\\{name}", true);
        }

        /// <summary>
        /// Saves or overwrites a json file in this category
        /// </summary>
        /// <param name="name">The name of the file to create</param>
        /// <param name="data">The object to serialize and store</param>
        public void SaveJson(string name, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText($"{FullPath}\\{name}.json", json);
        }

        /// <summary>
        /// Gets a json file in this category
        /// </summary>
        /// <typeparam name="T">The type of the json file</typeparam>
        /// <param name="name">The name of the json file</param>
        /// <returns>The deserialized object from the json file</returns>
        public T GetJson<T>(string name)
        {
            var FilePath = $"{FullPath}\\{name}.json";
            var text = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<T>(text);
        }
        /// <summary>
        /// Gets a json file in this category
        /// </summary>
        /// <param name="name">The name of the json file</param>
        /// <returns>A FileInfo object corresponding to the json file</returns>
        public FileInfo GetJson(string name)
        {
            return new FileInfo($"{FullPath}\\{name}.json");
        }

        /// <summary>
        /// Gets whether a json file with the specified name exists in this category
        /// </summary>
        /// <param name="name">The name of the json file to check for</param>
        /// <returns>A bool indicating whether the json file exists</returns>
        public bool HasJson(string name)
        {
            return File.Exists($"{FullPath}\\{name}.json");
        }

        /// <summary>
        /// Gets a list of names of all json files in this category
        /// </summary>
        /// <returns>A list of the names of json files in this category</returns>
        public IReadOnlyList<string> GetAllJson()
        {
            var info = new DirectoryInfo(FullPath);
            var files = info.GetFiles();
            var filenames = new List<string>();
            foreach (var file in files)
            {
                if (file.Extension == ".json")
                {
                    filenames.Add(Path.GetFileNameWithoutExtension(file.Name));
                }
            }
            return filenames;
        }
        /// <summary>
        /// Gets a dictionary of all json files in the category deserialized to the given Type
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <returns>A dictionary with the key being the name of the file and the value a the object</returns>
        public IReadOnlyDictionary<string, T> GetAllJson<T>()
        {
            var info = new DirectoryInfo(FullPath);
            var files = info.GetFiles();
            var dictionary = new Dictionary<string, T>();
            foreach (var file in files)
            {
                if (file.Extension == ".json")
                {
                    dictionary.Add(Path.GetFileNameWithoutExtension(file.Name), JsonConvert.DeserializeObject<T>(File.ReadAllText(file.FullName)));
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Deletes a json file in this category
        /// </summary>
        /// <param name="name"></param>
        public void DeleteJson(string name)
        {
            File.Delete($"{FullPath}\\{name}.json");
        }

        /// <summary>
        /// Saves the given text in a .txt file
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="text">The text to be written in the file</param>
        public void SaveText(string name, string text)
        {
            File.WriteAllText($"{FullPath}\\{name}.txt", text);
        }

        /// <summary>
        /// Gets the text from the given file
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <returns>The text from the file</returns>
        public string GetText(string name)
        {
            return File.ReadAllText($"{FullPath}\\{name}.txt");
        }

        /// <summary>
        /// Gets whether a text file with the specified name exists in this category
        /// </summary>
        /// <param name="name">The name of the text file to check for</param>
        /// <returns>A bool indicating whether the text file with the specified name exists</returns>
        public bool HasText(string name)
        {
            return File.Exists($"{FullPath}\\{name}.txt");
        }

        /// <summary>
        /// Gets a list of all text files from the category
        /// </summary>
        /// <returns>A dictionary with the key being the name of the file and the value the text in the file</returns>
        public IReadOnlyDictionary<string, string> GetAllText()
        {
            var info = new DirectoryInfo(FullPath);
            var files = info.GetFiles();
            var dictionary = new Dictionary<string, string>();
            foreach (var file in files)
            {
                if (file.Extension == ".txt")
                {
                    dictionary.Add(Path.GetFileNameWithoutExtension(file.Name), File.ReadAllText(file.FullName));
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Deletes the specified text file
        /// </summary>
        /// <param name="name">The name of the text file to delete</param>
        public void DeleteText(string name)
        {
            File.Delete($"{FullPath}\\{name}.txt");
        }

        /// <summary>
        /// Saves a file in this category with the specified name, extension and data
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        /// <param name="data">The data to be stored in the file</param>
        public void SaveFile(string name, string extension, string data)
        {
            File.WriteAllText($"{FullPath}\\{name}{extension}", data);
        }
        /// <summary>
        /// Saves a file in this category with the specified name, extension and data
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        /// <param name="data">The data to be stored in the file</param>
        public void SaveFile(string name, string extension, byte[] data)
        {
            File.WriteAllBytes($"{FullPath}\\{name}{extension}", data);
        }
        /// <summary>
        /// Saves a file in this category with the specified name, extension and data
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        /// <param name="data">The data to be stored in the file</param>
        public void SaveFile(string name, string extension, Stream data)
        {
            var path = $"{FullPath}\\{name}{extension}";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using FileStream fileStream = File.Create(path);
            data.CopyTo(fileStream);
        }

        /// <summary>
        /// Gets a file in this category with the specified name and extension
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file with a period (for example: ".jpg")</param>
        public FileInfo GetFile(string name, string extension)
        {
            return new FileInfo($"{FullPath}\\{name}{extension}");
        }

        /// <summary>
        /// Gets whether a file with the specified name and extension exists in this category
        /// </summary>
        /// <param name="name">The name of the file to look for</param>
        /// <param name="extension">The extension of the file to look for with a period (for example: ".jpg")</param>
        /// <returns>A bool indicating whether the file with the specified name and extension exists in this category</returns>
        public bool HasFile(string name, string extension)
        {
            return File.Exists($"{FullPath}\\{name}{extension}");
        }

        /// <summary>
        /// Gets a list of all the files in this category
        /// </summary>
        /// <returns>A list of files in this category</returns>
        public IReadOnlyList<FileInfo> GetAllFiles()
        {
            var info = new DirectoryInfo(FullPath);
            return info.GetFiles();
        }
        /// <summary>
        /// Gets a list of all the files with the specified extension in this category
        /// </summary>
        /// <param name="extension">The extension of the files to find with a period (for example: ".jpg")</param>
        /// <returns>A list of files with the specified extension</returns>
        public IReadOnlyList<FileInfo> GetAllFiles(string extension)
        {
            var info = new DirectoryInfo(FullPath);
            var files = info.GetFiles();
            return files.Where(file => file.Extension == extension).ToList();
        }

        /// <summary>
        /// Deleted the specified file in the category
        /// </summary>
        /// <param name="name">The name of the file to delete</param>
        /// <param name="extension">The extension of the file to delete with a period (for example: ".jpg")</param>
        public void DeleteFile(string name, string extension)
        {
            File.Delete($"{FullPath}\\{name}{extension}");
        }
    }
}
