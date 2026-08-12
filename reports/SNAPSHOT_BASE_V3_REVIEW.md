# Snapshot Scan 人工审阅报告

> 本报告由 `snapshot-report.json` 和对应 Workspace 源码确定性生成。所有展示的本地源码都已与报告中的 SHA-256 `contentHash` 核对；本报告不替人工给出通过/失败结论。

## 扫描概览

- Report 状态：`ANALYZED_WITH_UNRESOLVED`
- Schema / Scanner：`1.5` / `0.3.0`
- Workspace 入口：`SnapshotBaseV3.slnx`
- 已加载 Project：4 / 4
- 已分析文件：17
- 确定 Key 的 Feature Flag：17
- Key 尚未确定的表达式实体：6
- Reference：33（Direct 4 / Indirect 29）
- Unresolved / Diagnostic：27 / 0

JSON 对应关系：Feature Flag 列表来自 `flags`；逐条引用来自 `references`；调用链、Wrapper、输入值选择、Evaluation 返回值流向、受控代码和 Sink 来自 `graphNodes` / `graphEdges`；不能证明的部分来自 `unresolved`。

## Feature Flag 列表

| Key | 类型 / API | 默认值候选 | Reference | 证据结论 |
|---|---|---|---:|---|
| `acceptance.checkout.v2` | BOOLEAN/VALUE | false | 4 | DEFINITE |
| `acceptance.control.beta` | BOOLEAN/VALUE | false | 2 | POSSIBLE |
| `acceptance.control.stable` | BOOLEAN/VALUE | true | 2 | POSSIBLE |
| `acceptance.direct.boolean` | BOOLEAN/VALUE | false | 1 | DEFINITE |
| `acceptance.direct.string` | STRING/VALUE | control | 1 | DEFINITE |
| `acceptance.dispatch.delegate-factory` | BOOLEAN/VALUE | true | 1 | POSSIBLE |
| `acceptance.dispatch.factory` | BOOLEAN/VALUE | false | 1 | POSSIBLE |
| `acceptance.dispatch.interface` | BOOLEAN/VALUE | false | 2 | DEFINITE |
| `acceptance.dispatch.lambda` | BOOLEAN/VALUE | true | 1 | DEFINITE |
| `acceptance.dispatch.method-group` | BOOLEAN/VALUE | false | 1 | DEFINITE |
| `acceptance.dispatch.open-delegate` | BOOLEAN/VALUE | false | 1 | POSSIBLE |
| `acceptance.dispatch.open-interface` | BOOLEAN/VALUE | false | 1 | POSSIBLE |
| `acceptance.dispatch.parameter` | BOOLEAN/VALUE | false | 1 | DEFINITE |
| `acceptance.dispatch.unknown-factory` | BOOLEAN/VALUE | false | 1 | POSSIBLE |
| `acceptance.dispatch.virtual` | BOOLEAN/VALUE | true | 1 | DEFINITE |
| `acceptance.search.v3` | BOOLEAN/VALUE | true | 2 | DEFINITE |
| `acceptance.wrapper.fixed` | BOOLEAN/VALUE | true | 4 | DEFINITE |

## `acceptance.checkout.v2`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：4 条（Direct 0 / Indirect 4）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.App/BaseEntryPoints.cs:11`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L11>)
- 所在 Symbol：`SnapshotScan.App.BaseEntryPoints.CheckoutAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.checkout.v2` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.App.BaseEntryPoints.CheckoutAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:10`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L10-L11>)

```csharp
public Task<bool> CheckoutAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateVersionAsync(string, int, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:23`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L23-L27>)

```csharp
public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

### Reference 2 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.App/BaseEntryPoints.cs:14`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L14>)
- 所在 Symbol：`SnapshotScan.App.BaseEntryPoints.CheckoutAgainAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.checkout.v2` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.App.BaseEntryPoints.CheckoutAgainAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:13`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L13-L14>)

```csharp
public Task<bool> CheckoutAgainAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateVersionAsync(string, int, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:23`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L23-L27>)

```csharp
public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

### Reference 3 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:63`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L63>)
- 所在 Symbol：`SnapshotScan.ControlFlow.CheckoutDecisionService.IsCheckoutV2EnabledAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.checkout.v2` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.CheckoutDecisionService.IsCheckoutV2EnabledAsync()`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:62`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L62-L63>)

```csharp
public Task<bool> IsCheckoutV2EnabledAsync() =>
    baseEntryPoints.CheckoutAsync();
```

##### Wrapper — `SnapshotScan.App.BaseEntryPoints.CheckoutAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:10`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L10-L11>)

```csharp
public Task<bool> CheckoutAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateVersionAsync(string, int, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:23`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L23-L27>)

