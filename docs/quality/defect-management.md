# Defect management

Every defect is a GitHub issue labelled `type:defect`, with one severity label and one priority label.

## Severity

| Label | Meaning |
|---|---|
| `severity:critical` | An ineligible donor can book, or a user can reach another user's data |
| `severity:high` | Breaks a requirement with no workaround |
| `severity:medium` | Wrong or inconsistent result with a workaround |
| `severity:low` | Maintainability or cosmetic |

## Priority

| Label | Meaning |
|---|---|
| `priority:P1` | Must be fixed before release |
| `priority:P2` | Should be fixed this cycle |
| `priority:P3` | Fix if time allows |

## Status

| Label | Meaning |
|---|---|
| `status:in-progress` | Being worked on |
| `status:fixed` | Fix merged, awaiting verification |
| `status:verified` | Fix confirmed by a passing test on `main` |
| `status:deferred` | Accepted as open for this release, with a reason |

## Lifecycle

1. **Open:** logged with evidence, severity and priority.
2. **In progress:** someone is fixing it.
3. **Fixed:** the fix is merged through a PR that says `Fixes #n` and passes CI.
4. **Verified:** a test covering the defect passes on `main`.
5. **Closed:** verified and closed.
6. **Deferred:** accepted as open for release, with the reason written on the issue.

## Rule for fixes

Every defect fix must add or change a test that would have caught it.
