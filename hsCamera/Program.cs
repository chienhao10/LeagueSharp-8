﻿using System;
using System.Collections.Generic;
using System.Linq;
using hsCamera.Handlers;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace hsCamera
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += HsCameraOnLoad;
        }

        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public static Menu _config;
        public static List<Obj_AI_Hero> HeroList => HeroManager.Enemies.ToList();

        private static void AddChampionMenu()
        {
            for (int i = 0; i < HeroList.Count; i++)
            {
                _config.AddItem(new MenuItem("enemies" + HeroList[i].ChampionName, "Show Enemy -> (" + HeroList[i].ChampionName + ")")
                    .SetValue(new KeyBind(Convert.ToUInt32(96 + i), KeyBindType.Press)));

                _config.Item("enemies"+ HeroList[i].ChampionName).ValueChanged += (sender, e) =>
                 {
                     if (e.GetNewValue<KeyBind>().Active == e.GetOldValue<KeyBind>().Active) return;
                     if (e.GetNewValue<KeyBind>().Active == false) CameraMovement.SemiDynamic(Player.Position);
                 };
            }
        }

        /// <summary>
        /// Amazing OnLoad :jew:
        /// </summary>
        /// <param name="args"></param>
        private static void HsCameraOnLoad(EventArgs args)
        {
            _config = new Menu("hsCamera [Official]", "hsCamera", true);
            {
                AddChampionMenu();
                _config.AddItem(
                    new MenuItem("semi.dynamic", "Semi-Dynamic Camera?").SetValue(new KeyBind(32, KeyBindType.Press)))
                    .ValueChanged += (sender, e) =>
                    {
                        if (e.GetNewValue<KeyBind>().Active == e.GetOldValue<KeyBind>().Active) return;
                        if (e.GetNewValue<KeyBind>().Active == false) CameraMovement.SemiDynamic(Player.Position);
                    };
                _config.AddItem(new MenuItem("follow.dynamic", "Follow-Champion Camera?").SetValue(new KeyBind(17, KeyBindType.Press)));
                _config.AddItem(new MenuItem("dynamicmode", "Camera Mode?").SetValue(new StringList(new[] { "Normal", "Follow Cursor", "Follow Teamfights" }, 2)));

                _config.AddItem(new MenuItem("LastHit", "Last Hit").SetShared().SetValue(new KeyBind('X', KeyBindType.Press)));
                _config.AddItem(new MenuItem("LaneClear", "LaneClear").SetShared().SetValue(new KeyBind('V', KeyBindType.Press)));
                _config.AddItem(new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press)));
                _config.AddItem(new MenuItem("credits", "                      .:Official Version of hsCamera:.")).SetFontStyle(System.Drawing.FontStyle.Bold, SharpDX.Color.DeepPink);
                _config.AddToMainMenu();
            }
            Game.OnUpdate += HsCameraOnUpdate;
        }

        

        private static void HsCameraOnUpdate(EventArgs args)
        {
            AllModes.AllModes.CameraMode();

            for (int i = 0; i < HeroList.Count; i++)
            {
                if (_config.Item("enemies"+ HeroList[i].ChampionName).GetValue<KeyBind>().Active)
                {
                    if (HeroList[i].IsValid && HeroList[i] != null)
                    {
                        var position = HeroList[i].Position;
                        Camera.Position = position;
                    }
                }
            }
            
        }
        
    }
}
