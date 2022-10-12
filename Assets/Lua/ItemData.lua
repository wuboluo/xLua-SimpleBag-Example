-- 此脚本作用：将Json数据读取并转存到lua中的表进行存储

-- 1，先把JSON从AB包中加载出来
local txt = ABManager:LoadRes("json", "ItemData", typeof(TextAsset))
print(txt.text)

-- 2，获取txt的文本信息，进行json解析
local itemList = Json.decode(txt.text)
-- 加载出来是一个像数组结构的数据。遍历itemList里面的key，是索引，不是物品ID
-- 不方便通过ID去获取里面的内容，所以需要用一个新表去转存
-- for k, v in pairs(itemList) do
     -- print(k,v)  —> 1~6  table
-- end

-- 3，新建新的道具表（键值对形式：key是道具ID，value是其他信息），需要在任何地方都可被使用
ItemData = {}

-- 4，转存至新的道具表
for _, value in pairs(itemList) do
    ItemData[value.ID] = value
end
-- print(ItemData[1].Description)
