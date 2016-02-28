﻿using Dargon.Robotics.Codebases.Iterative;
using Dargon.Robotics.Devices;
using Dargon.Robotics.Subsystems.DriveTrains.Holonomic;

namespace Dargon.Robotics.Demo {
   public class DemoRobotIterativeUserCode : IterativeRobotUserCode {
      private readonly Gamepad gamepad;
      private readonly HolonomicDriveTrain driveTrain;

      public DemoRobotIterativeUserCode(Gamepad gamepad, HolonomicDriveTrain driveTrain) {
         this.gamepad = gamepad;
         this.driveTrain = driveTrain;
      }

      public override void OnTick() {
         if (gamepad.A) {
            driveTrain.MecanumDrive(gamepad.LeftX, gamepad.LeftY);
         } else if (gamepad.B) {
            driveTrain.TankDrive(gamepad.LeftY, gamepad.LeftY);
         } else {
            driveTrain.TankDrive(gamepad.LeftY, gamepad.RightY);
         }
      }
   }
}