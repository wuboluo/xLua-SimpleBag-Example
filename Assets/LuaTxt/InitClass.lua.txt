
require("Object")
require("SplitTools")
Json = require("JsonUtility")

GameObject = CS.UnityEngine.GameObject
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
Resources = CS.UnityEngine.Resources

Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2

UIBehaviour = CS.UnityEngine.EventSystems.UIBehaviour

-- 图集对象类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas

-- 文本文件类
TextAsset = CS.UnityEngine.TextAsset

-- UI
UI = CS.UnityEngine.UI
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect

-- 此项目的不需要切换场景，只有一个Canvas
Canvas = GameObject.Find("Canvas").transform

-- AB
-- 直接得到AB包管理器的单例对象
ABManager = CS.ABManager.Instance

