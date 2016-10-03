﻿namespace Dargon.Robotics.Simulations2D {
   public class SimulationConstants {
      public float LinearDamping { get; set; }
      public float AngularDamping { get; set; }
      public float WidthMeters { get; set; }
      public float HeightMeters { get; set; }
      public float Density { get; set; }
   }

   public static class SimulationConstantsFactory {
      private const float kAluminumDensity = 2700; // kg/m3

      public static SimulationConstants LandRobot() {
         return new SimulationConstants {
            AngularDamping = 8.75f,
            LinearDamping = 16.0f,
            WidthMeters = 0.66f,
            HeightMeters = 0.66f,
            Density = kAluminumDensity * 0.0125f // brings us to ~64 pounds
         };
      }

      public static SimulationConstants WideLandRobot() {
         // Higher damping = less skidding along simulated drive wheels.
         return new SimulationConstants {
            AngularDamping = 16.75f,
            LinearDamping = 1.0f,
            WidthMeters = 0.96f,
            HeightMeters = 0.71f,
            Density = kAluminumDensity * 0.0125f // brings us to ~64 pounds
         };
      }

      public static SimulationConstants WaterRobot() {
         return new SimulationConstants {
            AngularDamping = 30.0f,
            LinearDamping = 1.0f,
            WidthMeters = 0.66f,
            HeightMeters = 0.66f,
            Density = kAluminumDensity * 0.0475f // brings us to ~100 pounds
         };
      }
   }
}