```csharp
public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

### Reference 4 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:74`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L74>)
- 所在 Symbol：`SnapshotScan.ControlFlow.CheckoutEndpoint.ExecuteAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.checkout.v2` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.CheckoutEndpoint.ExecuteAsync()`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:72`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L72-L82>)

```csharp
public async Task<string> ExecuteAsync()
{
    var checkoutV2Enabled = await decisions.IsCheckoutV2EnabledAsync();

    if (!checkoutV2Enabled)
    {
        return "legacy-checkout";
    }

    return "checkout-v2";
}
```

##### Wrapper — `SnapshotScan.ControlFlow.CheckoutDecisionService.IsCheckoutV2EnabledAsync()`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:62`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L62-L63>)

```csharp
public Task<bool> IsCheckoutV2EnabledAsync() =>
    baseEntryPoints.CheckoutAsync();
```

##### Wrapper — `SnapshotScan.App.BaseEntryPoints.CheckoutAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:10`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L10-L11>)

```csharp
public Task<bool> CheckoutAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.checkout", 2, false);
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateVersionAsync(string, int, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:23`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L23-L27>)

```csharp
public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `checkoutV2Enabled: bool`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:74`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L74>)

```csharp
var checkoutV2Enabled = await decisions.IsCheckoutV2EnabledAsync();
```

##### 受返回值控制的代码

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:76`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L76-L79>)

```csharp
if (!checkoutV2Enabled)
{
    return "legacy-checkout";
}
```

## `acceptance.control.beta`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：2 条（Direct 0 / Indirect 2）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:29`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L29>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ConditionalEntryPoints.SelectKeyAndDefaultAsync(bool)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.control.beta` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ConditionalEntryPoints.SelectKeyAndDefaultAsync(bool)`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:14`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L14-L30>)

```csharp
// One branch chooses both values. The only valid pairs are beta/false and stable/true.
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

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 决定本次 Key / 默认值候选的控制条件

##### 值选择分支 — `when true`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:18`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L18-L27>)

```csharp
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
```

### Reference 2 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:31`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L31>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ResultControlEntryPoints.UseSelectedResultAsync(bool)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.control.beta` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ResultControlEntryPoints.UseSelectedResultAsync(bool)`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:29`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L29-L39>)

```csharp
// The same runtime branch that selects the key/default pair is passed into the
// evaluation. Its returned value is then stored and used by a negated guard.
public async Task<string> UseSelectedResultAsync(bool useBeta)
{
    var selectedFlagEnabled = await conditionalEntryPoints.SelectKeyAndDefaultAsync(useBeta);

    if (!selectedFlagEnabled)
    {
        return "selection-disabled";
    }

    return "selection-enabled";
}
```

##### Wrapper — `SnapshotScan.ControlFlow.ConditionalEntryPoints.SelectKeyAndDefaultAsync(bool)`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:14`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L14-L30>)

```csharp
// One branch chooses both values. The only valid pairs are beta/false and stable/true.
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

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 决定本次 Key / 默认值候选的控制条件

##### 值选择分支 — `when true`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:18`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L18-L27>)

```csharp
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
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `selectedFlagEnabled: bool`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:31`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L31>)

```csharp
var selectedFlagEnabled = await conditionalEntryPoints.SelectKeyAndDefaultAsync(useBeta);
```

##### 受返回值控制的代码

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:33`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L33-L36>)

```csharp
if (!selectedFlagEnabled)
{
    return "selection-disabled";
}
```

## `acceptance.control.stable`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：2 条（Direct 0 / Indirect 2）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:29`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L29>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ConditionalEntryPoints.SelectKeyAndDefaultAsync(bool)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.control.stable` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ConditionalEntryPoints.SelectKeyAndDefaultAsync(bool)`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:14`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L14-L30>)

```csharp
// One branch chooses both values. The only valid pairs are beta/false and stable/true.
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

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 决定本次 Key / 默认值候选的控制条件

