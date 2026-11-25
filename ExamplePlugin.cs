using System;
using System.Collections.Generic;
using Oxide.Core.Plugins;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("ExampleHologramUser", "Yaruani", "1.0.1")]
    [Description("Example plugin demonstrating how to use HybridHolograms API")]
    public class ExamplePlugin : RustPlugin
    {
        #region Reference

        [PluginReference]
        private Plugin HybridHolograms;

        #endregion

        #region State

        private const string HologramId = "example_hologram";

        #endregion

        #region Oxide Hooks

        void OnServerInitialized()
        {
            if (HybridHolograms == null)
            {
                PrintWarning("HybridHolograms not found. Plugin will not create hologram.");
                return;
            }

            // Create a hologram above world origin
            HybridHolograms.Call(
                "CreateHologram",
                HologramId,
                (Func<Vector3>)(() => GetHologramPosition()),
                new List<string>
                {
                    "HybridHolograms",
                    "External Plugin Example",
                    "API Call Successful"
                },
                1.5f,
                Color.cyan
            );
        }

        void Unload()
        {
            HybridHolograms?.Call("RemoveHologram", HologramId);
        }

        #endregion

        #region Helpers

        private Vector3 GetHologramPosition()
        {
            // Example fixed position (can be dynamic)
            return new Vector3(0f, 5f, 0f);
        }

        #endregion
    }
}
