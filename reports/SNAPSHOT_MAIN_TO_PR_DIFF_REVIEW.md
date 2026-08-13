# SnapshotScanner Main → PR Diff 人工验收

本文件是对结构化 Diff JSON 的人工阅读指南，不是 Scanner 自动给出的合并结论。Base 使用 `main` 已提交的 Base v4 Snapshot，Head 使用当前 PR 工作树的真实扫描结果。

## 输入与正式产物

| 角色 | 文件 | SHA-256 |
|---|---|---|
| Base Snapshot | `snapshot-base-v4.json` | `38305CB785A9ED50F69888D80CB59C7A1F6FBC6FC0DD387ECBC899537637A7B0` |
| Head Snapshot | `snapshot-pr-head.json` | `77D3FB2A818119E28BA144180F1F261C9885BF1ECD5D8966DA9D3ECDFC560306` |
| Head 人读报告 | `SNAPSHOT_PR_HEAD_REVIEW.md` | `CFAE5222DA925C7F22A1EDA7264FA6590115A133ACEDE7F68E79A58F414B8AAF` |
| Main → PR Diff | `snapshot-main-to-pr.diff.json` | `F37D4958A24F1F4C828A5895191556DC6F2BE10CF5BD3C8CB963E6B7886D3420` |

Head Snapshot 为 923,539 Bytes，Head Markdown 为 181,037 Bytes，Diff JSON 为 140,479 Bytes。

## 人工预期与实际 Delta

### Added Flag Keys

- `acceptance.pr.checkout-banner`：Boolean，默认值 `false`，新的 Direct / Definite Reference。
- `acceptance.pr.get-value-v2`：Boolean，默认值 `false`，由 `FeatureReview:DirectGetValue:*` 配置路径提供。

### Removed Flag Keys

- `acceptance.config.indexer`：`EvaluateIndexerAsync` 保留为普通业务入口，但不再调用 Flag Gateway。
- `acceptance.config.get-value`：同一配置消费者现在解析为 `acceptance.pr.get-value-v2`。

### Changed Flag Input

- `acceptance.di.constructor`：Key、类型和 DI 选择保持不变，默认值从 `false` 改为 `true`；Diff 的变化类型应包含 `DEFAULT_VALUE`。

### Reference Delta

| 类别 | Key | 所在 Symbol | 关键结果 |
|---|---|---|---|
| Added | `acceptance.pr.checkout-banner` | `PullRequestCheckoutBanner.GetBannerAsync()` | Direct / Definite / `false` |
| Removed | `acceptance.config.indexer` | `ConfigurationEntryPoints.EvaluateIndexerAsync()` | 原 Indirect / Definite Reference 消失 |
| Modified | `acceptance.config.get-value` → `acceptance.pr.get-value-v2` | `ConfigurationEntryPoints.EvaluateGetValueAsync()` | `KEY`、`KEY_SOURCE`、`DEFAULT_VALUE` |
| Modified | `acceptance.di.constructor` | `ConstructorInjectionEntry.EvaluateAsync()` | `DEFAULT_VALUE`，并伴随位置跨度和 Evidence Path 更新 |

结构化 Summary 必须精确为：

```text
addedFlagKeyCount               = 2
removedFlagKeyCount             = 2
changedFlagInputCount           = 1
addedReferenceCount             = 1
removedReferenceCount           = 1
modifiedReferenceCount          = 2
changedWrapperCount             = 28
changedArchitectureBindingCount = 2
changedConfigBindingCount       = 8
affectedChangedFileCount        = 3
unresolvedCount                 = 38
```

## Head 人读证据

阅读 `SNAPSHOT_PR_HEAD_REVIEW.md` 时重点检查：

1. `acceptance.pr.checkout-banner` 展示真实 OpenFeature Sink、`enabled` 返回值接收及三元业务结果。
2. `acceptance.pr.get-value-v2` 展示两个完整配置路径、`appsettings.json` 相对来源、Key/default 最终值和 Gateway/Sink。
3. `acceptance.di.constructor` 仍展示 Alpha 的 DI 注册、构造注入和真实 Sink，但默认值已为 `true`。
4. `acceptance.config.indexer` 与旧 `acceptance.config.get-value` 不再出现在 Head Flag 列表中。

Head 的业务总量为：41 Flags、32 个确定 Key、54 References（6 Direct / 48 Indirect；24 Definite / 21 Possible / 9 Unresolved）、350 Nodes、644 Edges、38 Unresolved、0 Diagnostics。

## Unresolved 与文件边界

Diff 状态为 `DIFFED_WITH_UNRESOLVED`，因为 Base 和 Head 都保留 Base v4 的动态 Key、动态配置路径及开放分派边界。38 条 Diff Unresolved 全部为 `BOTH`；没有 Head-only Unresolved，所以本 PR 没有扩大静态分析未知面。

`affectedChangedFiles` 是与 Reference/Graph/Unresolved 位置相连的 Flag 相关文件子集，不是原始 Git changed-files 列表。配置文件路径和值变化仍通过 `changedConfigBindings` 的标签和 Head Configuration Evidence 展示；Git 源码审阅仍负责查看完整 PR 文件差异。

## 确定性验证

正式流程执行两次。两次 Head JSON、Head Markdown 和 Diff JSON 分别与正式文件字节一致；临时产物随后删除。Snapshot 与 Diff 分别使用 Draft 2020-12 Schema 独立校验，报告中不包含机器绝对路径或 Secret。

复现命令见仓库 README 的“Main → PR Delta 闭环”。