##### 值选择分支 — `when false`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:18`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L18-L27>)

```csharp
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
```

### Reference 2 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:31`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L31>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ResultControlEntryPoints.UseSelectedResultAsync(bool)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.control.stable` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ResultControlEntryPoints.UseSelectedResultAsync(bool)`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:29`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L29-L39>)

```csharp
// The same runtime branch that selects the key/default pair is passed into the
// evaluation. Its returned value is then stored and used by a negated guard.
public async Task<string> UseSelectedResultAsync(bool useBeta)
{
    var selectedFlagEnabled = await conditionalEntryPoints.SelectKeyAndDefaultAsync(useBeta);

    if (!selectedFlagEnabled)
    {
        return "selection-disabled";
    }

    return "selection-enabled";
}
```

##### Wrapper — `SnapshotScan.ControlFlow.ConditionalEntryPoints.SelectKeyAndDefaultAsync(bool)`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:14`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L14-L30>)

```csharp
// One branch chooses both values. The only valid pairs are beta/false and stable/true.
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

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 决定本次 Key / 默认值候选的控制条件

##### 值选择分支 — `when false`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:18`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L18-L27>)

```csharp
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
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `selectedFlagEnabled: bool`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:31`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L31>)

```csharp
var selectedFlagEnabled = await conditionalEntryPoints.SelectKeyAndDefaultAsync(useBeta);
```

##### 受返回值控制的代码

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:33`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L33-L36>)

```csharp
if (!selectedFlagEnabled)
{
    return "selection-disabled";
}
```

## `acceptance.direct.boolean`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 1 / Indirect 0）
- 汇总结论：`DEFINITE`

### Reference 1 — Direct / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Flags/DirectEvaluations.cs:14`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L14>)
- 所在 Symbol：`SnapshotScan.Flags.DirectEvaluations.EvaluateAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.direct.boolean` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### Evaluation 所在代码 — `SnapshotScan.Flags.DirectEvaluations.EvaluateAsync()`

[`src/SnapshotScan.Flags/DirectEvaluations.cs:12`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L12-L17>)

```csharp
public async Task<(bool Enabled, string Variant)> EvaluateAsync()
{
    var enabled = await client.GetBooleanValueAsync(BooleanKey, false);
    var variant = await client.GetStringValueAsync("acceptance.direct.string", "control");
    return (enabled, variant);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/DirectEvaluations.cs:14`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L14>)

```csharp
var enabled = await client.GetBooleanValueAsync(BooleanKey, false);
```

## `acceptance.direct.string`

- 类型：STRING
- API 详情：STRING/VALUE
- 默认值候选：control
- 引用：1 条（Direct 1 / Indirect 0）
- 汇总结论：`DEFINITE`

