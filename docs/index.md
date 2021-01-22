# Welcome to the EzDatabase docs!
EzDatabase is a easy to use alternative to other database storage solutions like SQL that directly saves your data in simple files and folders, for ease of access and usage
# Example usage
Currently there is no way to directly install the package, so you can instead clone the project, build it and reference the dlls in your project
## Making a database
Once you have the dependencies installed, you can initiate a database like this:
```cs
Database database = new Database("Database Name");
```
You can then create a category inside the db:
```cs
DatabaseCategory category = database.CreateCategory("A category");
```
You can also make subcategories inside categories
```cs
DatabaseCategory subcategory = category.CreateSubCategory("a subcategory");
```
And use it as a regular category
## Storing data
To store an object as json inside the category, you can simply do
```cs
category.SaveJson(MyObject);
```
To store a file, you can do
```cs
//to store a string
category.SaveFile("filename", ".txt", "my text");

//to store a byte array
category.SaveFile("filename", ".mp3", ByteArray);

//to store a Stream
category.SaveFile("filename", ".jpg", MyStream);
```
To store simple text (as .txt), you can do
```cs
category.SaveText("filename", "my text");
```
## Reading and processing data
You can retrieve json data from your database
```cs
//directly deserializes the json file
ClassType data = category.GetJson<ClassType>("filename");

//gets the FileInfo for the json file
FileInfo filedata = category.GetJson("filename");
```
You can also get a list of all the json files in the category
```cs
//get a list of names of json files in the category
IReadOnlyList<string> jsoninfo = category.GetAllJson();

//or directly deserialize all the data at once into a dictionary
//the key is the file name
IReadOnlyDictionary<string, ClassType> jsondata = category.GetAllJson<ClassType>();
```
To get files in your category
```cs
//to get a single file
FileInfo file = category.GetFile("my image", ".jpg");

//gets a list of all files in the category
IReadOnlyList<FileInfo> files = category.GetAllFiles();

//or all files with the specified extension
IReadOnlyList<FileInfo> jpgfiles = category.GetAllFiles(".jpg");
```
To get text (.txt) files in your category
```cs
//to get data from a single file
string textfile = category.GetText("filename");

//to get all text files
//the key is the file name
IReadOnlyDictionary<string, string> textfiles = category.GetAllText();
```
## Deleting data
To delete a json file
```cs
category.DeleteJson("filename");
```
To delete a file
```cs
category.DeleteFile("filename", ".jpg");
```
To delete a text file
```cs
category.DeleteText("filename");
```