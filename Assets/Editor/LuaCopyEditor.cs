using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaCopyEditor : Editor
{
    [MenuItem("XLua/.lua —> .lua.txt")]
    public static void CopyLuaToTxt()
    {
        // lua文件所在路径
        var path = Application.dataPath + "/Lua/";
        if (!Directory.Exists(path)) return;

        // 得到每一个lua文件的路径
        var filesPath = Directory.GetFiles(path, "*.lua");

        // 目标路径
        var newPath = Application.dataPath + "/LuaTxt/";
        if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

        // 清空之前的文件，避免出现旧文件遗留
        var oldFilesPath = Directory.GetFiles(newPath, "*.txt");
        foreach (var s in oldFilesPath) File.Delete(s);

        // 拷贝
        var txtFilesPath = new List<string>();
        foreach (var s in filesPath)
        {
            var fileName = newPath + s[(s.LastIndexOf('/') + 1)..] + ".txt";
            txtFilesPath.Add(fileName);
            File.Copy(s, fileName);
        }

        // 刷新资产列表
        AssetDatabase.Refresh();

        // 刷新过后再改指定AB包。否则第一次改没有效果
        foreach (var s in txtFilesPath)
        {
            // GetAtPath()传入的路径必须是相对Assets的。 Assets/../..
            var importer = AssetImporter.GetAtPath(s[s.IndexOf("Assets", StringComparison.Ordinal)..]);
            if (importer != null)
                importer.assetBundleName = "lua";
        }
    }
}