### Reference 1 — Direct / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Flags/DirectEvaluations.cs:15`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L15>)
- 所在 Symbol：`SnapshotScan.Flags.DirectEvaluations.EvaluateAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetStringValueAsync(System.String,System.String,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（STRING / VALUE）
- Key / 默认值：`acceptance.direct.string` / `control`

#### 调用、Wrapper 与真实 Evaluation

##### Evaluation 所在代码 — `SnapshotScan.Flags.DirectEvaluations.EvaluateAsync()`

[`src/SnapshotScan.Flags/DirectEvaluations.cs:12`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L12-L17>)

```csharp
public async Task<(bool Enabled, string Variant)> EvaluateAsync()
{
    var enabled = await client.GetBooleanValueAsync(BooleanKey, false);
    var variant = await client.GetStringValueAsync("acceptance.direct.string", "control");
    return (enabled, variant);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetStringValueAsync(string, string, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/DirectEvaluations.cs:15`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L15>)

```csharp
var variant = await client.GetStringValueAsync("acceptance.direct.string", "control");
```

## `acceptance.dispatch.delegate-factory`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:63`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L63>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.DelegateFactoryAsync(bool)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.delegate-factory` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.DelegateFactoryAsync(bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:62`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L62-L63>)

```csharp
public Task<bool> DelegateFactoryAsync(bool useMethodGroup) =>
    CreateDelegate(useMethodGroup)("acceptance.dispatch.delegate-factory", true);
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（2 个静态候选） — `System.Func<string, bool, System.Threading.Tasks.Task<bool>>.Invoke(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:62`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L62-L63>)

```csharp
public Task<bool> DelegateFactoryAsync(bool useMethodGroup) =>
    CreateDelegate(useMethodGroup)("acceptance.dispatch.delegate-factory", true);
```

##### Delegate / Lambda 目标 / `POSSIBLE` — `lambda expression`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:52`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L52>)

```csharp
(key, fallback) => gateway.EvaluateAsync(key, fallback)
```

##### Delegate 目标 / `POSSIBLE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

## `acceptance.dispatch.factory`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:39`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L39>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.InterfaceFactoryAsync(bool)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.factory` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.InterfaceFactoryAsync(bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:36`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L36-L40>)

```csharp
public Task<bool> InterfaceFactoryAsync(bool useAlpha)
{
    var strategy = CreateStrategy(useAlpha);
    return strategy.EvaluateAsync("acceptance.dispatch.factory", false);
}
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（2 个静态候选） — `SnapshotScan.Dispatch.IDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:39`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L39>)

```csharp
return strategy.EvaluateAsync("acceptance.dispatch.factory", false);
```

##### Interface 实现目标 / `POSSIBLE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Interface 实现目标 / `POSSIBLE` — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

## `acceptance.dispatch.interface`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：2 条（Direct 0 / Indirect 2）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:14`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L14>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.KnownInterfaceAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.interface` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.KnownInterfaceAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:11`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L11-L15>)

```csharp
public Task<bool> KnownInterfaceAsync()
{
    IDispatchFlagStrategy strategy = new AlphaDispatchFlagStrategy(gateway);
    return strategy.EvaluateAsync("acceptance.dispatch.interface", false);
}
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `SnapshotScan.Dispatch.IDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:14`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L14>)

```csharp
return strategy.EvaluateAsync("acceptance.dispatch.interface", false);
```

##### Interface 实现目标 / `DEFINITE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

### Reference 2 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:20`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L20>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.UseKnownInterfaceResultAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.interface` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.UseKnownInterfaceResultAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:18`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L18-L28>)

```csharp
// Base v2 + v3 composition: a value returned through interface dispatch controls business code.
public async Task<string> UseKnownInterfaceResultAsync()
{
    var interfaceEnabled = await KnownInterfaceAsync();

    if (!interfaceEnabled)
    {
        return "legacy-interface";
    }

    return "dispatched-interface";
}
```

##### Wrapper — `SnapshotScan.Dispatch.DispatchEntryPoints.KnownInterfaceAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:11`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L11-L15>)

```csharp
public Task<bool> KnownInterfaceAsync()
{
    IDispatchFlagStrategy strategy = new AlphaDispatchFlagStrategy(gateway);
    return strategy.EvaluateAsync("acceptance.dispatch.interface", false);
}
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `SnapshotScan.Dispatch.IDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:14`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L14>)

```csharp
return strategy.EvaluateAsync("acceptance.dispatch.interface", false);
```

##### Interface 实现目标 / `DEFINITE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `interfaceEnabled: bool`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:20`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L20>)

```csharp
var interfaceEnabled = await KnownInterfaceAsync();
```

##### 受返回值控制的代码

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:22`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L22-L25>)

```csharp
if (!interfaceEnabled)
{
    return "legacy-interface";
}
```

## `acceptance.dispatch.lambda`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:53`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L53>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.LambdaAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.lambda` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.LambdaAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:49`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L49-L54>)

```csharp
public Task<bool> LambdaAsync()
{
    Func<string, bool, Task<bool>> callback =
        (key, fallback) => gateway.EvaluateAsync(key, fallback);
    return callback("acceptance.dispatch.lambda", true);
}
```

##### Wrapper — `lambda expression`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:52`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L52>)

