using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        LuaManager.Instance().Init();
        LuaManager.Instance().DoLuaFile("Main");
    }
}