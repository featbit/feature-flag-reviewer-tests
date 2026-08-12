# SnapshotScanner 人工精度验收样本

这个仓库用于让人直接核对 SnapshotScanner 的扫描结果是否符合源码事实。它没有自动断言项目，也不会替审阅者给出“通过”或“不通过”的结论。

当前提供三个递进的扫描基线：

| 基线 | 扫描入口 | 验证重点 | 人读报告 |
|---|---|---|---|
| Base v1 | [`SnapshotBase.slnx`](SnapshotBase.slnx) | Direct + 参数化、多层、跨 Project Wrapper | [`SNAPSHOT_BASE_V1_REVIEW.md`](reports/SNAPSHOT_BASE_V1_REVIEW.md) |
| Base v2 | [`SnapshotBaseV2.slnx`](SnapshotBaseV2.slnx) | Base v1 + Key/default 输入值流 + Evaluation 结果控制业务代码 | [`SNAPSHOT_BASE_V2_REVIEW.md`](reports/SNAPSHOT_BASE_V2_REVIEW.md) |
| Base v3 | [`SnapshotBaseV3.slnx`](SnapshotBaseV3.slnx) | Base v2 + Interface / Virtual / Factory / Delegate / Lambda 静态分派 | [`SNAPSHOT_BASE_V3_REVIEW.md`](reports/SNAPSHOT_BASE_V3_REVIEW.md) |

建议先阅读本 README 中对应基线的场景，再逐个 Feature Flag 阅读 Markdown 报告。只有需要核对稳定 ID、完整 Graph 或 Unresolved 时，才查看 JSON。

## 工程布局

| Project | Base v1 | Base v2 | Base v3 | 作用 |
|---|:---:|:---:|:---:|---|
| `SnapshotScan.Flags` | ✓ | ✓ | ✓ | 引用真实 OpenFeature 2.14.0，包含 Direct Evaluation、真实 Sink 和参数化 Wrapper |
| `SnapshotScan.App` | ✓ | ✓ | ✓ | 跨 Project 调用 Wrapper，并包含一个同名 API 零误报样本 |
| `SnapshotScan.ControlFlow` | — | ✓ | ✓ | 包含 Key/default 条件选择、Evaluation 返回值接收和受控业务代码 |
| `SnapshotScan.Dispatch` | — | — | ✓ | 包含接口、抽象/虚方法、Factory、方法组、Lambda、Delegate 参数及开放分派边界 |

Scanner 只做静态分析，不运行这些业务方法。报告中的“受控代码”表示该代码块在静态控制依赖上受到 Flag 结果影响，不表示某次真实运行一定执行了它。

[`Directory.Build.props`](Directory.Build.props) 关闭了生成 `AssemblyInformationalVersion` 时的 Git Commit 注入。否则本仓库每次提交后，`obj/**/AssemblyInfo.cs` 的内容 Hash 都会变化，即使所有业务源码和扫描结论完全相同；这项设置只稳定生成文件 Inventory，不改变业务代码语义。

## Base v1：Direct + 参数化 Wrapper

Base v1 应找到 **5 个 Key 可以静态确定的 Feature Flag**：

| Key | 类型 | 默认值 | 源码场景 | 预期 Reference |
|---|---|---|---|---|
| `acceptance.direct.boolean` | Boolean | `false` | `const` Key，直接调用 OpenFeature | 1 Direct / Definite |
| `acceptance.direct.string` | String | `"control"` | 字面量 Key，直接调用 String API | 1 Direct / Definite |
| `acceptance.checkout.v2` | Boolean | `false` | 两个调用点复用参数化 Wrapper，Interpolation 构造 Key | 2 Indirect / Definite |
| `acceptance.search.v3` | Boolean | `true` | 相同 Wrapper 的另一组参数，验证调用方隔离 | 1 Indirect / Definite |
| `acceptance.wrapper.fixed` | Boolean | `true` | Wrapper 内固定 Key，再由 App 跨 Project 调用 | 2 Indirect / Definite |

当前 Base v1 报告应为：5 个确定 Key、8 个 Flag 实体、10 条 Reference（4 Direct / 6 Indirect）、9 个 Unresolved。多出的 3 个 `key = null` 实体是未绑定的表达式身份，不是业务 Feature Flag。

### Direct Evaluation

[`DirectEvaluations.cs`](src/SnapshotScan.Flags/DirectEvaluations.cs) 中直接调用真实 `IFeatureClient`：

