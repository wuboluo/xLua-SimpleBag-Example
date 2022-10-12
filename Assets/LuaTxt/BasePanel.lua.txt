-- 利用面向对象
Object:subClass("BasePanel")

BasePanel.panelObj = nil
BasePanel.initializedEvent = false

-- 模拟一个字典（key=控件名，value=控件）
BasePanel.behaviours = {}

function BasePanel:Init(panelName)
    if self.panelObj == nil then

        -- 实例化
        self.panelObj = ABManager:LoadRes("ui", panelName, typeof(GameObject))
        self.panelObj.transform:SetParent(Canvas, false)

        -- 找所有UI控件
        local uiBehaviours = self.panelObj:GetComponentsInChildren(typeof(UIBehaviour))
        for i = 0, uiBehaviours.Length - 1 do
            local behaviourName = uiBehaviours[i].name
            -- 根据命名规则查找，避免保存不需要的控件
            if (self:HaveFound(behaviourName, "Button") or
                    self:HaveFound(behaviourName, "Toggle") or
                    self:HaveFound(behaviourName, "Image") or
                    self:HaveFound(behaviourName, "Scroll") or
                    self:HaveFound(behaviourName, "Text")) then

                -- 为了能够取具体类型的控件，需要通过反射保存控件的类型
                local typeName = uiBehaviours[i]:GetType().Name

                -- 避免同一个UI对象上存在多个需要保存的控件导致重叠覆盖的问题
                -- 将key（对象名）对应的value（保存的空间里列表）
                -- 例如：{ xx Button = {Image=控件, Button=控件} }
                if self.behaviours[behaviourName] ~= nil then
                    -- 通过自定义索引的形式，添加一个新的控件键值对
                    self.behaviours[behaviourName][typeName] = uiBehaviours[i]
                else
                    self.behaviours[behaviourName] = { [typeName] = uiBehaviours[i] }
                end
            end
        end
    end
end

function BasePanel:Show(panelName)
    self:Init(panelName)
    self.panelObj:SetActive(true)
end

function BasePanel:Hide()
    self.panelObj:SetActive(false)
end

function BasePanel:HaveFound(behaviourName, keyword)
    return string.find(behaviourName, keyword) ~= nil
end

-- 根据对象名称和想要的控件类型得控件
function BasePanel:GetBehaviours(behaviourName, typeName)
    -- 判断该名称的对象是否被存进字典中
    if self.behaviours[behaviourName] ~= nil then
        -- 得到将这个对象身上符合要求的所有UI控件
        local components = self.behaviours[behaviourName]
        -- 如果有想要的类型的控件，取出
        if components[typeName] ~= nil then
            return components[typeName]
        end
    end

    return nil
end 