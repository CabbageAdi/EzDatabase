using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzDatabase
{
    /// <summary>
    /// Represents a database object
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Gets the name of the database
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the path of this database
        /// </summary>
        public string FullPath { get; internal set; }

        /// <summary>
        /// Gets the directory this database is associated with
        /// </summary>
        public DirectoryInfo BaseDirectory { get; internal set; }

        internal string BaseDirectoryPath;

        /// <summary>
        /// Creates a new database
        /// </summary>
        /// <param name="name">The name of the database to create</param>
        /// <param name="basepath">The path to make the database in</param>
        public Database(string name, string basepath = "")
        {
            Name = name;
            BaseDirectoryPath = basepath;

            Initialize();
        }

        internal void Initialize()
        {
            DirectoryInfo directory;
            if(BaseDirectoryPath == "")
            {
                directory = Directory.CreateDirectory($"{Name}");
            }
            else
            {
                directory = Directory.CreateDirectory($"{BaseDirectoryPath}\\{Name}");
            }
            FullPath = directory.FullName;
            BaseDirectory = directory;
        }

        /// <summary>
        /// Gets or creates a category in the database
        /// </summary>
        /// <param name="name">The name of the category to create</param>
        /// <returns>The category created</returns>
        public DatabaseCategory CreateCategory(string name)
        {
            return new DatabaseCategory(this, name);
        }

        /// <summary>
        /// Gets a category in the database, returns null if no category of the name is found
        /// </summary>
        /// <param name="name">The name of the category to get</param>
        /// <returns>The category requested, null if not found</returns>
        public DatabaseCategory GetCategory(string name)
        {
            if (!Directory.Exists($"{FullPath}\\{name}")) return null;
            return new DatabaseCategory(this, name);
        }

        /// <summary>
        /// Gets a list of all the categories in the database
        /// </summary>
        /// <returns>A list of all the categories</returns>
        public IReadOnlyList<DatabaseCategory> GetCategories()
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
        /// Deletes a category in the database
        /// <para>WARNING: This irreversibly deletes the category and all the data inside of it</para>
        /// </summary>
        /// <param name="name">The name of the category to delete</param>
        public void DeleteCategory(string name)
        {
            Directory.Delete($"{FullPath}\\{name}", true);
        }
    }
}
