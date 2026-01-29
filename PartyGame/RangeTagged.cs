// Pseudocode / Plan (detailed):
// - Purpose: Provide a simple marker component named `RangeTagged` so `HitBoxTag` can
//   add or detect this component using `TryGetComponent<RangeTagged>(out _)`.
// - Requirements:
//   - Must be a Unity `MonoBehaviour` so it can be added/removed at runtime with `AddComponent`.
//   - No behaviour is required; it's purely a marker tag to avoid relying on Unity Tags.
//   - Keep class public and in the global namespace to match existing usage in `HitBoxTag`.
// - Implementation steps:
//   1. Create a new C# file `RangeTagged.cs` under `Assets/Jayden/Hit Box/`.
//   2. Add `using UnityEngine;` for `MonoBehaviour` base class.
//   3. Declare `public class RangeTagged : MonoBehaviour` with no additional fields or methods.
//   4. Optionally add a comment explaining its marker role for future maintainers.
//
// The class below implements this marker component.

using UnityEngine;

/// <summary>
/// Marker component used by `HitBoxTag` to mark objects detected in range when not using Unity tags.
/// This component intentionally contains no logic — it exists solely so `TryGetComponent<RangeTagged>(out _)`
/// and `AddComponent<RangeTagged>()` calls succeed without requiring a Tag asset in the Tag Manager.
/// </summary>
public class RangeTagged : MonoBehaviour
{
    // Intentionally empty - serves as a lightweight marker component.
}