```csharp
(key, fallback) => gateway.EvaluateAsync(key, fallback)
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `System.Func<string, bool, System.Threading.Tasks.Task<bool>>.Invoke(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:53`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L53>)

```csharp
return callback("acceptance.dispatch.lambda", true);
```

##### Delegate / Lambda 目标 / `DEFINITE` — `lambda expression`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:52`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L52>)

```csharp
(key, fallback) => gateway.EvaluateAsync(key, fallback)
```

## `acceptance.dispatch.method-group`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:46`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L46>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.MethodGroupAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.method-group` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.MethodGroupAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:42`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L42-L47>)

```csharp
public Task<bool> MethodGroupAsync()
{
    Func<string, bool, Task<bool>> callback =
        new AlphaDispatchFlagStrategy(gateway).EvaluateAsync;
    return callback("acceptance.dispatch.method-group", false);
}
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `System.Func<string, bool, System.Threading.Tasks.Task<bool>>.Invoke(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:46`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L46>)

```csharp
return callback("acceptance.dispatch.method-group", false);
```

##### Delegate 目标 / `DEFINITE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

## `acceptance.dispatch.open-delegate`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:75`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L75>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.OpenDelegateAsync(bool, System.Func<string, bool, System.Threading.Tasks.Task<bool>>)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.open-delegate` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.OpenDelegateAsync(bool, System.Func<string, bool, System.Threading.Tasks.Task<bool>>)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:72`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L72-L77>)

```csharp
// Open Delegate boundary: one branch is a verified source target while the other
// comes from an unknown caller. The known reference is Possible, never Definite.
public Task<bool> OpenDelegateAsync(
    bool useKnown,
    Func<string, bool, Task<bool>> externalCallback) =>
    CreateOpenDelegate(useKnown, externalCallback)(
        "acceptance.dispatch.open-delegate",
        false);
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `System.Func<string, bool, System.Threading.Tasks.Task<bool>>.Invoke(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:72`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L72-L77>)

```csharp
// Open Delegate boundary: one branch is a verified source target while the other
// comes from an unknown caller. The known reference is Possible, never Definite.
public Task<bool> OpenDelegateAsync(
    bool useKnown,
    Func<string, bool, Task<bool>> externalCallback) =>
    CreateOpenDelegate(useKnown, externalCallback)(
        "acceptance.dispatch.open-delegate",
        false);
```

##### Delegate 目标 / `POSSIBLE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

#### 本条 Reference 的 Unresolved
- `UNKNOWN_DELEGATE_TARGET` / `CALL`：The Delegate value contains a target that cannot be enumerated from source.

## `acceptance.dispatch.open-interface`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:68`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L68>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.OpenInterfaceAsync(SnapshotScan.Dispatch.IDispatchFlagStrategy)`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.open-interface` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.OpenInterfaceAsync(SnapshotScan.Dispatch.IDispatchFlagStrategy)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:67`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L67-L68>)

```csharp
// The source contains two known implementations, but an interface parameter can still
// receive an external implementation at runtime. Known candidates must remain incomplete.
public Task<bool> OpenInterfaceAsync(IDispatchFlagStrategy strategy) =>
    strategy.EvaluateAsync("acceptance.dispatch.open-interface", false);
```

##### Wrapper — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（2 个静态候选） — `SnapshotScan.Dispatch.IDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:67`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L67-L68>)

```csharp
// The source contains two known implementations, but an interface parameter can still
// receive an external implementation at runtime. Known candidates must remain incomplete.
public Task<bool> OpenInterfaceAsync(IDispatchFlagStrategy strategy) =>
    strategy.EvaluateAsync("acceptance.dispatch.open-interface", false);
```

##### Interface 实现目标 / `POSSIBLE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Interface 实现目标 / `POSSIBLE` — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

#### 本条 Reference 的 Unresolved
- `INCOMPLETE_DISPATCH_CANDIDATES` / `CALL`：Known dispatch targets coexist with possible external runtime targets; the candidate set is incomplete.

## `acceptance.dispatch.parameter`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:57`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L57>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.DelegateParameterAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.parameter` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.DelegateParameterAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:56`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L56-L60>)

```csharp
public Task<bool> DelegateParameterAsync() =>
    InvokeAsync(
        new BetaDispatchFlagStrategy(gateway).EvaluateAsync,
        "acceptance.dispatch.parameter",
        false);
```

##### Wrapper — `SnapshotScan.Dispatch.DispatchEntryPoints.InvokeAsync(System.Func<string, bool, System.Threading.Tasks.Task<bool>>, string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:88`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L88-L92>)

```csharp
private static Task<bool> InvokeAsync(
    Func<string, bool, Task<bool>> callback,
    string key,
    bool fallback) =>
    callback(key, fallback);
```

##### Wrapper — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `System.Func<string, bool, System.Threading.Tasks.Task<bool>>.Invoke(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:88`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L88-L92>)

```csharp
private static Task<bool> InvokeAsync(
    Func<string, bool, Task<bool>> callback,
    string key,
    bool fallback) =>
    callback(key, fallback);
```

##### Delegate 目标 / `DEFINITE` — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

## `acceptance.dispatch.unknown-factory`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`POSSIBLE`

### Reference 1 — Indirect / `POSSIBLE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:81`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L81>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.UnknownFactoryAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.unknown-factory` / `false`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.UnknownFactoryAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:80`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L80-L81>)

```csharp
// Negative boundary: the factory body cannot be reduced to source object creations.
public Task<bool> UnknownFactoryAsync() =>
    LoadExternalStrategy().EvaluateAsync("acceptance.dispatch.unknown-factory", false);
```

##### Wrapper — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（2 个静态候选） — `SnapshotScan.Dispatch.IDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:80`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L80-L81>)

```csharp
// Negative boundary: the factory body cannot be reduced to source object creations.
public Task<bool> UnknownFactoryAsync() =>
    LoadExternalStrategy().EvaluateAsync("acceptance.dispatch.unknown-factory", false);
```

##### Interface 实现目标 / `POSSIBLE` — `SnapshotScan.Dispatch.AlphaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:36`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L36-L37>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Interface 实现目标 / `POSSIBLE` — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

#### 本条 Reference 的 Unresolved
- `UNKNOWN_FACTORY_RESULT` / `CALL`：The Factory result could not be reduced to a complete set of source object creations.

## `acceptance.dispatch.virtual`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：1 条（Direct 0 / Indirect 1）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:33`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L33>)
- 所在 Symbol：`SnapshotScan.Dispatch.DispatchEntryPoints.KnownVirtualAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.dispatch.virtual` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Dispatch.DispatchEntryPoints.KnownVirtualAsync()`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:30`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L30-L34>)

```csharp
public Task<bool> KnownVirtualAsync()
{
    DispatchFlagStrategy strategy = new BetaDispatchFlagStrategy(gateway);
    return strategy.EvaluateAsync("acceptance.dispatch.virtual", true);
}
```

##### Wrapper — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### 静态分派目标

##### 分派调用（1 个静态候选） — `SnapshotScan.Dispatch.DispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:33`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L33>)

```csharp
return strategy.EvaluateAsync("acceptance.dispatch.virtual", true);
```

##### Virtual Override 目标 / `DEFINITE` — `SnapshotScan.Dispatch.BetaDispatchFlagStrategy.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Dispatch/DispatchStrategies.cs:50`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L50-L51>)

```csharp
public override Task<bool> EvaluateAsync(string key, bool fallback) =>
    Gateway.EvaluateAsync(key, fallback);
```

## `acceptance.search.v3`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：2 条（Direct 0 / Indirect 2）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.App/BaseEntryPoints.cs:17`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L17>)
- 所在 Symbol：`SnapshotScan.App.BaseEntryPoints.SearchAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.search.v3` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.App.BaseEntryPoints.SearchAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:16`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L16-L17>)

```csharp
public Task<bool> SearchAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.search", 3, true);
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateVersionAsync(string, int, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:23`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L23-L27>)

```csharp
public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

### Reference 2 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:16`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L16>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ResultControlEntryPoints.UseSearchResultAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.search.v3` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ResultControlEntryPoints.UseSearchResultAsync()`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:14`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L14-L25>)

```csharp
// The result crosses the SearchAsync return boundary, is assigned to a local,
// and is used later after an unrelated statement.
public async Task<string> UseSearchResultAsync()
{
    var searchV3Enabled = await baseEntryPoints.SearchAsync();
    var selectedPipeline = "legacy-search";

    if (searchV3Enabled)
    {
        selectedPipeline = "search-v3";
    }

    return selectedPipeline;
}
```

##### Wrapper — `SnapshotScan.App.BaseEntryPoints.SearchAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:16`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L16-L17>)

```csharp
public Task<bool> SearchAsync() =>
    wrapper.EvaluateVersionAsync("acceptance.search", 3, true);
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateVersionAsync(string, int, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:23`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L23-L27>)

```csharp
public Task<bool> EvaluateVersionAsync(string area, int version, bool fallback)
{
    var key = $"{area}.v{version}";
    return gateway.EvaluateAsync(key, fallback);
}
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `searchV3Enabled: bool`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:16`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L16>)

