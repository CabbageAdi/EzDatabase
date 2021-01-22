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
        /// Creates a new database
        /// </summary>
        /// <param name="name">The name of the database to create</param>
        public Database(string name)
        {
            Name = name;

            Initialize();
        }

        internal void Initialize()
        {
            var directory = Directory.CreateDirectory(Name);
            FullPath = directory.FullName;
        }

        /// <summary>
        /// Gets or creates a category in the database
        /// </summary>
        /// <param name="name">The name of the category to create</param>
        /// <returns>The category created</returns>
        public DatabaseCategory CreateCategory(string name)
        {
            var category = new DatabaseCategory(this, name);
            return category;
        }

        /// <summary>
        /// Gets or creates a category in the database
        /// </summary>
        /// <param name="name">The name of the category to get</param>
        /// <returns>The category requested</returns>
        public DatabaseCategory GetCategory(string name)
        {
            var category = new DatabaseCategory(this, name);
            return category;
        }

        /// <summary>
        /// Gets a list of all the categories in the database
        /// </summary>
        /// <returns>A list of all the categories</returns>
        public IReadOnlyList<DatabaseCategory> GetCategories()
        {
            var info = new DirectoryInfo(Name);
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
            Directory.Delete($"{Name}\\{name}", true);
        }
    }
}
