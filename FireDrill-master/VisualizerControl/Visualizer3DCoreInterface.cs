using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;
using DongUtility;
using System.Collections.Generic;
using System.IO;
using VisualizerBaseClasses;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Linq;

namespace VisualizerControl
{
    public class Visualizer3DCoreInterface : HwndHost
    {
        internal const int
                    WsChild = 0x40000000,
                    WsVisible = 0x10000000,
                    LbsNotify = 0x00000001,
                    HostId = 0x00000002,
                    ListboxId = 0x00000001,
                    WsVscroll = 0x00200000,
                    WsBorder = 0x00800000;

        public int HostHeight { get; set; }
        public int HostWidth { get; set; }
        private IntPtr hwndHost;

        public Visualizer3DCoreInterface()
        { }

        public Visualizer3DCoreInterface(double windowWidth, double windowHeight)
        {
            SetWindowDimensions(windowWidth, windowHeight);
        }

        public IntPtr HwndListBox { get; private set; }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            HwndListBox = IntPtr.Zero;
            hwndHost = IntPtr.Zero;

            string windowName = "internalWindow";
            //string curDir = Directory.GetCurrentDirectory();
            RegisterWindow(windowName);

            double fourKScaleFactor = 1;
            /*
             * On 4k screens you have to have UI scaling in order for things to not look super tiny.
             * Unfortunately it seems that the internal win32 apps don't get automatically scaled while the wpf does so we have to
             * manually scale here.
             */

            double debugInternalScale = 1;

            hwndHost = CreateWindowEx(0, "static", "",
                WsChild | WsVisible,
                0, 0,
                (int)(HostHeight * fourKScaleFactor), (int)(HostWidth * fourKScaleFactor),
                hwndParent.Handle,
                (IntPtr)HostId,
                IntPtr.Zero,
                0);

            HwndListBox = MakeWindow(windowName,
                WsChild | WsVisible | LbsNotify | WsBorder,
                (int)(HostHeight * fourKScaleFactor * debugInternalScale),
                (int)(HostWidth * fourKScaleFactor * debugInternalScale),
                hwndHost);

            return new HandleRef(this, hwndHost);
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }

        private Dictionary<string, int> shapeCodes = new Dictionary<string, int>();
        private Dictionary<BasicMaterial, int> materialCodes = new Dictionary<BasicMaterial, int>();
        private Dictionary<int, int> externalIndexToInternalIndex = new Dictionary<int, int>();

        internal void AddObject(Object3D obj, int externalIndex)
        {
            // Only add the shape if it is not currently in the dictionary
            var shape = obj.Shape;
            if (!shapeCodes.ContainsKey(shape.ShapeName))
            {
                AddShape(shape);
                // So each shape is added only once
            }

            var material = obj.Material;

            // Why doesn't this work?

            //if (!materialCodes.ContainsKey(material))
            //{
            //    AddMaterial(material);
            //}

            int materialCode = -1;
            foreach (var key in materialCodes.Keys)
            {
                if (key == material)
                {
                    materialCode = materialCodes[key];
                    break;
                }
            }

            if (materialCode == -1)
            {
                materialCode = AddMaterial(material);
            }

            float[] position = ConvertVector(obj.Position);
            float[] scale = ConvertVector(obj.Scale);
            float[] rotation = ConvertMatrix(obj.Rotation.Value);
            int internalIndex = AddObjectX(scale, rotation, position,
                shapeCodes[shape.ShapeName], materialCode);
            externalIndexToInternalIndex.Add(externalIndex, internalIndex);
        }

        private float[] ConvertVector(Vector3D vec)
        {
            float[] response = new float[3];
            response[0] = (float)vec.X;
            response[1] = (float)vec.Y;
            response[2] = (float)vec.Z;
            return response;
        }