```csharp
var searchV3Enabled = await baseEntryPoints.SearchAsync();
```

##### 受返回值控制的代码

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:19`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L19-L22>)

```csharp
if (searchV3Enabled)
{
    selectedPipeline = "search-v3";
}
```

## `acceptance.wrapper.fixed`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：4 条（Direct 0 / Indirect 4）
- 汇总结论：`DEFINITE`

### Reference 1 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.App/BaseEntryPoints.cs:20`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L20>)
- 所在 Symbol：`SnapshotScan.App.BaseEntryPoints.FixedAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.wrapper.fixed` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.App.BaseEntryPoints.FixedAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:19`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L19-L20>)

```csharp
public Task<bool> FixedAsync() =>
    wrapper.EvaluateFixedAsync();
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateFixedAsync()`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:29`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L29-L30>)

```csharp
public Task<bool> EvaluateFixedAsync() =>
    gateway.EvaluateAsync("acceptance.wrapper.fixed", true);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

### Reference 2 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:35`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L35>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ConditionalEntryPoints.UseFixedFlagInControlFlowAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.wrapper.fixed` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ConditionalEntryPoints.UseFixedFlagInControlFlowAsync()`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:33`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L33-L41>)

```csharp
// The evaluation result directly controls this complete if block.
public async Task<string> UseFixedFlagInControlFlowAsync()
{
    if (await wrapper.EvaluateFixedAsync())
    {
        return "new-checkout";
    }

    return "legacy-checkout";
}
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateFixedAsync()`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:29`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L29-L30>)

