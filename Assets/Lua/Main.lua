--package.cpath = package.cpath .. ';C:/Users/admin/AppData/Roaming/JetBrains/Rider2021.3/plugins/EmmyLua/debugger/emmy/windows/x64/?.dll'
--local dbg = require('emmy_core')
--dbg.tcpConnect('localhost', 23124)

print("准备就绪")

-- 初始化所有准备好的类别名
require("InitClass")


-- -------------------- 初始化数据 --------------------
-- 初始化道具信息
require("ItemData")
-- 初始化玩家信息
require("PlayerData")
PlayerData:Init()


-- -------------------- 游戏逻辑 --------------------
require("BasePanel")
require("MainPanel")
require("BagPanel")
require("Grid")

MainPanel:Show("Main Panel")