        private float[] ConvertMatrix(Matrix3D mat)
        {
            float[] response = new float[16];

            response[0] = (float)mat.M11;
            response[1] = (float)mat.M21;
            response[2] = (float)mat.M31;
            response[3] = (float)mat.OffsetX;
            response[4] = (float)mat.M12;
            response[5] = (float)mat.M22;
            response[6] = (float)mat.M32;
            response[7] = (float)mat.OffsetY;
            response[8] = (float)mat.M13;
            response[9] = (float)mat.M23;
            response[10] = (float)mat.M33;
            response[11] = (float)mat.OffsetZ;
            response[12] = (float)mat.M14;
            response[13] = (float)mat.M24;
            response[14] = (float)mat.M34;
            response[15] = (float)mat.M44;
            return response;
        }

        private int AddMaterial(BasicMaterial material)
        {
            var color = material.Color;
            string name = color.ToString();
            float r = (float)color.R / 255;
            float g = (float)color.G / 255;
            float b = (float)color.B / 255;
            float a = (float)color.A / 255;
            materialCodes.Add(material, materialCodes.Count);
            AddMaterialX(materialCodes[material], name, r, g, b, a, (float)material.Fresnel, (float)material.Roughness);

            return materialCodes[material];
        }

        private void AddShape(Shapes.Shape3D shape)
        {
            var mesh = shape.Mesh;
            int nVertices = mesh.Positions.Count;
            // Vectors are packed into flat arrays
            // So there are 3 * nVertices points in vertices[] and normals[]
            int size = nVertices * 3;
            float[] vertices = new float[size];
            float[] normals = new float[size];
            for (int i = 0; i < nVertices; ++i)
            {
                var vertex = mesh.Positions[i];
                int index = i * 3;
                vertices[index] = (float)(vertex.X);
                vertices[index + 1] = (float)(vertex.Y);
                vertices[index + 2] = (float)(vertex.Z);
                var normal = mesh.Normals[i];
                normals[index] = (float)(normal.X);
                normals[index + 1] = (float)(normal.Y);
                normals[index + 2] = (float)(normal.Z);
            }

            int nTriangleIndices = mesh.TriangleIndices.Count;
            UInt32[] triangles = new UInt32[nTriangleIndices];
            for (int i = 0; i < nTriangleIndices; ++i)
            {
                triangles[i] = (UInt32)(mesh.TriangleIndices[i]);
            }

            shapeCodes.Add(shape.ShapeName, shapeCodes.Count);
            AddShapeX(shapeCodes[shape.ShapeName], nVertices, vertices, normals, nTriangleIndices,
                triangles);
        }

        internal void MoveObject(int externalIndex, Vector3D newPosition)
        {
            int internalIndex = externalIndexToInternalIndex[externalIndex];

            MoveObjectX(internalIndex, ConvertVector(newPosition));
        }

        internal void TransformObject(int externalIndex, Vector3D newScale,
            Matrix3D newRotation, Vector3D newPosition)
        {
            int internalIndex = externalIndexToInternalIndex[externalIndex];
            float[] position = ConvertVector(newPosition);
            float[] scale = ConvertVector(newScale);
            float[] rotation = ConvertMatrix(newRotation);
            TransformObjectX(internalIndex, scale, rotation, position);
        }

        internal void RemoveObject(int externalIndex)
        {
            int internalIndex = externalIndexToInternalIndex[externalIndex];
            RemoveObjectX(internalIndex);
            externalIndexToInternalIndex.Remove(externalIndex);
            // Redo internal indices
            foreach (var key in externalIndexToInternalIndex.Keys.ToList())
            {
                if (externalIndexToInternalIndex[key] > internalIndex)
                    --externalIndexToInternalIndex[key];
            }
        }

        internal void RemoveAllObjects()
        {
            RemoveAllObjectsX();
            externalIndexToInternalIndex.Clear();
        }

        internal void SetAutoCamera(bool value)
        {
            SetAutoCameraX(value);
        }

        internal void MoveCamera(Point3D newPosition)
        {
            float[] position = ConvertVector(WPFUtility.UtilityFunctions.ConvertToVector3D(newPosition));
            MoveCameraX(position);
        }