```csharp
public Task<bool> EvaluateFixedAsync() =>
    gateway.EvaluateAsync("acceptance.wrapper.fixed", true);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `evaluation result`

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:35`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L35-L38>)

```csharp
if (await wrapper.EvaluateFixedAsync())
{
    return "new-checkout";
}
```

##### 受返回值控制的代码

[`src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs:35`](<../src/SnapshotScan.ControlFlow/ConditionalEntryPoints.cs#L35-L38>)

```csharp
if (await wrapper.EvaluateFixedAsync())
{
    return "new-checkout";
}
```

### Reference 3 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:44`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L44>)
- 所在 Symbol：`SnapshotScan.ControlFlow.ResultControlEntryPoints.UseFixedResultAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.wrapper.fixed` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.ControlFlow.ResultControlEntryPoints.UseFixedResultAsync()`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:42`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L42-L54>)

```csharp
// The fixed wrapper result is stored once and controls explicit if/else blocks.
public async Task<string> UseFixedResultAsync()
{
    var fixedFlagEnabled = await baseEntryPoints.FixedAsync();

    if (fixedFlagEnabled)
    {
        return "fixed-enabled";
    }
    else
    {
        return "fixed-disabled";
    }
}
```

##### Wrapper — `SnapshotScan.App.BaseEntryPoints.FixedAsync()`

[`src/SnapshotScan.App/BaseEntryPoints.cs:19`](<../src/SnapshotScan.App/BaseEntryPoints.cs#L19-L20>)

```csharp
public Task<bool> FixedAsync() =>
    wrapper.EvaluateFixedAsync();
```

##### Wrapper — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateFixedAsync()`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:29`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L29-L30>)

```csharp
public Task<bool> EvaluateFixedAsync() =>
    gateway.EvaluateAsync("acceptance.wrapper.fixed", true);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

#### Evaluation 返回值流向与受控业务代码

##### 返回值接收 — `fixedFlagEnabled: bool`

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:44`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L44>)