```csharp
private const string BooleanKey = "acceptance.direct.boolean";

var enabled = await client.GetBooleanValueAsync(BooleanKey, false);
var variant = await client.GetStringValueAsync(
    "acceptance.direct.string",
    "control");
```

人工检查重点：

- Boolean 和 String 类型不能混淆；
- `const` 与字面量 Key 都应解析为常量；
- 两条 Reference 都应为 Direct，并连接真实 OpenFeature Symbol。

### 参数化、多层、跨 Project Wrapper

App 调用点位于 [`BaseEntryPoints.cs`](src/SnapshotScan.App/BaseEntryPoints.cs)：

```csharp
public Task<bool> CheckoutAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);

public Task<bool> CheckoutAgainAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);

public Task<bool> SearchAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.search", 3, true);
```

Wrapper 位于 [`ParameterizedFlagWrappers.cs`](src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs)：

```csharp
public Task<bool> EvaluateVersionAsync(
    string area,
    int version,
    bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

Gateway 最终连接真实 OpenFeature Sink：

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

人工检查重点：

- `checkout + 2` 必须还原成 `acceptance.checkout.v2`；
- `search + 3` 必须还原成 `acceptance.search.v3`；
- Key 和默认值必须一起传播；
- 两个 checkout 调用点应成为两条 Reference，但归属于同一个 Flag；
- search 的 `true` 默认值不能被 checkout 的 `false` 污染；
- 每条间接 Reference 都应沿 Wrapper 链到达真实 Sink。

## Base v2：Control Flow + Value Source

Base v2 是 Base v1 的增量，增加 [`SnapshotScan.ControlFlow`](src/SnapshotScan.ControlFlow)。它应找到 **7 个确定 Key**：Base v1 的 5 个，加上：

| Key | 类型 | 默认值 | 新增值流 |
|---|---|---|---|
| `acceptance.control.beta` | Boolean | `false` | `if` true 分支同时选择 Key/default |
| `acceptance.control.stable` | Boolean | `true` | 同一个 `if/else` 的 false 分支选择另一组 Key/default |

当前 Base v2 报告应为：7 个确定 Key、10 个 Flag 实体、19 条 Reference（4 Direct / 15 Indirect）、9 个 Unresolved，以及 5 个“Evaluation 结果控制条件”节点。

Base v2 同时验证两个方向：

```text
输入侧：运行时条件 -> Key/default 候选 -> Evaluation

输出侧：Evaluation -> Wrapper 返回值 -> 调用方变量
       -> if/else 条件 -> 受控业务代码块
```

### 场景 1：条件同时选择 Key 和默认值

[`ConditionalEntryPoints.cs`](src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs) 中只有两组合法配对：

```csharp
public Task<bool> SelectKeyAndDefaultAsync(bool useBeta)
{
    string key;
    bool fallback;
    if (useBeta)
    {
        key = "acceptance.control.beta";
        fallback = false;
    }
    else
    {
        key = "acceptance.control.stable";
        fallback = true;
    }

    return gateway.EvaluateAsync(key, fallback);
}
```

报告中只能出现：

```text
acceptance.control.beta   + false + when true
acceptance.control.stable + true  + when false
```

不能出现 `beta + true` 或 `stable + false`。两条候选都是 `Possible`，并应展示“决定本次 Key / 默认值候选的控制条件”。

### 场景 2：Evaluation 直接写在 `if` 条件中

```csharp
public async Task<string> UseFixedFlagInControlFlowAsync()
{
    if (await wrapper.EvaluateFixedAsync())
    {
        return "new-checkout";
    }

    return "legacy-checkout";
}
```

报告应把完整 `if` 块展示为 `acceptance.wrapper.fixed` 的受控业务代码，而不是只展示调用表达式。

### 场景 3：返回值赋给局部变量，稍后再使用

[`ResultControlEntryPoints.cs`](src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs) 先调用 Base v1 的 `SearchAsync()`，再经过无关赋值后使用结果：

```csharp
var searchV3Enabled = await baseEntryPoints.SearchAsync();
var selectedPipeline = "legacy-search";

if (searchV3Enabled)
{
    selectedPipeline = "search-v3";
}
```

`acceptance.search.v3` 的报告应单独展示：

1. `SearchAsync()` 到真实 Sink 的 Wrapper 链；
2. `var searchV3Enabled = ...` 返回值接收语句；
3. `if (searchV3Enabled)` 及其中的业务代码。

### 场景 4：输入侧选择与输出侧控制同时存在

```csharp
var selectedFlagEnabled =
    await conditionalEntryPoints.SelectKeyAndDefaultAsync(useBeta);

