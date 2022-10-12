-- 初始化格子对象
-- 生成一个table，继承自Object。目的是要用它里面的new()和subClass()
Object:subClass("Grid")

-- 变量
Grid.obj = nil
Grid.icon = nil
Grid.amount = nil


-- 实例化格子对象
function Grid:Init(parentRoot)
    -- 实例化
    self.obj = ABManager:LoadRes("ui", "Grid")
    -- 设置父节点
    self.obj.transform:SetParent(parentRoot, false)
    -- 得到控件
    self.icon = self.obj.transform:Find("Icon Image"):GetComponent(typeof(Image))
    self.amount = self.obj.transform:Find("Amount Text"):GetComponent(typeof(Text))
end

-- 初始化格子信息
-- data就是PlayerData里面存的三个不同类型物品的信息表（key=id，value=amount）
function Grid:InitData(data)
    -- 通过ID获取信息。之前将ID和其他信息存在了ItemData这张表中
    local info = ItemData[data.id].Icon
    -- 加载图集
    local iconData = string.split(info, "_")
    -- 根据图集中的图标加载图片
    local spriteAtlas = ABManager:LoadRes("ui", iconData[1], typeof(SpriteAtlas))
    -- 设置图标
    self.icon.sprite = spriteAtlas:GetSprite(iconData[2])
    -- 设置数量
    self.amount.text = data.amount
end

-- 自己的逻辑（拖拽，悬停信息...等）
function Grid:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end 