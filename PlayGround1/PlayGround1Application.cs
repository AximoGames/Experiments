// This file is part of Aximo, a Game Engine written in C#. Web: https://github.com/AximoGames
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Aximo.Engine;
using Aximo.Engine.Audio;
using Aximo.Engine.Components.Geometry;
using Aximo.Engine.Components.Lights;
using Aximo.Engine.Components.UI;
using Aximo.Engine.Windows;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;

namespace Aximo.PlayGround1
{
    public class PlayGround1Application : Application
    {
        protected override void SetupScene()
        {
            Aximo.Engine.Audio.AudioTest.Main_();

            // 745209020,1575012077, 1070395224,1297930682,1919654508,276724715,1493067016,651225054,904005342
            var gen = new Generators.AlchemyCircle.AlchemyCircleOptions
            {
                Seed = 1919654508,
                Size = 256,
                Thickness = 4,
            };

            var materialUV = new Material()
            {
                DiffuseTexture = Texture.GetFromFile("Textures/Engine/UVTest.png"),
                Ambient = 0.3f,
                Shininess = 32.0f,
                SpecularStrength = 0.5f,
                CastShadow = true,
            };

            var materialGroundCursor = new Material()
            {
                DiffuseTexture = Texture.GetFromFile(AssetManager.GetAssetsPath("Textures/AlchemyCircle/.png", gen)),
                Ambient = 0.3f,
                Shininess = 32.0f,
                SpecularStrength = 0.5f,
                CastShadow = false,
                PipelineType = PipelineType.Forward,
            };

            var materialWood1 = new Material()
            {
                DiffuseTexture = Texture.GetFromFile("Textures/woodenbox.png"),
                SpecularTexture = Texture.GetFromFile("Textures/woodenbox_specular.png"),
                Ambient = 0.3f,
                Shininess = 32.0f,
                SpecularStrength = 0.5f,
                CastShadow = true,
            };
            var materialWood2 = new Material()
            {
                DiffuseTexture = Texture.GetFromFile("Textures/wood.png"),
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

            SceneContext.AddActor(new Actor(new QuadComponent()
            {
                Name = "GroundCursor",
                RelativeTranslation = new Vector3(0, 1, 0.05f),
                RelativeScale = new Vector3(1.0f, 1.0f, 0.1f),
                Material = materialGroundCursor,
            }));

            // GameContext.AddActor(new Actor(new CubeComponent()
            // {
            //     Name = "Box1",
            //     RelativeRotation = new Vector3(0, 0, 0.5f).ToQuaternion(),
            //     RelativeScale = new Vector3(1),
            //     RelativeTranslation = new Vector3(0, 0, 0.5f),
            //     Material = materialWood1,
            // }));

            var cmp = CreateMesh();
            cmp.AddMaterial(materialUV);
            cmp.AddMaterial(materialWood1);
            cmp.AddMaterial(materialWood2);
            SceneContext.AddActor(new Actor(cmp));

            Tween.For(cmp)
                .Duration(2)
                .ScaleFunc(ScaleFuncs.Power2EaseInOut)
                .Scale(new Vector3(3f)).Translate(1, 0).Then()
                .Translate(1, 3).Then()
                .Scale(new Vector3(1)).Then()
                .Translate(-1, -1).Scale(new Vector3(3)).Then()
                .Duration(0.0f).Translate(0, 0).Scale(1)
                .Repeat().Start();

            // For performance reasons, skybox should rendered as last
            SceneContext.AddActor(new Actor(new SkyBoxComponent()
            {
                Name = "Sky",
            }));
        }

        protected override void OnMouseDown(MouseButtonArgs e)
        {
            AudioManager.Default.PlayAsync("/tmp/subrack.json");
            base.OnMouseDown(e);
        }

        private StaticMeshComponent CreateMesh()
        {
            var tmp = Mesh.CreateSphere();
            tmp.Name = "Bomb";

            var m2 = Mesh.CreateCylinder();
            m2.Scale(0.3f, 0.3f);
            m2.Translate(new Vector3(0, 0, 0.05f));
            tmp.AddMesh(m2, 1);

            var m3 = Mesh.CreateCylinder();
            m2.Scale(0.15f, 0.15f);
            m2.Translate(new Vector3(0, 0, 0.3f));
            tmp.AddMesh(m2, 2);

            // var tmp3 = Mesh.CreateCylinder(1f, 0.5f);
            // tmp3.Translate(new Vector3(0, -1f, 0));
            // tmp.AddMesh(tmp3);
            // //tmp.AddMesh(Mesh.CreateCube());

            return new StaticMeshComponent(tmp)
            {
                RelativeTranslation = new Vector3(0, 0, 0.55f),
            };
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

    // public static class NormalSolver
    // {
    //     /// <summary>
    //     ///     Recalculate the normals of a mesh based on an angle threshold. This takes
    //     ///     into account distinct vertices that have the same position.
    //     /// </summary>
    //     /// <param name="mesh"></param>
    //     /// <param name="angle">
    //     ///     The smoothing angle. Note that triangles that already share
    //     ///     the same vertex will be smooth regardless of the angle!
    //     /// </param>
    //     public static void RecalculateNormals(this Mesh mesh, float angle)
    //     {
    //         var cosineThreshold = MathF.Cos(angle * AxMath.Deg2Rad);

    //         var vertices = mesh.vertices;
    //         var normals = new Vector3[vertices.Length];

    //         // Holds the normal of each triangle in each sub mesh.
    //         var triNormals = new Vector3[mesh.subMeshCount][];

    //         var dictionary = new Dictionary<VertexKey, List<VertexEntry>>(vertices.Length);

    //         for (var subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex)
    //         {

    //             var triangles = mesh.GetTriangles(subMeshIndex);

    //             triNormals[subMeshIndex] = new Vector3[triangles.Length / 3];

    //             for (var i = 0; i < triangles.Length; i += 3)
    //             {
    //                 int i1 = triangles[i];
    //                 int i2 = triangles[i + 1];
    //                 int i3 = triangles[i + 2];

    //                 // Calculate the normal of the triangle
    //                 Vector3 p1 = vertices[i2] - vertices[i1];
    //                 Vector3 p2 = vertices[i3] - vertices[i1];
    //                 Vector3 normal = Vector3.Cross(p1, p2).Normalized();
    //                 int triIndex = i / 3;
    //                 triNormals[subMeshIndex][triIndex] = normal;

    //                 List<VertexEntry> entry;
    //                 VertexKey key;

    //                 if (!dictionary.TryGetValue(key = new VertexKey(vertices[i1]), out entry))
    //                 {
    //                     entry = new List<VertexEntry>(4);
    //                     dictionary.Add(key, entry);
    //                 }
    //                 entry.Add(new VertexEntry(subMeshIndex, triIndex, i1));

    //                 if (!dictionary.TryGetValue(key = new VertexKey(vertices[i2]), out entry))
    //                 {
    //                     entry = new List<VertexEntry>();
    //                     dictionary.Add(key, entry);
    //                 }
    //                 entry.Add(new VertexEntry(subMeshIndex, triIndex, i2));

    //                 if (!dictionary.TryGetValue(key = new VertexKey(vertices[i3]), out entry))
    //                 {
    //                     entry = new List<VertexEntry>();
    //                     dictionary.Add(key, entry);
    //                 }
    //                 entry.Add(new VertexEntry(subMeshIndex, triIndex, i3));
    //             }
    //         }

    //         // Each entry in the dictionary represents a unique vertex position.

    //         foreach (var vertList in dictionary.Values)
    //         {
    //             for (var i = 0; i < vertList.Count; ++i)
    //             {

    //                 var sum = new Vector3();
    //                 var lhsEntry = vertList[i];

    //                 for (var j = 0; j < vertList.Count; ++j)
    //                 {
    //                     var rhsEntry = vertList[j];

    //                     if (lhsEntry.VertexIndex == rhsEntry.VertexIndex)
    //                     {
    //                         sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
    //                     }
    //                     else
    //                     {
    //                         // The dot product is the cosine of the angle between the two triangles.
    //                         // A larger cosine means a smaller angle.
    //                         var dot = Vector3.Dot(
    //                             triNormals[lhsEntry.MeshIndex][lhsEntry.TriangleIndex],
    //                             triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex]);
    //                         if (dot >= cosineThreshold)
    //                         {
    //                             sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
    //                         }
    //                     }
    //                 }

    //                 normals[lhsEntry.VertexIndex] = sum.Normalized();
    //             }
    //         }

    //         mesh.normals = normals;
    //     }

    //     private struct VertexKey
    //     {
    //         private readonly long _x;
    //         private readonly long _y;
    //         private readonly long _z;

    //         // Change this if you require a different precision.
    //         private const int Tolerance = 100000;

    //         // Magic FNV values. Do not change these.
    //         private const long FNV32Init = 0x811c9dc5;
    //         private const long FNV32Prime = 0x01000193;

    //         public VertexKey(Vector3 position)
    //         {
    //             _x = (long)(MathF.Round(position.X * Tolerance));
    //             _y = (long)(MathF.Round(position.Y * Tolerance));
    //             _z = (long)(MathF.Round(position.Z * Tolerance));
    //         }

    //         public override bool Equals(object obj)
    //         {
    //             var key = (VertexKey)obj;
    //             return _x == key._x && _y == key._y && _z == key._z;
    //         }

    //         public override int GetHashCode()
    //         {
    //             long rv = FNV32Init;
    //             rv ^= _x;
    //             rv *= FNV32Prime;
    //             rv ^= _y;
    //             rv *= FNV32Prime;
    //             rv ^= _z;
    //             rv *= FNV32Prime;

    //             return rv.GetHashCode();
    //         }
    //     }

    //     private struct VertexEntry
    //     {
    //         public int MeshIndex;
    //         public int TriangleIndex;
    //         public int VertexIndex;

    //         public VertexEntry(int meshIndex, int triIndex, int vertIndex)
    //         {
    //             MeshIndex = meshIndex;
    //             TriangleIndex = triIndex;
    //             VertexIndex = vertIndex;
    //         }
    //     }
    // }
}
