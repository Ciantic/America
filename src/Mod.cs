﻿using System.Reflection;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using HarmonyLib;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

namespace TerraformHardening
{
    public class Mod : IMod
    {
        public static Harmony harmony;
        public static ILog log = LogManager.GetLogger($"{nameof(TerraformHardening)}.{nameof(Mod)}").SetShowsErrorsInUI(true);

        public static Settings settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            // log.Info("Loading Terraform Hardening");

            settings = new Settings(this);
            settings.RegisterInOptionsUI();

            GameManager.instance.localizationManager.AddSource("en-US", Settings.GetLocales(settings));
            AssetDatabase.global.LoadSettings(nameof(TerraformHardening), settings, new Settings(this));

            harmony = new Harmony("terraformhardening");
            try
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (System.Exception e)
            {
                log.Error(e);
            }

            // updateSystem.UpdateAt<MySearchSystem>(SystemUpdatePhase.MainLoop);

            // if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            //     log.Info($"Current mod asset at {asset.path}");
        }

        public void OnDispose()
        {
            // log.Info(nameof(OnDispose));
            harmony.UnpatchAll();
        }
    }

}
