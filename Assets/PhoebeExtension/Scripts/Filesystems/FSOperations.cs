using System;
using System.IO;
using UnityEngine;

public static class FSOperations
{
    // Return enum
    public enum IsFolder_Return
    {
        IsFolder,
        IsFile,
        NotExist
    }
    // Check if file exists, return bool
    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }

    // Check if directory exists, return bool
    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    
    // Check type of specified path, return enum
    public static IsFolder_Return IsFolder(string path)
    {
        if (!DirectoryExists(path) || !FileExists(path)) return IsFolder_Return.NotExist;
        FileAttributes attr = File.GetAttributes(path);

        if (attr.HasFlag(FileAttributes.Directory))
            return IsFolder_Return.IsFolder;
        return IsFolder_Return.IsFile;
    }
    
    // Create directory if doesn't exist
    public static bool CreateDirectory(string path)
    {
        try
        {
            if (DirectoryExists(path)) return false;
            Directory.CreateDirectory(path);
            return true;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    // Write file content, overwrite if told to
    public static bool WriteFile(string path, string content, bool overwrite = false)
    {
        try
        {
            IsFolder_Return isFolder = IsFolder(path);
            if (isFolder == IsFolder_Return.IsFolder) return false;
            if (FileExists(path))
            {
                if (overwrite)
                {
                    File.WriteAllText(path, content);
                    return true;
                }
                
                return false;
            }

            File.WriteAllText(path, content);
            return true;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    // Read file as string
    public static string ReadFile(string path)
    {
        try
        {
            if (!FileExists(path)) return string.Empty;
            return File.ReadAllText(path);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}
