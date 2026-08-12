# SnapshotScanner 人工精度验收样本

这个仓库用于让人直接核对 SnapshotScanner 的扫描结果是否符合源码事实。它没有自动断言项目，也不会替审阅者给出“通过”或“不通过”的结论。

当前提供两个递进的扫描基线：

| 基线 | 扫描入口 | 验证重点 | 人读报告 |
|---|---|---|---|
| Base v1 | [`SnapshotBase.slnx`](SnapshotBase.slnx) | Direct + 参数化、多层、跨 Project Wrapper | [`SNAPSHOT_BASE_V1_REVIEW.md`](reports/SNAPSHOT_BASE_V1_REVIEW.md) |
| Base v2 | [`SnapshotBaseV2.slnx`](SnapshotBaseV2.slnx) | Base v1 + Key/default 输入值流 + Evaluation 结果控制业务代码 | [`SNAPSHOT_BASE_V2_REVIEW.md`](reports/SNAPSHOT_BASE_V2_REVIEW.md) |

建议先阅读本 README 中对应基线的场景，再逐个 Feature Flag 阅读 Markdown 报告。只有需要核对稳定 ID、完整 Graph 或 Unresolved 时，才查看 JSON。

## 工程布局

| Project | Base v1 | Base v2 | 作用 |
|---|:---:|:---:|---|
| `SnapshotScan.Flags` | ✓ | ✓ | 引用真实 OpenFeature 2.14.0，包含 Direct Evaluation、真实 Sink 和参数化 Wrapper |
| `SnapshotScan.App` | ✓ | ✓ | 跨 Project 调用 Wrapper，并包含一个同名 API 零误报样本 |
| `SnapshotScan.ControlFlow` | — | ✓ | 包含 Key/default 条件选择、Evaluation 返回值接收和受控业务代码 |

Scanner 只做静态分析，不运行这些业务方法。报告中的“受控代码”表示该代码块在静态控制依赖上受到 Flag 结果影响，不表示某次真实运行一定执行了它。

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
6. 最后的 Unresolved 是否只对应动态值、定义级未绑定参数或明确分析边界。

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

脚本会 Restore/Build 两边的 Solution，然后调用 `featbit-demo/feature-flag-reviewer` 编译出的真实 CLI 执行 `scan` 和 `render`。它会打印 JSON 与 Markdown 的 SHA-256，但不会比较 Golden，也不会产生精度断言。

Git Base/Head 物化和 Report Differ 编排不属于当前人工精度验收范围。