        internal void MoveCamera(Point3D newPosition, Vector3D newLookDirection, Vector3D newUpDirection)
        {
            float[] position = ConvertVector(WPFUtility.UtilityFunctions.ConvertToVector3D(newPosition));
            float[] lookDirection = ConvertVector(newLookDirection);
            float[] upDirection = ConvertVector(newUpDirection);
            MoveAndTurnCameraX(position, lookDirection, upDirection);
        }

        internal void LookAt(Point3D newPosition, Point3D target, Vector3D newUpDirection)
        {
            float[] position = ConvertVector(WPFUtility.UtilityFunctions.ConvertToVector3D(newPosition));
            float[] newTarget = ConvertVector(WPFUtility.UtilityFunctions.ConvertToVector3D(target));
            float[] upDirection = ConvertVector(newUpDirection);
            LookAtX(position, newTarget, upDirection);
        }

        internal void AdjustLens(double fieldOfViewY, double aspectRatio, double nearZ, double farZ)
        {
            AdjustLensX((float)fieldOfViewY, (float)aspectRatio, (float)nearZ, (float)farZ);
        }

        private const string dllName = @"..\..\..\..\VisualizerControl\Visualizer3DCore.dll";
        //private const string dllName = @"C:\Users\pdong\Documents\Visual Studio Repositories\Computational Science Spring 2021\VisualizerControl\Visualizer3DCore.dll";

        [DllImport(dllName, EntryPoint = "RegisterWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool RegisterWindow(string ClassName);

        [DllImport(dllName, EntryPoint = "MakeWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MakeWindow(string ClassName, int style, int height, int width, IntPtr parent);

        [DllImport(dllName, EntryPoint = "SetupDirectX", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetupDirectX();

        [DllImport(dllName, EntryPoint = "AddShape", CallingConvention = CallingConvention.Cdecl)]
        private static extern void AddShapeX(int index, int nVertices, float[] vertices,
        float[] normals, int nTriangleIndices, UInt32[] triangles);

        [DllImport(dllName, EntryPoint = "AddMaterial", CallingConvention = CallingConvention.Cdecl)]
        private static extern void AddMaterialX(int index, string name, float colorR, 
            float colorG, float colorB, float alpha, float fresnel, float roughness);

        [DllImport(dllName, EntryPoint = "AddObject", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddObjectX(float[] scale, float[] rotation, 
            float[] position, int shape, int material);

        [DllImport(dllName, EntryPoint = "MoveObject", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MoveObjectX(int index, float[] newPosition);

        [DllImport(dllName, EntryPoint = "TransformObject", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TransformObjectX(int index, float[] scale,
            float[] rotation, float[] position);

        [DllImport(dllName, EntryPoint = "RemoveObject", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RemoveObjectX(int index);

        [DllImport(dllName, EntryPoint = "RemoveAllObjects", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RemoveAllObjectsX();

        [DllImport(dllName, EntryPoint = "SetAutoCamera", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetAutoCameraX(bool value);

        [DllImport(dllName, EntryPoint = "MoveCamera", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MoveCameraX(float[] newPosition);

        [DllImport(dllName, EntryPoint = "MoveAndTurnCamera", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MoveAndTurnCameraX(float[] newPosition, float[] lookDirection, float[] upDirection);

        [DllImport(dllName, EntryPoint = "LookAt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void LookAtX(float[] newPosition, float[] target, float[] upDirection);

        [DllImport(dllName, EntryPoint = "AdjustLens", CallingConvention = CallingConvention.Cdecl)]
        private static extern void AdjustLensX(float fieldOfViewY, float aspectRatio, float nearZ, float farZ);

        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateWindowEx(int dwExStyle,
            string lpszClassName,
            string lpszWindowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        private static extern bool DestroyWindow(IntPtr hwnd);

        public void SetWindowDimensions(double windowWidth, double windowHeight)
        {
            HostHeight = (int)windowHeight;
            HostWidth = (int)windowWidth;
        }
    }
}
