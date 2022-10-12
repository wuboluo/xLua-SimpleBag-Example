using System.IO;
using UnityEngine;
using XLua;

// 主要提供 lua解析器，保证解析器的惟一性
public class LuaManager : Singleton<LuaManager>
{
    private LuaEnv luaEnv;

    /// 得到 lua中的 _G 
    public LuaTable Global => luaEnv.Global;

    /// 初始化解析器
    public void Init()
    {
        luaEnv ??= new LuaEnv();

        // 加载 lua脚本重定向
        // luaEnv.AddLoader(MyCustomLoader);
        luaEnv.AddLoader(CustomABLoader);
    }

    /// 传入 lua文件名执行 lua脚本
    public void DoLuaFile(string luaFileName)
    {
        DoString($"require('{luaFileName}')");
    }

    /// 执行lua语言
    public void DoString(string luaCode)
    {
        luaEnv?.DoString(luaCode);
    }

    /// 释放lua垃圾
    public void Tick()
    {
        luaEnv?.Tick();
    }

    /// 销毁解析器
    public void Dispose()
    {
        luaEnv?.Dispose();
        luaEnv = null;
    }

    /// 自动执行
    private byte[] MyCustomLoader(ref string filepath)
    {
        // 通过函数中的逻辑去加载 lua文件
        // 传入的参数时 require执行的lua脚本文件名
        // 拼接一个 lua文件所在路径
        var path = Application.dataPath + "/Lua/" + filepath + ".lua";

        // 有路径就去加载文件
        // File知识点：C#提供的文件读写的类
        if (File.Exists(path)) // 判断文件是否存在
            return File.ReadAllBytes(path);

        Debug.Log("Error：MyCustomLoader重定向失败，文件名为：" + filepath);

        return null;
    }


    /// 重定向加载 AB包中的 lua脚本
    // lua脚本最终会放在 AB包中
    // 最终会通过加载 AB包，去加载其中的 lua脚本资源，再执行它
    // AB包中如果要加载文本，后缀存在一定的限制，'.lua'不能被识别，所以打包时还要改成 '.txt'
    private byte[] CustomABLoader(ref string filepath)
    {
        // // 从 AB包中加载 lua文件
        // // 加载 AB包
        // AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/lua");
        // 
        // // 加载 lua文件
        // TextAsset tx = ab.LoadAsset<TextAsset>(filepath + ".lua");
        // 
        // // 加载 lua文件中的 byte数组
        // return tx.bytes;


        // 加载 lua文件不要用异步加载，因为需要立刻返回内容
        var luaTx = ABManager.Instance.LoadRes<TextAsset>("lua", filepath + ".lua");
        return luaTx != null ? luaTx.bytes : null;
    }
}