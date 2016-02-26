﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dargon.Robotics.Devices.BeagleBone.Util;
using Dargon.Robotics.Devices.Common;

namespace Dargon.Robotics.Devices.BeagleBone {
   public class BeagleBoneDeviceFactory : DeviceFactory {
      private readonly DeviceValueFactory deviceValueFactory;
      private readonly IBeagleBoneGpioMotorDeviceFactory gpioMotorDeviceFactory;

      public BeagleBoneDeviceFactory(DeviceValueFactory deviceValueFactory, IBeagleBoneGpioMotorDeviceFactory gpioMotorDeviceFactory) {
         this.deviceValueFactory = deviceValueFactory;
         this.gpioMotorDeviceFactory = gpioMotorDeviceFactory;
      }

      public DigitalOutput DigitalOutput(int pin) {
         return new GpioDigitalOutputImpl(
            $"GPIO_DO_{pin}",
            deviceValueFactory.FromFileCached<bool>(
               BuildPinValuePath(pin),
               DeviceValueAccess.ReadWrite));
      }

      public Motor PwmMotor(int pin) => gpioMotorDeviceFactory.PwmMotor(pin);

      private string BuildPinRootPath(int pin) => $"/sys/class/gpio/gpio{pin}";
      private string BuildPinValuePath(int pin) => $"{BuildPinRootPath(pin)}/value";
   }
}
