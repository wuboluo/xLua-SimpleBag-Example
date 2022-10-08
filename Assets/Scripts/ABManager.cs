// 知识点：
// 1，AB包相关 API
// 2，单例
// 3，委托（lambda表达式）
// 4，协程
// 5，字典

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class ABManager : SingletonMono<ABManager>
{
    // AB包管理器
    // 主要目的：让外部更方便的进行资源加载

    // 使用字典记录加载的 AB包避免重复加载报错
    private readonly Dictionary<string, AssetBundle> abDict = new();

    // 主包
    private AssetBundle mainAB;

    // 依赖包获取用的配置文件
    private AssetBundleManifest manifest;

    // AB包存放路径
    private string PathUrl => Application.streamingAssetsPath + "/";

    // 主包名
    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "ANDROID";
#else
            return "PC";
#endif
        }
    }

    // 加载 AB包
    public void LoadAB(string abName)
    {
        // 加载主包，并通过主包获取依赖包的配置文件
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        // 获取依赖包相关信息
        AssetBundle ab;
        var infos = manifest.GetAllDependencies(abName);
        foreach (var info in infos)
            // 判断依赖包是否加载过
            if (!abDict.ContainsKey(info))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + info);
                abDict.Add(info, ab);
            }

        // 加载资源来源包
        // 如果没有加载过，再加载
        if (!abDict.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDict.Add(abName, ab);
        }
    }

    #region 同步加载

    // 同步加载（不指定类型）
    public Object LoadRes(string abName, string resName)
    {
        LoadAB(abName);

        // 加载资源
        var obj = abDict[abName].LoadAsset(resName);

        // 如果是 GameObject，就直接实例化出来再返回出去
        return obj is GameObject ? Instantiate(obj) : obj;
    }

    // 同步加载（根据 Type）
    public Object LoadRes(string abName, string resName, Type type)
    {
        LoadAB(abName);

        var obj = abDict[abName].LoadAsset(resName, type);
        return obj is GameObject ? Instantiate(obj) : obj;
    }

    // 同步加载（使用泛型）
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);

        var obj = abDict[abName].LoadAsset<T>(resName);
        return obj is GameObject ? Instantiate(obj) : obj;
    }

    #endregion

    #region 卸载

    // 单个包卸载
    public void UnloadAB(string abName)
    {
        if (abDict.ContainsKey(abName))
        {
            abDict[abName].Unload(false);
            abDict.Remove(abName);
        }
    }

    // 所有包卸载
    public void UnloadAllAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDict.Clear();

        mainAB = null;
        manifest = null;
    }

    #endregion

    #region 异步加载：这里只是加载资源时使用异步

    // 异步加载（使用 Type）
    public void LoadResAsync(string abName, string resName, Type type, UnityAction<Object> callback)
    {
        StartCoroutine(LoadResAsyncCor(abName, resName, type, callback));
    }

    private IEnumerator LoadResAsyncCor(string abName, string resName, Type type, UnityAction<Object> callback)
    {
        LoadAB(abName);

        var obj = abDict[abName].LoadAssetAsync(resName, type);
        yield return obj;

        // 异步加载结束后，通过委托传递给外部使用
        callback?.Invoke(obj.asset is GameObject ? Instantiate(obj.asset) : obj.asset);
    }

    // 异步加载（使用泛型）
    public void LoadResAsync<T>(string abName, string resName, UnityAction<Object> callback) where T : Object
    {
        StartCoroutine(LoadResAsyncCor<T>(abName, resName, callback));
    }

    private IEnumerator LoadResAsyncCor<T>(string abName, string resName, UnityAction<Object> callback) where T : Object
    {
        LoadAB(abName);

        var obj = abDict[abName].LoadAssetAsync<T>(resName);
        yield return obj;

        callback?.Invoke(obj.asset is GameObject ? Instantiate(obj.asset) : obj.asset);
    }

    #endregion
}