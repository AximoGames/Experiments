// This file is part of Aximo, a Game Engine written in C#. Web: https://github.com/AximoGames
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Aximo.Engine;
using Aximo.Engine.Components.Geometry;
using Aximo.Engine.Components.Lights;
using Aximo.Engine.Components.UI;
using Aximo.VertexData;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;

namespace Aximo.ProcTest
{
    public class ProcTestApplication : Application
    {
        protected override void SetupScene()
        {
            var materialWood1 = new Material()
            {
                DiffuseTexture = Texture.GetFromFile("Textures/woodenbox.png"),
                SpecularTexture = Texture.GetFromFile("Textures/woodenbox_specular.png"),
                Ambient = 0.3f,
                Shininess = 32.0f,
                SpecularStrength = 0.5f,
                CastShadow = true,
            };

            SceneContext.AddActor(new Actor(new CubeComponent()
            {
                Name = "Ground",
                RelativeScale = new Vector3(50, 50, 1),
                RelativeTranslation = new Vector3(0f, 0f, -0.5f),
                Material = materialWood1,
            }));

            SceneContext.AddActor(new Actor(new GridPlaneComponent(10, true)
            {
                Name = "GridPlaneXY",
                RelativeTranslation = new Vector3(0f, 0f, 0.01f),
            }));
            SceneContext.AddActor(new Actor(new GridPlaneComponent(10, true)
            {
                Name = "GridPlaneYZ",
                RelativeTranslation = new Vector3(-10f, 0f, 0.01f),
                RelativeRotation = new Vector3(0, 0.25f, 0).ToQuaternion(),
            }));
            SceneContext.AddActor(new Actor(new GridPlaneComponent(10, true)
            {
                Name = "GridPlaneXZ",
                RelativeTranslation = new Vector3(0f, 10f, 0.01f),
                RelativeRotation = new Vector3(0.25f, 0, 0).ToQuaternion(),
            }));
            SceneContext.AddActor(new Actor(new CrossLineComponent(10, true)
            {
                Name = "CenterCross",
                RelativeTranslation = new Vector3(0f, 0f, 0.02f),
                RelativeScale = new Vector3(2.0f),
            }));

            var flowContainer = new UIFlowContainer()
            {
                DefaultChildSizes = new Vector2(0, 50),
                ExtraChildMargin = new UIAnchors(10, 10, 10, 0),
            };
            SceneContext.AddActor(new Actor(flowContainer));

            SceneContext.AddActor(new Actor(new StatsComponent()
            {
                Name = "Stats",
                CustomOrder = 10,
            }));

            SceneContext.AddActor(new Actor(new LineComponent(new Vector3(0, 0, 0), new Vector3(2, 2, 2))
            {
                Name = "DebugLine",
            }));

            SceneContext.AddActor(new Actor(new DirectionalLightComponent()
            {
                RelativeTranslation = new Vector3(0, 2, 2.5f),
                Name = "MovingLight",
            }));
            SceneContext.AddActor(new Actor(new DirectionalLightComponent()
            {
                RelativeTranslation = new Vector3(2f, 0.5f, 3.25f),
                Name = "StaticLight",
            }));

            SceneContext.AddActor(new Actor(new CubeComponent()
            {
                Name = "GroundCursor",
                RelativeTranslation = new Vector3(0, 1, 0.05f),
                RelativeScale = new Vector3(1.0f, 1.0f, 0.1f),
                Material = materialWood1,
            }));

            SceneContext.AddActor(new Actor(new CubeComponent()
            {
                Name = "Box1",
                RelativeRotation = new Vector3(0, 0, 0.5f).ToQuaternion(),
                RelativeScale = new Vector3(1),
                RelativeTranslation = new Vector3(0, 0, 0.5f),
                Material = materialWood1,
            }));

            var cmp = CreateMesh();
            SceneContext.AddActor(new Actor(cmp));

            // For performance reasons, skybox should rendered as last
            SceneContext.AddActor(new Actor(new SkyBoxComponent()
            {
                Name = "Sky",
            }));
        }

        private StaticMeshComponent CreateMesh()
        {
            var box = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_BOX_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_BOX_COORDINATES);
            var sphere = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_SPHERE_COORDINATES);
            sphere.Scale(0.68, 0.68, 0.68);

            var cylinder1 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES);
            cylinder1.Scale(0.38, 1, 0.38);

            var cylinder2 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES);
            cylinder2.Scale(0.38, 1, 0.38);
            cylinder2.Rotate(Math.PI / 2, 0);

            var cylinder3 = new Net3dBool.Solid(Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_VERTICES, Net3dBool.DefaultCoordinates.DEFAULT_CYLINDER_COORDINATES);
            cylinder3.Scale(0.38, 1, 0.38);
            cylinder3.Rotate(Math.PI / 2, 0);
            cylinder3.Rotate(0, Math.PI / 2);

            var modeller = new Net3dBool.BooleanModeller(box, sphere);
            var tmp = modeller.GetIntersection();

            modeller = new Net3dBool.BooleanModeller(tmp, cylinder1);
            tmp = modeller.GetDifference();

            modeller = new Net3dBool.BooleanModeller(tmp, cylinder2);
            tmp = modeller.GetDifference();

            modeller = new Net3dBool.BooleanModeller(tmp, cylinder3);
            tmp = modeller.GetDifference();

            VertexDataPosNormalColor[] data = tmp.GetVertices().Select(v => new VertexDataPosNormalColor(new Vector3((float)v.X, (float)v.Y, (float)v.Z), new Vector3(1, 0, 0), new Vector4(1, 1, 0, 1))).ToArray();
            for (var i = 0; i < data.Length; i++)
            {
                var face = i / 3;
                var vertex = i % 3;
                // switch (vertex)
                // {
                //     case 0:
                //         data[i].Color = new Vector4(1, 0, 0, 1);
                //         break;
                //     case 1:
                //         data[i].Color = new Vector4(0, 1, 0, 1);
                //         break;
                //     case 2:
                //         data[i].Color = new Vector4(0, 0, 1, 1);
                //         break;
                // }
                data[i].Normal = Vector3.UnitX;
            }
            var meshData = Mesh.CreateFromVertices(data, tmp.GetIndices().ToArray());
            meshData.Expand();
            meshData.RecalculateNormals(25f);

            var material = new Material
            {
                Ambient = 0.5f,
                Color = new Vector4(1, 1, 1, 1),
                UseVertexColor = true,
                PipelineType = PipelineType.Forward,
            };

            var comp = new StaticMeshComponent()
            {
                Name = "BoolMesh",
                RelativeRotation = new Vector3(0, 0, 0.5f).ToQuaternion(),
                RelativeScale = new Vector3(1),
                RelativeTranslation = new Vector3(2, 0, 0.5f),
                Material = material,
            };
            comp.SetMesh(meshData);
            return comp;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (CurrentMouseWorldPositionIsValid)
            {
                var cursor = SceneContext.GetActor("GroundCursor")?.RootComponent;
                if (cursor != null)
                    cursor.RelativeTranslation = new Vector3(CurrentMouseWorldPosition.X, CurrentMouseWorldPosition.Y, cursor.RelativeTranslation.Z);
            }
        }
    }
}