if (!selectedFlagEnabled)
{
    return "selection-disabled";
}
```

同一个调用会展开成 `acceptance.control.beta` 和 `acceptance.control.stable` 两条相关候选。两条报告都必须同时保留：

- 各自正确的 Key/default 选择分支；
- `selectedFlagEnabled` 局部变量；
- 否定条件 `if (!selectedFlagEnabled)` 的受控代码。

### 场景 5：局部变量控制完整 `if/else`

```csharp
var fixedFlagEnabled = await baseEntryPoints.FixedAsync();

if (fixedFlagEnabled)
{
    return "fixed-enabled";
}
else
{
    return "fixed-disabled";
}
```

报告应展示完整 `if/else`，并把它关联到 `acceptance.wrapper.fixed`。

### 场景 6：结果跨额外方法返回后再控制代码

```csharp
public Task<bool> IsCheckoutV2EnabledAsync() =>
    baseEntryPoints.CheckoutAsync();

var checkoutV2Enabled = await decisions.IsCheckoutV2EnabledAsync();

if (!checkoutV2Enabled)
{
    return "legacy-checkout";
}
```

`acceptance.checkout.v2` 的调用链应包含：

```text
CheckoutEndpoint.ExecuteAsync
  -> CheckoutDecisionService.IsCheckoutV2EnabledAsync
  -> BaseEntryPoints.CheckoutAsync
  -> ParameterizedFlagWrapper.EvaluateVersionAsync
  -> OpenFeatureBooleanGateway.EvaluateAsync
  -> IFeatureClient.GetBooleanValueAsync
```

随后报告还应展示 `checkoutV2Enabled` 的接收语句和提前返回代码块。

## Base v3：Interface / Virtual / Delegate Dispatch

Base v3 是 Base v2 的增量，增加 [`SnapshotScan.Dispatch`](src/SnapshotScan.Dispatch)。它应找到 **17 个 Key 可以静态确定的 Feature Flag**：Base v2 的 7 个，加上下面 10 个。

| Key | 默认值 | 分派场景 | 预期结论 |
|---|---|---|---|
| `acceptance.dispatch.interface` | `false` | 局部变量静态绑定到 `Alpha` 后经接口调用；另有一个调用方消费返回值 | 2 Indirect / Definite；目标只有 Alpha |
| `acceptance.dispatch.virtual` | `true` | 抽象基类变量静态绑定到 `Beta`，调用 Override | 1 Indirect / Definite；目标只有 Beta |
| `acceptance.dispatch.factory` | `false` | Factory 根据条件返回 Alpha 或 Beta | 1 Indirect / Possible；完整展示两个候选 |
| `acceptance.dispatch.method-group` | `false` | 实例方法组赋给 `Func<...>` 后 Invoke | 1 Indirect / Definite |
| `acceptance.dispatch.lambda` | `true` | Lambda 内调用真实 Gateway | 1 Indirect / Definite |
| `acceptance.dispatch.parameter` | `false` | 方法组经 Delegate 参数跨方法传递 | 1 Indirect / Definite |
| `acceptance.dispatch.delegate-factory` | `true` | Factory 返回方法组或 Lambda | 1 Indirect / Possible；完整展示两个候选 |
| `acceptance.dispatch.open-interface` | `false` | 公共接口参数可能来自仓库外 | 1 Indirect / Possible + `INCOMPLETE_DISPATCH_CANDIDATES` |
| `acceptance.dispatch.open-delegate` | `false` | Delegate 一支已知、一支由调用方传入 | 1 Indirect / Possible + `UNKNOWN_DELEGATE_TARGET` |
| `acceptance.dispatch.unknown-factory` | `false` | Factory 结果无法从源码还原 | 1 Indirect / Possible + `UNKNOWN_FACTORY_RESULT` |

当前 Base v3 报告应为：17 个确定 Key、23 个 Flag 实体、33 条 Reference（4 Direct / 29 Indirect；18 Definite / 9 Possible / 6 Unresolved）、199 Nodes、338 Edges、27 个 Unresolved、0 Diagnostics，以及 6 个“Evaluation 结果控制条件”节点。多出的 6 个 `key = null` 实体是定义级未绑定或未知表达式身份，不是新的业务 Feature Flag。

### 场景 1：已知 Interface 与 Virtual Override

[`DispatchStrategies.cs`](src/SnapshotScan.Dispatch/DispatchStrategies.cs) 定义一个接口、一个抽象基类和两个 Sealed 实现；两个实现最终都调用 Base v1 的真实 Gateway：

```csharp
public interface IDispatchFlagStrategy
{
    Task<bool> EvaluateAsync(string key, bool fallback);
}

