using System;
using System.IO;
using UnityEngine;

public static class FSOperations
{
    public enum IsFolder_Return
    {
        IsFolder,
        IsFile,
        NotExist
    }
    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public static IsFolder_Return IsFolder(string path)
    {
        if (!DirectoryExists(path) || !FileExists(path)) return IsFolder_Return.NotExist;
        FileAttributes attr = File.GetAttributes(path);

        if (attr.HasFlag(FileAttributes.Directory))
            return IsFolder_Return.IsFolder;
        return IsFolder_Return.IsFile;
    }
    
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
