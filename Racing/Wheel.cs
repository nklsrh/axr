using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.DirectX.DirectInput;

namespace Racing
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Wheel : Microsoft.Xna.Framework.GameComponent
    {
        public Wheel(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            CheckingForceFeedback();
        }
        Device joystick;

        public void CheckingForceFeedback()
        {
            foreach (DeviceInstance di in Manager.GetDevices(
        DeviceClass.GameControl,
        EnumDevicesFlags.AttachedOnly | EnumDevicesFlags.ForceFeeback))
            {
                joystick = new Device(di.InstanceGuid);
                break;
            }
            if (joystick == null)
            {
                //Throw exception if joystick not found.
                throw new Exception("No joystick that supports forced feedback found.");
            }
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game1 game)
        {
            // TODO: Add your initialization code here
            joystick.SetCooperativeLevel(
                game.Window.Handle,
                CooperativeLevelFlags.Exclusive | CooperativeLevelFlags.Background);

            //Set axis mode absolute.
            joystick.Properties.AxisModeAbsolute = true;
            joystick.Properties.AutoCenter = false;
            joystick.SetDataFormat(DeviceDataFormat.Joystick);
            //Acquire joystick for capturing.
            joystick.Acquire();

            // Enumerate any axes
            foreach (DeviceObjectInstance doi in joystick.Objects)
            {
                if ((doi.ObjectId & (int)DeviceObjectTypeFlags.Axis) != 0)
                {
                    // We found an axis, set the range to a max of 10,000
                    joystick.Properties.SetRange(ParameterHow.ById,
                    doi.ObjectId, new InputRange(-5000, 5000));
                }

                int[] temp;

                // Get info about first two FF axii on the device
                if ((doi.Flags & (int)ObjectInstanceFlags.Actuator) != 0)
                {
                    if (axis != null)
                    {
                        temp = new int[axis.Length + 1];
                        axis.CopyTo(temp, 0);
                        axis = temp;
                    }
                    else
                    {
                        axis = new int[1];
                    }

                    // Store the offset of each axis.
                    axis[axis.Length - 1] = doi.Offset;
                    if (axis.Length == 2)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        EffectObject eo = null;
        Microsoft.DirectX.DirectInput.Effect e;
        int[] axis = null;

        public void Update(Game1 game, GameTime gameTime)
        {
            // TODO: Add your update code here
            foreach (EffectInformation ei in joystick.GetEffects(EffectType.All))
            {
                if (DInputHelper.GetTypeCode(ei.EffectType)
                  == (int)EffectType.ConstantForce)
                {
                    // Fill in some generic values for the effect.
                    e = new Microsoft.DirectX.DirectInput.Effect();
                    e.SetDirection(new int[axis.Length]);
                    e.SetAxes(new int[1]);
                    e.ConditionStruct = new Condition[axis.Length];

                    e.EffectType = EffectType.ConstantForce;
                    e.Duration = (int)DI.Infinite;
                    e.Gain = 10000;
                    e.Constant = new ConstantForce();
                    e.Constant.Magnitude = 1000;
                    e.UsesEnvelope = false;
                    e.SamplePeriod = 0;
                    e.TriggerButton = (int)Microsoft.DirectX.DirectInput.Button.NoTrigger;
                    e.TriggerRepeatInterval = (int)DI.Infinite;
                    e.Flags = EffectFlags.ObjectOffsets | EffectFlags.Cartesian;

                    // Create the effect, using the passed in guid.
                    eo = new EffectObject(ei.EffectGuid, e, joystick);

                }
            }

            

            base.Update(gameTime);
        }
    }
}