public abstract class DispatchFlagStrategy
{
    public abstract Task<bool> EvaluateAsync(string key, bool fallback);
}

public sealed class AlphaDispatchFlagStrategy
    : DispatchFlagStrategy, IDispatchFlagStrategy
{
    public override Task<bool> EvaluateAsync(string key, bool fallback) =>
        Gateway.EvaluateAsync(key, fallback);
}
```

[`DispatchEntryPoints.cs`](src/SnapshotScan.Dispatch/DispatchEntryPoints.cs) 用具体对象约束运行时接收者：

```csharp
IDispatchFlagStrategy strategy = new AlphaDispatchFlagStrategy(gateway);
return strategy.EvaluateAsync("acceptance.dispatch.interface", false);

DispatchFlagStrategy strategy = new BetaDispatchFlagStrategy(gateway);
return strategy.EvaluateAsync("acceptance.dispatch.virtual", true);
```

人工检查报告中的“静态分派目标”：Interface 只能指向 Alpha，Virtual Override 只能指向 Beta；两者都应为 `DEFINITE`，并继续沿 Gateway 到达真实 `IFeatureClient.GetBooleanValueAsync`。

### 场景 2：有限 Factory 必须保留所有候选

```csharp
private IDispatchFlagStrategy CreateStrategy(bool useAlpha) =>
    useAlpha
        ? new AlphaDispatchFlagStrategy(gateway)
        : new BetaDispatchFlagStrategy(gateway);

var strategy = CreateStrategy(useAlpha);
return strategy.EvaluateAsync("acceptance.dispatch.factory", false);
```

运行时条件未知，但候选集合是完整且有限的。因此报告应生成一个 `POSSIBLE` Reference，并在同一条 Reference 的“静态分派目标”下分别展示 Alpha 与 Beta 两个独立源码块。不能只展示代表路径中的 Alpha，也不能把它提升为 `DEFINITE`。

### 场景 3：Method Group、Lambda、Delegate 参数和 Delegate Factory

Base v3 分别验证四种 Delegate 值来源：

```csharp
// Method Group
Func<string, bool, Task<bool>> callback =
    new AlphaDispatchFlagStrategy(gateway).EvaluateAsync;

// Lambda
Func<string, bool, Task<bool>> callback =
    (key, fallback) => gateway.EvaluateAsync(key, fallback);

// 跨方法 Delegate 参数
InvokeAsync(
    new BetaDispatchFlagStrategy(gateway).EvaluateAsync,
    "acceptance.dispatch.parameter",
    false);

// Delegate Factory：方法组或 Lambda
CreateDelegate(useMethodGroup)(
    "acceptance.dispatch.delegate-factory",
    true);
```

前三种目标唯一，应为 `DEFINITE`。Delegate Factory 的两种目标都可枚举，但运行时分支未知，因此应为 `POSSIBLE`，并分别展示 Method Group 目标与 Lambda 源码。Scanner 必须依据 Roslyn Symbol、Delegate 赋值和调用关系识别，不能依据方法名猜测。

### 场景 4：分派返回值继续控制业务代码

`acceptance.dispatch.interface` 还有第二条 Reference，专门组合 Base v2 的输出值流与 Base v3 的接口分派：

```csharp
var interfaceEnabled = await KnownInterfaceAsync();

if (!interfaceEnabled)
{
    return "legacy-interface";
}
```

该 Reference 的报告应同时展示：

1. `UseKnownInterfaceResultAsync -> KnownInterfaceAsync` 的调用入口；
2. Interface 调用及唯一的 Alpha 实现；
3. Gateway 与真实 OpenFeature Sink；
4. `interfaceEnabled` 返回值接收语句；
5. `if (!interfaceEnabled)` 的完整受控业务代码。

只有返回表达式中的调用身份与已解析的 Evaluation 调用路径精确一致时，Scanner 才能建立这条结果关系。它不能因为某个 Evaluation 只是作为另一个方法的参数出现，就声称后者的返回值来自该 Flag。

### 场景 5：开放分派必须保留未知边界

三个场景故意让静态候选集不完整：

```csharp
// 调用方仍可传入仓库外的接口实现
strategy.EvaluateAsync("acceptance.dispatch.open-interface", false);

