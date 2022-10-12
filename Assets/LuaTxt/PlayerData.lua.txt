-- 单机：本地（PlayerPrefs，JSON，二进制...等）
-- 网络：服务器

PlayerData = {}

-- 目前只做背包功能，只需要道具信息
PlayerData.equips = {}
PlayerData.items = {}
PlayerData.gems = {}

-- 玩家数据初始化
function PlayerData:Init()
    -- 道具只存ID和数量，其他的信息不需要存
    
    -- 因为没有服务器数据/本地存档文件，暂时写固定值做测试
    table.insert(self.equips, { id = 1, amount = 1 })
    table.insert(self.equips, { id = 2, amount = 1 })

    table.insert(self.items, { id = 3, amount = 10 })
    table.insert(self.items, { id = 4, amount = 8 })

    table.insert(self.gems, { id = 5, amount = 20 })
    table.insert(self.gems, { id = 6, amount = 30 })
end

