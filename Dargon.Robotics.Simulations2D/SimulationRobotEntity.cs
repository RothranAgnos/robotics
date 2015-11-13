using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using ItzWarty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Dargon.Robotics.Simulations2D {
   public class SimulationRobotEntity {
      private readonly SimulationConstants constants;
      private readonly SimulationRobotState robotState;
      private Body robotBody;

      public SimulationRobotEntity(SimulationConstants constants, SimulationRobotState robotState) {
         this.constants = constants;
         this.robotState = robotState;
      }

      public void Initialize(World world) {
         robotBody = BodyFactory.CreateRectangle(world, robotState.Width, robotState.Height, robotState.Density);
         robotBody.BodyType = BodyType.Dynamic;
         robotBody.Position = new Vector2(10, 10);
         robotBody.Rotation = (float)Math.PI;
         robotBody.AngularDamping = constants.AngularDamping;
         robotBody.LinearDamping = constants.LinearDamping;
      }

      public void Render(Renderer renderer) {
         DrawRobotBody(renderer);
         robotState.MotorStates.ForEach(x => DrawMotorBody(renderer, x));
      }

      private void DrawRobotBody(Renderer renderer) {
         renderer.DrawCenteredRectangleWorld(
            robotBody.Position,
            new Vector2(robotState.Width, robotState.Height),
            robotBody.Rotation,
            Color.Red);
         
         // Draw up vector
         renderer.DrawLineSegmentWorld(
            robotBody.Position,
            robotBody.Position + Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(robotBody.Rotation)),
            Color.Magenta);

         // Draw friction vector
//         renderer.DrawForceVectorWorld(
//            robotBody.Position,
//            ComputeRobotFrictionForceWorld(),
//            Color.Lime);

      }

      private void DrawMotorBody(Renderer renderer, SimulationMotorState motorState) {
         var position = robotBody.GetWorldPoint(motorState.RelativePosition);
         var extents = new Vector2(robotState.Width / 10, robotState.Height / 10);
         renderer.DrawCenteredRectangleWorld(
            position,
            extents,
            robotBody.Rotation,
            Color.Lime);
         var forceVectorWorld = Vector2.Transform(motorState.CurrentForceVector, Matrix.CreateRotationZ(robotBody.Rotation));
         renderer.DrawForceVectorWorld(position, forceVectorWorld, Color.Cyan);
      }

      public void ApplyForces() {
         var motors = robotState.MotorStates;
         motors.ForEach(ApplyMotorForces);
      }
      
      private void ApplyMotorForces(SimulationMotorState motorState) {
         var forceVectorWorld = Vector2.Transform(motorState.CurrentForceVector, Matrix.CreateRotationZ(robotBody.Rotation));
//         Console.WriteLine(forceVectorWorld);
         robotBody.ApplyForce(
            forceVectorWorld,
            robotBody.GetWorldPoint(motorState.RelativePosition)
            );
         //         renderer.DrawForceVectorWorld(position, forceVectorWorld, Color.Cyan);
      }

      public void UpdateSensors(float dtSeconds) {
         var wheelEncoders = robotState.WheelEncoderStates;
         wheelEncoders.ForEach(x => UpdateWheelEncoders(dtSeconds, x));
//         if (StaticRandom.Next(0, 2000) == 0) {
//            Console.WriteLine(wheelEncoders.Select(x => x.Name + ": " + x.Position).Join(", "));
//         }
      }

      private void UpdateWheelEncoders(float dtSeconds, SimulationWheelEncoderState wheelEncoderState) {
         var motor = wheelEncoderState.Motor;
         var currentWorldPosition = robotBody.GetWorldPoint(motor.RelativePosition);
         if (!wheelEncoderState.HasBeenUpdated) {
            wheelEncoderState.HasBeenUpdated = true;
         } else {
            var deltaPositionAbsolute = currentWorldPosition - wheelEncoderState.LastWorldPosition;
            var deltaPositionRelative = Vector2.Transform(deltaPositionAbsolute, Matrix.CreateRotationZ(-robotBody.Rotation));
            var deltaPositionSign = Math.Sign(Vector2.Dot(motor.MaxForceVector, deltaPositionRelative));
            var deltaPosition = deltaPositionAbsolute.Length() * deltaPositionSign;
            var velocity = deltaPosition / dtSeconds;
            wheelEncoderState.Position += deltaPosition;
            wheelEncoderState.Velocity = velocity;
         }
         wheelEncoderState.LastWorldPosition = currentWorldPosition;
      }
   }
}