# Welcome to EzDatabase docs!

# Example usage
## Making a database
Once you have the dependencies installed, you can initiate a database like this:
```cs
var database = new Database("Database Name");
```
You can then create a category inside the db:
```cs
var category = database.CreateCategory("A category");
```
You can also make subcategories inside categories
```cs
var subcategory = category.CreateSubCategory("a subcategory");
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
## Reading and processing data
You can retrieve json data from your database
```cs
//directly deserializes the json file
category.GetJson<ClassType>("filename");

//gets the FileInfo for the json file
category.GetJson("filename");
```
You can also get a list of all the json files in the category
```cs
//get a list of names of json files in the category
var jsoninfo = category.GetAllJson();

//or directly deserialize all the data at once into a list
var jsondata = category.GetAllJson<ClassType>();
```
To get files in your category
```cs
//returns a FileInfo object
var file = category.GetFile("my image", ".jpg");

//gets a list of all files in the category
var files = category.GetAllFiles();

//or all files with the specified extension
var jpgfiles = category.GetAllFiles(".jpg");
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