```csharp
var fixedFlagEnabled = await baseEntryPoints.FixedAsync();
```

##### 受返回值控制的代码

[`src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs:46`](<../src/SnapshotScan.ControlFlow/ResultControlEntryPoints.cs#L46-L53>)

```csharp
if (fixedFlagEnabled)
{
    return "fixed-enabled";
}
else
{
    return "fixed-disabled";
}
```

### Reference 4 — Indirect / `DEFINITE`

- 调用位置：[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:30`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L30>)
- 所在 Symbol：`SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateFixedAsync()`
- OpenFeature API：`M:OpenFeature.IFeatureClient.GetBooleanValueAsync(System.String,System.Boolean,OpenFeature.Model.EvaluationContext,OpenFeature.Model.FlagEvaluationOptions,System.Threading.CancellationToken)`（BOOLEAN / VALUE）
- Key / 默认值：`acceptance.wrapper.fixed` / `true`

#### 调用、Wrapper 与真实 Evaluation

##### 引用入口 — `SnapshotScan.Flags.ParameterizedFlagWrapper.EvaluateFixedAsync()`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:29`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L29-L30>)

```csharp
public Task<bool> EvaluateFixedAsync() =>
    gateway.EvaluateAsync("acceptance.wrapper.fixed", true);
```

##### Wrapper — `SnapshotScan.Flags.OpenFeatureBooleanGateway.EvaluateAsync(string, bool)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:10`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L10-L15>)

```csharp
public Task<bool> EvaluateAsync(string key, bool fallback)
{
    var sinkKey = (string)key;
    var sinkFallback = fallback;
    return client.GetBooleanValueAsync(sinkKey, sinkFallback);
}
```

##### 真实 OpenFeature Evaluation — `OpenFeature.IFeatureClient.GetBooleanValueAsync(string, bool, OpenFeature.Model.EvaluationContext?, OpenFeature.Model.FlagEvaluationOptions?, System.Threading.CancellationToken)`

[`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>)

```csharp
return client.GetBooleanValueAsync(sinkKey, sinkFallback);
```

## Key 尚未确定的 Evaluation

这些是源码中存在的 Evaluation，但 Scanner 不能把 Key 证明为一个完整常量；它们不是额外的确定 Feature Flag。

### `Conversion:302d5def021b`
- `UNRESOLVED` / Indirect / [`src/SnapshotScan.Dispatch/DispatchStrategies.cs:37`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L37>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

### `Conversion:302d5def021b`
- `UNRESOLVED` / Indirect / [`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:26`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L26>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

### `Conversion:302d5def021b`
- `UNRESOLVED` / Indirect / [`src/SnapshotScan.Dispatch/DispatchEntryPoints.cs:52`](<../src/SnapshotScan.Dispatch/DispatchEntryPoints.cs#L52>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

### `Conversion:302d5def021b`
- `UNRESOLVED` / Indirect / [`src/SnapshotScan.Dispatch/DispatchStrategies.cs:51`](<../src/SnapshotScan.Dispatch/DispatchStrategies.cs#L51>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

### `key`
- `UNRESOLVED` / Direct / [`src/SnapshotScan.Flags/DirectEvaluations.cs:22`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L22>) / 原因：`UNRESOLVED_EXPRESSION`

### `sinkKey`
- `UNRESOLVED` / Direct / [`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

## Coverage、Unresolved 与 Diagnostic

- Coverage：发现 4 个 Project，加载 4 个，失败 0 个；分析 17 个文件。
- Unresolved：27 项。
  - `INCOMPLETE_DISPATCH_CANDIDATES`：2
  - `UNBOUND_PARAMETER`：10
  - `UNKNOWN_DELEGATE_TARGET`：2
  - `UNKNOWN_FACTORY_RESULT`：2
  - `UNRESOLVED_EXPRESSION`：11
- Diagnostic：0 项。

_本报告实际读取并核对了 7 个源码文件；未写入绝对路径、扫描时间或机器信息。_
