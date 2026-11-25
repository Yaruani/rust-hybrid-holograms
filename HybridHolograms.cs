using Oxide.Core;
using Oxide.Core.Plugins;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("HybridHolograms", "Yaruani", "1.0.4")]
    [Description("Stable hybrid 3D hologram system with auto height correction, debug mode and adaptive tick")]
    public class HybridHolograms : RustPlugin
    {
        #region ===== Data =====

        private PluginConfig config;
        private readonly Dictionary<string, Hologram> holograms = new Dictionary<string, Hologram>();

        private class Hologram
        {
            public string Id;
            public Func<Vector3> PositionFunc;
            public List<string> Lines;
            public float BaseDistance;
            public Color Color;

            public readonly Dictionary<ulong, float> NextDrawTimes = new Dictionary<ulong, float>();
        }

        #endregion

        #region ===== Config =====

        private class DebugSettings
        {
            public bool Enabled = false;
            public float HeightPadding = 0.25f;
            public float FallbackHeight = 1.8f;
            public float ForceDistance = 0f;
            public bool IgnoreVisibilityCheck = false;
            public bool PrefixDebugText = true;
        }

        private class TickSettings
        {
            public float NearDistance = 8f;
            public float NearTick = 0.1f;

            public float FarDistance = 25f;
            public float FarTick = 0.5f;

            public float MaxDistance = 60f;
            public float MaxTick = 1.2f;
        }

        private class PluginConfig
        {
            public DebugSettings Debug = new DebugSettings();
            public TickSettings Tick = new TickSettings();
        }

        protected override void LoadDefaultConfig()
        {
            config = new PluginConfig();
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<PluginConfig>();
            }
            catch
            {
                PrintWarning("Config broken, regenerating.");
                LoadDefaultConfig();
            }
            SaveConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(config, true);
        }

        #endregion

        #region ===== Lifecycle =====

        private void OnServerInitialized()
        {
            timer.Every(0.1f, GlobalTick);
            Puts("HybridHolograms loaded.");
        }

        private void Unload()
        {
            holograms.Clear();
        }

        private void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            foreach (var holo in holograms.Values)
                holo.NextDrawTimes.Remove(player.userID);
        }

        #endregion

        #region ===== Global Tick =====

        private void GlobalTick()
        {
            foreach (var player in BasePlayer.activePlayerList)
            {
                if (player == null || !player.IsConnected)
                    continue;

                foreach (var holo in holograms.Values)
                {
                    DrawForPlayer(player, holo);
                }
            }
        }

        private void DrawForPlayer(BasePlayer player, Hologram holo)
        {
            ulong userId = player.userID;
            float now = Time.time;
            if (holo.NextDrawTimes.TryGetValue(userId, out float nextDrawTime) && now < nextDrawTime)
                return;

            Vector3 basePos = holo.PositionFunc();
            Vector3 pos = AutoHeight(basePos);
            float distance = Vector3.Distance(player.transform.position, pos);
            float maxDistance = config.Debug.Enabled && config.Debug.ForceDistance > 0f
                ? config.Debug.ForceDistance
                : holo.BaseDistance;
            if (distance > maxDistance) return;

            if (!config.Debug.Enabled || !config.Debug.IgnoreVisibilityCheck)
            {
                if (!player.IsVisible(pos))
                    return;
            }

            float interval = GetTickInterval(distance);
            holo.NextDrawTimes[userId] = now + interval;
            float drawLifetime = interval * 1.5f;

            List<string> lines = holo.Lines;

            if (config.Debug.Enabled && config.Debug.PrefixDebugText)
            {
                lines = new List<string> { "<color=#ff4444>[DEBUG]</color>" };
                lines.AddRange(holo.Lines);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                player.SendConsoleCommand(
                    "ddraw.text",
                    drawLifetime,
                    holo.Color,
                    pos + Vector3.up * (0.25f * i),
                    lines[i]
                );
            }
        }

        private float GetTickInterval(float distance)
        {
            if (distance <= config.Tick.NearDistance)
                return config.Tick.NearTick;

            if (distance <= config.Tick.FarDistance)
                return config.Tick.FarTick;

            return config.Tick.MaxTick;
        }

        #endregion

        #region ===== Height Auto Correction =====

        private Vector3 AutoHeight(Vector3 basePos)
        {
            var list = new List<BaseEntity>();
            Vis.Entities(basePos, 1.5f, list);

            foreach (var ent in list)
            {
                if (ent == null) continue;

                var col = ent.GetComponent<Collider>();
                if (col != null)
                {
                    return col.bounds.center +
                           Vector3.up * (col.bounds.extents.y + config.Debug.HeightPadding);
                }

                var rend = ent.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    return rend.bounds.center +
                           Vector3.up * config.Debug.HeightPadding;
                }
            }

            return basePos + Vector3.up * config.Debug.FallbackHeight;
        }

        #endregion

        #region ===== Public API =====

        /*
         * API: CreateHologram
         *
         * string id
         * Func<Vector3> positionFunc
         * List<string> lines
         * float distance (0 = auto)
         * Color color
         */
        [HookMethod("CreateHologram")]
        public void CreateHologram(
            string id,
            Func<Vector3> positionFunc,
            List<string> lines,
            float distance = 0f,
            Color? color = null)
        {
            holograms[id] = new Hologram
            {
                Id = id,
                PositionFunc = positionFunc,
                Lines = lines,
                BaseDistance = distance <= 0f ? config.Tick.MaxDistance : distance,
                Color = color ?? Color.white
            };
        }

        /*
         * API: UpdateText
         */
        [HookMethod("UpdateText")]
        public void UpdateText(string id, List<string> lines)
        {
            if (holograms.TryGetValue(id, out var holo))
                holo.Lines = lines;
        }

        /*
         * API: RemoveHologram
         */
        [HookMethod("RemoveHologram")]
        public void RemoveHologram(string id)
        {
            holograms.Remove(id);
        }

        #endregion
    }
}