// 一支是已知方法组，另一支来自外部 Delegate 参数
CreateOpenDelegate(useKnown, externalCallback)(
    "acceptance.dispatch.open-delegate",
    false);

// Factory 方法体不能还原为具体 object creation
LoadExternalStrategy().EvaluateAsync(
    "acceptance.dispatch.unknown-factory",
    false);
```

报告可以展示源码中已知的 Alpha/Beta 目标，但这些 Reference 必须保持 `POSSIBLE`，并分别关联 `INCOMPLETE_DISPATCH_CANDIDATES`、`UNKNOWN_DELEGATE_TARGET` 或 `UNKNOWN_FACTORY_RESULT`。已知候选只是“当前源码内可能到达的路径”，不代表 Scanner 已证明运行时目标完整。

## 两个负例边界

### 动态 Key：必须 Unresolved

```csharp
public Task<bool> EvaluateDynamicAsync(string tenant)
{
    var key = $"acceptance.dynamic.{tenant}";
    return client.GetBooleanValueAsync(key, true);
}
```

Scanner 不得猜测租户值。正确结果是保留表达式、默认值 `true` 和明确的 Key Unresolved。

### 同名业务 API：必须零误报

[`SameNameBusinessClient.cs`](src/SnapshotScan.App/SameNameBusinessClient.cs) 定义了业务类型自己的 `GetBooleanValueAsync`：

```csharp
client.GetBooleanValueAsync("acceptance.false-positive", false);
```

`acceptance.false-positive` 不应出现在 Flags、References 或 Evidence 中，因为它不是 OpenFeature Symbol。

## 如何阅读人读报告

对每个 Key 按下面顺序检查：

1. 顶部 Feature Flag 列表中的 Key、类型、默认值候选和 Reference 数量。
2. 每条 Reference 的调用位置和 `Direct / Indirect`。
3. “调用、Wrapper 与真实 Evaluation”是否从入口一直连接到真实 OpenFeature API。
4. Base v2 的“决定本次 Key / 默认值候选的控制条件”是否保持正确配对。
5. Base v2 的“Evaluation 返回值流向与受控业务代码”是否分别展示返回值接收语句和完整控制块。
6. Base v3 的“静态分派目标”是否列出全部有限候选，每个候选是否有自己的源码块与正确确定性。
7. Base v3 的开放 Interface / Delegate / Factory 是否保持 `Possible` 并关联明确 Unresolved，而不是伪装成完整结果。
8. 最后的 Unresolved 是否只对应动态值、定义级未绑定参数或明确分析边界。

JSON 中，输出侧证据使用现有 Graph 语义表达：

```text
FlagEvaluation --RETURNS--> ValueSource(局部变量)
ValueSource    --RETURNS--> ControlCondition
ControlCondition --GUARDS--> 所在业务成员
```

源码片段不会写入 JSON。`render` 会从显式 Workspace 只读加载源码，先核对报告里的 `contentHash`，再生成 Markdown。

## 生成报告

从本仓库执行 Base v1：

```powershell
.\scripts\Scan-SnapshotBase.ps1 `
  -BaseVersion V1 `
  -ReviewerRoot C:\Code\featbit\featbit-demo\feature-flag-reviewer `
  -Output .\reports\snapshot-base-v1-candidate.json `
  -ReviewOutput .\reports\SNAPSHOT_BASE_V1_CANDIDATE_REVIEW.md
```

执行 Base v2：

```powershell
.\scripts\Scan-SnapshotBase.ps1 `
  -BaseVersion V2 `
  -ReviewerRoot C:\Code\featbit\featbit-demo\feature-flag-reviewer `
  -Output .\reports\snapshot-base-v2-candidate.json `
  -ReviewOutput .\reports\SNAPSHOT_BASE_V2_CANDIDATE_REVIEW.md
```

执行 Base v3：

```powershell
.\scripts\Scan-SnapshotBase.ps1 `
  -BaseVersion V3 `
  -ReviewerRoot C:\Code\featbit\featbit-demo\feature-flag-reviewer `
  -Output .\reports\snapshot-base-v3-candidate.json `
  -ReviewOutput .\reports\SNAPSHOT_BASE_V3_CANDIDATE_REVIEW.md
```

脚本会 Restore/Build 两边的 Solution，然后调用 `featbit-demo/feature-flag-reviewer` 编译出的真实 CLI 执行 `scan` 和 `render`。它会打印 JSON 与 Markdown 的 SHA-256，但不会比较 Golden，也不会产生精度断言。

Git Base/Head 物化和 Report Differ 编排不属于当前人工精度验收范围。
