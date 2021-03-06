﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace hsCamera.Handlers
{
    class CameraMovement : Program
    {
        public static void SemiDynamic(Vector3 position)
        {
            var distance = Camera.Position.Distance(position);


            if (distance <= 1)
            {
                return;
            }

            var speed = Math.Max(0.2f, Math.Min(20, distance * 0.0007f * 20));
            switch (_config.Item("dynamicmode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    {
                        var direction = (position - Camera.Position).Normalized() * (speed);
                        Camera.Position = direction + Camera.Position;
                    }
                    break;
                case 1:
                    {
                        var direction = (position - Camera.Position).Normalized() * (22.3f);
                        Camera.Position = direction + Camera.Position;
                    }
                    break;
                case 2:
                    {
                        var direction = (position - Camera.Position).Normalized() * (15.2f);
                        Camera.Position = direction + Camera.Position;
                    }
                    break;
            }
        }
    }
}
