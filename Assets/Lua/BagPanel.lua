BasePanel:subClass("BagPanel")

BagPanel.content = nil

-- 记录当前存在格子
BagPanel.currentGrids = {}
-- 记录当前选择的页签，避免重复刷新
BagPanel.currentType = nil

-- 成员方法
function BagPanel:Init(panelName)
    self.base.Init(self, panelName)
    
    if self.initializedEvent == false then
        self.content = self:GetBehaviours("Bag Scroll", "ScrollRect").transform:Find("Viewport"):Find("Content")

        -- 为控件添加事件监听
        -- 关闭按钮
        self:GetBehaviours("Close Button", "Button").onClick:AddListener(function()
            self:Hide()
        end)
        -- 单选框事件
        -- Toggle对应的委托是Unity泛型委托，需要定义该委托放进xLua规定的静态函数的静态列表中
        self:GetBehaviours("Equip Toggle", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:SwitchType(1)
            end
        end)
        self:GetBehaviours("Item Toggle", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:SwitchType(2)
            end
        end)
        self:GetBehaviours("Gem Toggle", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:SwitchType(3)
            end
        end)

        self.initializedEvent = true
    end
end

function BagPanel:Show(panelName)
    self.base.Show(self, panelName)
    
    -- 第一次打开背包的时候，默认更新第一个页签。否则，显示上次关闭前的页签
    if BagPanel.currentType == nil then
        self:SwitchType(1)
    else
        self:SwitchType(BagPanel.currentType)
    end
end

-- 1:Equip 2:Item 3:Gem
function BagPanel:SwitchType(type)
    -- 已经是该页签，不在更新
    if type == BagPanel.currentType then
        return
    end
    BagPanel.currentType = type

    -- 1，移除旧的格子
    for i = 1, #self.currentGrids do
        self.currentGrids[i]:Destroy()
    end
    self.currentGrids = {}

    -- 2，根据页签生成对应的新的格子
    -- 根据页签得到对应的数据表
    local currentBagTypeData
    if type == 1 then
        currentBagTypeData = PlayerData.equips
    elseif type == 2 then
        currentBagTypeData = PlayerData.items
    else
        currentBagTypeData = PlayerData.gems
    end

    -- 创建格子
    for i = 1, #currentBagTypeData do
        local grid = Grid:new()
        grid:Init(self.content)
        grid:InitData(currentBagTypeData[i])

        -- 保存到当前格子的表中
        table.insert(self.currentGrids, grid)
    end
end 