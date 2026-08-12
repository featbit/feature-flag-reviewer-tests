# Snapshot Scan 人工审阅报告

> 本报告由 `snapshot-report.json` 和对应 Workspace 源码确定性生成。所有展示的本地源码都已与报告中的 SHA-256 `contentHash` 核对；本报告不替人工给出通过/失败结论。

## 扫描概览

- Report 状态：`ANALYZED_WITH_UNRESOLVED`
- Schema / Scanner：`1.5` / `0.3.0`
- Workspace 入口：`SnapshotBase.slnx`
- 已加载 Project：2 / 2
- 已分析文件：9
- 确定 Key 的 Feature Flag：5
- Key 尚未确定的表达式实体：3
- Reference：10（Direct 4 / Indirect 6）
- Unresolved / Diagnostic：9 / 0

JSON 对应关系：Feature Flag 列表来自 `flags`；逐条引用来自 `references`；调用链、Wrapper、输入值选择、Evaluation 返回值流向、受控代码和 Sink 来自 `graphNodes` / `graphEdges`；不能证明的部分来自 `unresolved`。

## Feature Flag 列表

| Key | 类型 / API | 默认值候选 | Reference | 证据结论 |
|---|---|---|---:|---|
| `acceptance.checkout.v2` | BOOLEAN/VALUE | false | 2 | DEFINITE |
| `acceptance.direct.boolean` | BOOLEAN/VALUE | false | 1 | DEFINITE |
| `acceptance.direct.string` | STRING/VALUE | control | 1 | DEFINITE |
| `acceptance.search.v3` | BOOLEAN/VALUE | true | 1 | DEFINITE |
| `acceptance.wrapper.fixed` | BOOLEAN/VALUE | true | 2 | DEFINITE |

## `acceptance.checkout.v2`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：false
- 引用：2 条（Direct 0 / Indirect 2）
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

## `acceptance.search.v3`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：1 条（Direct 0 / Indirect 1）
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

## `acceptance.wrapper.fixed`

- 类型：BOOLEAN
- API 详情：BOOLEAN/VALUE
- 默认值候选：true
- 引用：2 条（Direct 0 / Indirect 2）
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
- `UNRESOLVED` / Indirect / [`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:26`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L26>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

### `key`
- `UNRESOLVED` / Direct / [`src/SnapshotScan.Flags/DirectEvaluations.cs:22`](<../src/SnapshotScan.Flags/DirectEvaluations.cs#L22>) / 原因：`UNRESOLVED_EXPRESSION`

### `sinkKey`
- `UNRESOLVED` / Direct / [`src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs:14`](<../src/SnapshotScan.Flags/ParameterizedFlagWrappers.cs#L14>) / 原因：`UNBOUND_PARAMETER`, `UNRESOLVED_EXPRESSION`

## Coverage、Unresolved 与 Diagnostic

- Coverage：发现 2 个 Project，加载 2 个，失败 0 个；分析 9 个文件。
- Unresolved：9 项。
  - `UNBOUND_PARAMETER`：4
  - `UNRESOLVED_EXPRESSION`：5
- Diagnostic：0 项。

_本报告实际读取并核对了 3 个源码文件；未写入绝对路径、扫描时间或机器信息。_
