# 專案開發流程(Clean Architecture) My Way
---
這邊雖然是掛著Clean Architecture，但更多的是參考[Explicit Architecture](https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/)的設計。

![explicit architecture](./images/explicit_architecture.png)

常見的Hexagonal Architecture、Onion Architecture、Clean Architecture儘管著重的重點有些不同，但概念上是相同的。
在設計規劃上，會使用Event Storming的藍圖來協助進行專案的實作，其優點是Event Storming中的元素內容，能夠很好的貼合專案實作的內容(In-Code)。
## 規劃
> ***Plans are worthless, but planning is everything.***
> <p align="right">Eisenhower</p>

首先延續在Lite版本中Modeling階段的結果，為Domain Model中的Aggregate加入Event Storming裡Domain Event的便利貼元素，如下所示：

![domain model](./images/domain_model.png)

接著將Domain Model的Method與Domain Event，照著DST中的Sequence Number，排列成Event Storming的形式，如下所示：

![es](./images/es.png)

從左到右是時間流(Time Flow)的方向，且分成上下兩個區塊，上面是Command、下面是Query。
#### Domain Service
在Review的過程中，可能會發現有些Command Block不是很應景。

![command_block](./images/command_block.png)

以上圖為例，推播登記通常不會只包含一個Device，Notification Registered Domain Event後續Policy衍伸的Attach Notification Command可以收攏成Bulk的命令形式，操作多個Device Aggregate進行Attach Notification的動作，可以藉由Domain Service來協助達成。

![command_block2](./images/command_block2.png)

這邊會發現在Event Storming的藍圖中，Notification Bulk Attached Domain Event取代了Notification Attached Domain Event，但在Domain Model中只有Notification Attached Domain Event的規劃。
所以可以區分一下，Notification Attached Domain Event會由Aggregate生成，Notification Bulk Attached Domain Event會由Domain Service生成，而實作上可以將Notification Attached Domain Event放進Notification Bulk Attached Domain Event當中。

![domain service](./images/domain_service.png)
#### Application Service

接著可能會發現有些Command Block可能會涉及到外部的IO來協助。

![command_block3](./images/command_block3.png)

以上圖為例，Push Notification的動作會需要FCM/APNs等外部的資源來協助進行推播的發送服務，可以加入對應的Application Service來表示。
**注意，實作上Domain Event依舊是透過Aggregate來產生。**

![command_block4](./images/command_block4.png)

整理過後會如下圖所示：

![es2](./images/es2.png)