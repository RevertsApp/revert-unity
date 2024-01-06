using System;
using UnityEngine.LowLevel;
using System.Linq;
using Revert.Core.Patterns;
using UnityEngine;

namespace Revert.Initializers {
  /// <summary>
  /// This class is responsible for managing the player loop.
  /// By default all updates loops are disabled
  /// </summary>
  [DefaultExecutionOrder(-10)]
  public class PlayerLoopManager : Singleton<PlayerLoopManager> {
    private PlayerLoopSystem originalPlayerLoop;
    public PlayerLoopSystem customPlayerLoop;
    protected override void Awake() {
      base.Awake();
      originalPlayerLoop = PlayerLoop.GetDefaultPlayerLoop(); // Storing the original player loop
      customPlayerLoop = CreateCustomPlayerLoop();
      PlayerLoop.SetPlayerLoop(customPlayerLoop);
    }

    /// <summary>
    /// Creates a custom player loop, where all update loops are disabled
    /// </summary>
    /// <returns> Returns a new player loop, where all update loops are disabled </returns>
    private PlayerLoopSystem CreateCustomPlayerLoop() {
      var customPlayerLoop = originalPlayerLoop;
      
      var playerLoopSystems = customPlayerLoop.subSystemList.Where(subSystem =>
        subSystem.type != typeof(UnityEngine.PlayerLoop.FixedUpdate) && // Disable the physics loop
        subSystem.type != typeof(UnityEngine.PlayerLoop.Update) &&
        subSystem.type != typeof(UnityEngine.PlayerLoop.EarlyUpdate) &&
        subSystem.type != typeof(UnityEngine.PlayerLoop.PreUpdate) &&
        subSystem.type != typeof(UnityEngine.PlayerLoop.TimeUpdate) &&
        // subSystem.type != typeof(UnityEngine.PlayerLoop.PostLateUpdate) && // Responsible for calling start and rendering UI elements inside the editor
        subSystem.type != typeof(UnityEngine.PlayerLoop.PreLateUpdate)).ToArray();
      
      customPlayerLoop.subSystemList = playerLoopSystems;
      return customPlayerLoop;
    }

    /// <summary>
    /// Toggles the update loop on or off
    /// Make sure to always disable loops after you are done with it; for example: if you are running an animation
    /// keep the update loop on, but if you are done with the animation, disable the update loop 
    /// </summary>
    /// <param name="on"> determine if the update loop should be enabled or disabled </param>
    public void ToggleUpdateLoop(bool on) { // TODO: Enables all type of update loops instead of just the update loop
      int updateLoopIndex = Array.FindIndex(customPlayerLoop.subSystemList, system => system.type == typeof(UnityEngine.PlayerLoop.Update));
      if (on && updateLoopIndex == -1) {
        var newSubSystemList = customPlayerLoop.subSystemList.ToList();
        newSubSystemList.Add(originalPlayerLoop.subSystemList.First(playerLoopSystem =>
          playerLoopSystem.type == typeof(UnityEngine.PlayerLoop.Update)));
        newSubSystemList.Add(originalPlayerLoop.subSystemList.First(playerLoopSystem =>
          playerLoopSystem.type == typeof(UnityEngine.PlayerLoop.TimeUpdate)));
        customPlayerLoop.subSystemList = newSubSystemList.ToArray();
      } else {
        if (updateLoopIndex != -1) {
          var newSubSystemList = customPlayerLoop.subSystemList.ToList();
          newSubSystemList.Remove(originalPlayerLoop.subSystemList.First(playerLoopSystem =>
            playerLoopSystem.type == typeof(UnityEngine.PlayerLoop.Update)));
          newSubSystemList.Remove(originalPlayerLoop.subSystemList.First(playerLoopSystem =>
            playerLoopSystem.type == typeof(UnityEngine.PlayerLoop.TimeUpdate)));
          customPlayerLoop.subSystemList = newSubSystemList.ToArray();
        }
      }
      PlayerLoop.SetPlayerLoop(customPlayerLoop);
    }
  }
}
