# C#-net-core-api-starter

## 基本說明

- 樣板使用時，請先建立本地端的 env.json
- 資料庫 ： Sqlserver
- 連線字串位置 ： env.json > Database > ConnectionString

## env.json的相關設定

```json
{
  "Author": { 
    "Issuer": "發行權證的簽章",
    "ExpireMinutes": "每次登入的效期(單位:分)",
    "RefreshExpireDays": "RefreshToken的效期(單位:天)"
  },
  "Database": {
    "ConnectionString": "使用的連線字串"
  },
  "Crypto": {
    "IV": "[必填]:密碼加解密所用的IV , 32碼的HexString",
    "Salt": "[必填]:密碼加解密所用的Salt , 無長度限制"
  }
}
```

## 實作功能

|   功能項   |                                      機制來源 |        額外說明        |     文件參考        |
| :--------: | --------------------------------------------: | :--------------------: | :-----------------: |
|  登入認證  | Microsoft.AspNetCore.Authentication.JwtBearer |                        | [Docs][jwtbearer]   |
| 資料庫相關 |       Microsoft.EntityFrameworkCore.SqlServer | CodeFirst & Migrations |  [Docs][efcore]     |
|  Logging   |                                          Nlog |                        |   [Docs][nlog]      |
|  OpenAPI   |                                   Swashbuckle |                        | [Docs][swashbuckle] |

## EF Core Migrations 操作說明

### 注意事項 : 

- 以下操作皆在套件管理器主控台(NuGet PowerShell)中執行

### 執行指令前需知

- 安裝EF Migrations 指令包(若跑過可忽略)

``` code
PM > dotnet tool install --global dotnet-ef
```
- 若出現tool版本過低的提示請執行

``` code
PM > dotnet tool update --global dotnet-ef
```

- **讓套件管理器主控台(NuGet PowerShell)先進入framework的目錄**

``` code
PM > cd C:\your\project\root\to\NetCoreApi.Infrastructure
```

<br>

### 指令

#### 當有發生需要資料庫調整、異動等情況時
``` code
PM > dotnet ef migrations add [keyword]
```
<br>

#### 手動執行 Migrations
``` code
PM > dotnet ef database update
```

## 第三方套件

| Package                                       |                                  Usage | Version | Link                        |
| --------------------------------------------- | -------------------------------------: | ------: | --------------------------- |
| Microsoft.EntityFrameworkCore.SqlServer       |              EntityFramework SqlServer |   5.0.6 | [Docs][efcore]              |
| Microsoft.EntityFrameworkCore.Design          | EntityFramework CodeFirst & Migrations |   5.0.6 | [Docs][efcore.migration]    |
| Microsoft.AspNetCore.Authentication.JwtBearer |                               Jwt 驗證 |   5.0.6 | [Docs][jwtbearer]           |
| Nlog                                          |                               紀錄文件 |  4.7.10 | [Docs][nlog]                |
| NLog.Web.AspNetCore                           |                    Nlog 與 AspNet 整合 |  4.12.0 | [Docs][nlog.web]            |
| Swashbuckle.AspNetCore                        |                      Swagger Supported |   6.1.4 | [Docs][swashbuckle]         |
| Swashbuckle.AspNetCore.Filters                |           Swagger 擴充(權限、過濾 etc) |   7.0.2 | [Docs][swashbuckle.filters] |

[//]: # "reference links"
[nlog]: https://github.com/nlog/nlog/wiki
[nlog.web]: https://github.com/NLog/NLog.Web
[efcore]: https://docs.microsoft.com/zh-tw/ef/core/
[efcore.migration]: https://docs.microsoft.com/zh-tw/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
[jwtbearer]: https://docs.microsoft.com/zh-tw/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-5.0
[swashbuckle]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
[swashbuckle.filters]: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters
