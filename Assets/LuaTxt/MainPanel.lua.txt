-- 该脚本作用：实例化面板对象，为这个面板实现对应的逻辑。比如：按钮点击...等

-- 只要是一个新的对象（面板），就先新建一个表
BasePanel:subClass("MainPanel")

-- 关联的面板
MainPanel.panelObj = nil
-- 关联的面板控件
MainPanel.buttonRole = nil
MainPanel.buttonSkill = nil

-- 初始化
function MainPanel:Init(panelName)
    self.base.Init(self, panelName)

    -- 为了只添加一次事件
    if self.initializedEvent == false then
        -- 为控件添加事件监听
        -- 这里如果直接使用self'.'自己的方法，实际上调用者不是MainPanel，而是AddListener方法里面去调用的，所以这里要使用匿名函数
        -- self.buttonRole.onClick:AddListener(self.OnClickButtonRole)  如果这样写，OnClickButtonRole方法里不能再用self，而是要具体使用MainPanel
        self:GetBehaviours("Role Button", "Button").onClick:AddListener(function()
            -- 把实际的方法包裹在匿名函数中
            self:OnClickButtonRole()
        end)
        
        self.initializedEvent = true
    end
end

function MainPanel:OnClickButtonRole()
    BagPanel:Show("Bag Panel")
end