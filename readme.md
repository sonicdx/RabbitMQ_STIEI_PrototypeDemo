


上海电子信息职业技术学院 消息总线原型演示 
===========================================================
[TOC]

## 子项目说明

[AppBase_Send]  模拟应用消息发送端







## 项目说明：

​	**AppBase_Send** 模拟一个应用，产生一个消息源。应用名 `ClockTick` , 其消息交换机名称同应用名。 应用编号0x01020304。







## 技术架构

​	**AppBase_Send** 建立了一个名为`ClockTick` ，类型为 “Topic" 的交换机。

​	它有2个消息队列：

1. *SecondTick* 消息： 队列路由名“01020304.SecondTick", `MsgTypeID` 为 0x00000001。

   该消息每隔一秒发送一次。消息内容正文为一个UTF8编码的JSON字符串：

   ```json
   {
     Msg:"SecondTick",
     Year:2017,
     DayofYear:45,
     Hour:12,
     Minue:15,
     Second:12
   }
   ```

2. MinuteTick 消息：队列路由名“01020304.MinuteTick ", `MsgTypeID` 为 0x00000002。
   该消息每隔一分钟发送一次。消息内容正文为一个UTF8编码的JSON字符串：
   ```json
   {
     Msg:"MinuteTick",
     Year:2017,
     DayofYear:45,
     Hour:12,
     Minue:15,
     Second:12
   }
   ```