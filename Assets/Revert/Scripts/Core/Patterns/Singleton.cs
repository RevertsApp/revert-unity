using System;
using UnityEngine;

namespace Revert.Core.Patterns {
  /// <summary>
  /// Singleton class that ensures only one statically accessible instance of this MonoBehaviour exists.
  /// </summary>
  public abstract class Singleton<T> : MonoBehaviour, IDisposable where T : Singleton<T> {
    public static T Instance { get; protected set; }

    // IMPORTANT: When overriding, make sure to call base.Awake()
    protected virtual void Awake() {
      if ( Instance != null && Instance != this ) {
        Debug.LogWarning("An instance of this Singleton already exists." + "Only one singleton should exist.",
          gameObject);
        DestroyImmediate( gameObject );
      } else {
        Instance = ( T )this;
      }
    }

    public virtual void Dispose() {
      if (Instance == this) {
        Instance = null;
      }
    }

    protected virtual void OnDestroy() {
      Dispose();
    }
  }
}
