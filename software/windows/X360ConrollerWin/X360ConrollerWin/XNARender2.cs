using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThW.Render;
using ThW.Render.Textures;
using ThW.Utils;
using ThW.XNA.Render;
using ThW.XNA.Textures;

namespace ThW.X360.Controller.Windows
{
    class XNARender2 : ThW.Render.IRender, IDisposable
    {
        public XNARender2(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
        {
            this.graphics = graphics;
            this.graphicsDevice = graphicsDevice;
            this.vertexShaderDeclaration = XNAVertex.Declaration;

            this.blendState2d.ColorDestinationBlend = Blend.InverseSourceAlpha;
            this.blendState2d.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            this.blendState2d.ColorSourceBlend = Blend.SourceAlpha;
            this.blendState2d.AlphaSourceBlend = Blend.SourceAlpha;

            this.depthStencilStateOn.DepthBufferEnable = true;
            this.depthStencilStateOn.DepthBufferFunction = CompareFunction.LessEqual;
            this.depthStencilStateOn.DepthBufferWriteEnable = true;

            this.depthStencilStateOff.DepthBufferEnable = true;
            this.depthStencilStateOff.DepthBufferFunction = CompareFunction.LessEqual;
            this.depthStencilStateOff.DepthBufferWriteEnable = false;

            this.rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            this.rasterizerState.FillMode = FillMode.Solid;

            this.rasterizerStateInverted.CullMode = CullMode.CullClockwiseFace;
            this.rasterizerStateInverted.FillMode = FillMode.Solid;

            this.blendState3d.ColorDestinationBlend = Blend.InverseSourceAlpha;
            this.blendState3d.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            this.blendState3d.ColorSourceBlend = Blend.SourceAlpha;
            this.blendState3d.AlphaSourceBlend = Blend.SourceAlpha;
        }

        public void Dispose()
        {
            this.effect2d.Dispose();
            this.effect3d.Dispose();
            this.blendState2d.Dispose();
            this.blendState3d.Dispose();
            this.vertexShaderDeclaration.Dispose();
            this.depthStencilStateOn.Dispose();
            this.rasterizerState.Dispose();
            this.rasterizerStateInverted.Dispose();
            this.depthStencilStateOff.Dispose();
        }

        public void RenderFace(Face face)
        {
            if ((face.Indices.Length == 0) || (face.tag3 != null && false.Equals(face.tag3)))
            {
                return;
            }

            Matrix old1 = this.effect.World;

            if (null != face.TransformMatrix)
            {
                this.effect.World = (old1 * ToMatrix(face.TransformMatrix.data));
            }

            if (true == face.DynamicVertexes)
            {
                if ((true == face.Texture.Dynamic) && (face.Texture is Shader))
                {
                    RenderFaceWP7(face, (Shader)face.Texture, this.effect, this.graphics.GraphicsDevice, new GameTime());
                }
                else
                {
                    RenderFace(face, graphics.GraphicsDevice, null, null, true);
                }
            }
            else
            {
                GraphicsDevice device = graphics.GraphicsDevice;
                IndexBuffer indexBuffer = null;
                VertexBuffer vertexBuffer = null;

                if (face.tag1 == null)
                {
                    indexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, face.IndicesTriangles.Length, BufferUsage.WriteOnly);
                    indexBuffer.SetData<short>(face.IndicesTriangles);
                    face.tag1 = indexBuffer;
                }
                else
                {
                    indexBuffer = (IndexBuffer)face.tag1;
                }

                if (face.tag2 == null)
                {
                    XNAVertex[] vertexArray = new XNAVertex[face.NumVertexes];

                    for (int i = 0; i < face.NumVertexes; i++)
                    {
                        vertexArray[i].Position.X = face.Vertexes[i].X;
                        vertexArray[i].Position.Y = face.Vertexes[i].Y;
                        vertexArray[i].Position.Z = face.Vertexes[i].Z;
                        vertexArray[i].TextureCoordinate.X = face.Vertexes[i].TexS;
                        vertexArray[i].TextureCoordinate.Y = face.Vertexes[i].TexT;
                        vertexArray[i].TextureCoordinate.Z = 1.0f;
                        vertexArray[i].LmTextureCoordinate.X = face.Vertexes[i].LTexS;
                        vertexArray[i].LmTextureCoordinate.Y = face.Vertexes[i].LTexT;
                        vertexArray[i].Color.R = face.Vertexes[i].R;
                        vertexArray[i].Color.G = face.Vertexes[i].G;
                        vertexArray[i].Color.B = face.Vertexes[i].B;
                        vertexArray[i].Color.A = face.Vertexes[i].A;

                        vertexArray[i].Normal.X = face.Vertexes[i].NX;
                        vertexArray[i].Normal.Y = face.Vertexes[i].NY;
                        vertexArray[i].Normal.Z = face.Vertexes[i].NZ;

                    }

                    vertexBuffer = new VertexBuffer(this.graphicsDevice, this.vertexShaderDeclaration, vertexArray.Length, BufferUsage.WriteOnly);
                    vertexBuffer.SetData(vertexArray);
                    face.tag2 = vertexBuffer;
                }
                else
                {
                    vertexBuffer = (VertexBuffer)face.tag2;
                }

                if (face.Color[3] < 1.0f)
                {
                    this.effect3d.Alpha = face.Color[3];
                }

                if (null != face.tag4)
                {
                    this.graphicsDevice.DepthStencilState = this.depthStencilStateOff;
                }

                RenderFace(face, device, indexBuffer, vertexBuffer, false);

                if (face.Color[3] < 1.0f)
                {
                    this.effect3d.Alpha = 1.0f;
                }

                if (null != face.tag4)
                {
                    this.graphicsDevice.DepthStencilState = this.depthStencilStateOn;
                }
            }

            this.effect.World = old1;
        }

        public void LoadShaders(ContentManager content)
        {
            this.effect2d = new BasicEffect(this.graphicsDevice);
            this.effect3d = new BasicEffect(this.graphicsDevice);

            this.effect2d.VertexColorEnabled = true;
            this.effect2d.TextureEnabled = true;
            this.effect2d.LightingEnabled = true;

            this.effect3d.TextureEnabled = true;
            this.effect3d.LightingEnabled = true;
            this.effect3d.VertexColorEnabled = false;

            this.effect3d.DirectionalLight0.Enabled = true;
            this.effect3d.DirectionalLight0.Direction = new Microsoft.Xna.Framework.Vector3(1.0f, 0.0f, 0.0f);
            this.effect3d.DirectionalLight0.DiffuseColor = new Microsoft.Xna.Framework.Vector3(0.15f, 0.15f, 0.15f);

            this.effect3d.PreferPerPixelLighting = false;
        }

        public void SetMatrixes(Matrix world, Matrix view, Matrix projection)
        {
            SetWorld(world);
            SetView(view);
            SetProjection(projection);
        }

        public void SetMatrixes(float[] world, float[] view, float[] projection)
        {
            SetWorld(ToMatrix(world));
            SetView(ToMatrix(view));
            SetProjection(ToMatrix(projection));
        }

        private void SetProjection(Matrix projection)
        {
            this.effect.Projection = projection;
        }

        private void SetView(Matrix view)
        {
            this.effect.View = view;
        }

        private void SetWorld(Matrix world)
        {
            this.effect.World = world;
        }

        public void LoadProjectionMatrix(float[] p)
        {
        }

        public void LoadModelViewMatrix(float[] p)
        {
        }

        public object CreateTexture(Image pImage, bool bUseMipMap, bool bWrapS, bool bWrapT, TextureMinFilter eMinFilter, TextureMagFilter eMagFilter, bool bCompress, uint nTextureBits)
        {
            return new XNAImage(this.graphics.GraphicsDevice, (int)pImage.Width, (int)pImage.Height, (int)pImage.BitsPerPixel, pImage.Bytes, pImage.FileName).Texture;
        }

        public void SetTextureData(object pTexture, bool useMipMap, uint w, uint h, bool wraps, bool wrapt, int bpp, byte[] p, Image image)
        {
            Texture2D img = (Texture2D)pTexture;

            XNAImage.SetBytes(img, (int)w, (int)h, bpp, image.Bytes);
        }

        public void SetGamma(float level)
        {
        }

        public static Matrix ToMatrix(float[] matrix)
        {
            if (null == matrix)
            {
                return Matrix.Identity;
            }

            Matrix m = new Matrix();

            m.M11 = matrix[0]; m.M21 = matrix[4]; m.M31 = matrix[8]; m.M41 = matrix[12];
            m.M12 = matrix[1]; m.M22 = matrix[5]; m.M32 = matrix[9]; m.M42 = matrix[13];
            m.M13 = matrix[2]; m.M23 = matrix[6]; m.M33 = matrix[10]; m.M43 = matrix[14];
            m.M14 = matrix[3]; m.M24 = matrix[7]; m.M34 = matrix[11]; m.M44 = matrix[15];

            return m;
        }

        private void RenderFace(Face face, GraphicsDevice device, IndexBuffer indexBuffer, VertexBuffer vertexBuffer, bool dynamicGeometry)
        {
            if ((null == face.Texture) || (true == ((Texture2D)face.Texture.TextureId).IsDisposed))
            {
                return;
            }

            this.effect.Texture = ((Texture2D)face.Texture.TextureId);

            this.effect.EmissiveColor = new Microsoft.Xna.Framework.Vector3(face.Color[0], face.Color[1], face.Color[2]);

            if (true == face.ReverseFaces)
            {
                this.graphicsDevice.RasterizerState = this.rasterizerStateInverted;
            }

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                if (true == dynamicGeometry)
                {
                    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, face.Vertexes, 0, face.NumVertexes, face.Indices, 0, face.Indices.Length / 3, this.vertexShaderDeclaration);
                }
                else
                {
                    device.Indices = indexBuffer;
                    device.SetVertexBuffer(vertexBuffer);
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, face.NumVertexes, 0, face.IndicesTriangles.Length / 3);
                }
            }

            if (true == face.ReverseFaces)
            {
                this.graphicsDevice.RasterizerState = this.rasterizerState;
            }
        }

        public void Init2d()
        {
            this.effect = this.effect2d;

            this.graphicsDevice.RasterizerState = this.rasterizerState;
            this.graphicsDevice.DepthStencilState = this.depthStencilStateOn;
            this.graphicsDevice.BlendState = this.blendState2d;

            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateLookAt(new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 0.001f), Microsoft.Xna.Framework.Vector3.Zero, Microsoft.Xna.Framework.Vector3.Up);
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, (float)this.graphicsDevice.Viewport.Width, (float)this.graphicsDevice.Viewport.Height, 0, -10.0f, 1000.0f);

            SetMatrixes(world, view, projection);
        }

        public void Init3d()
        {
            this.effect = this.effect3d;

            this.graphicsDevice.RasterizerState = this.rasterizerState;
            this.graphicsDevice.DepthStencilState = this.depthStencilStateOn;
            this.graphicsDevice.BlendState = this.blendState3d;
        }

        public void Init()
        {
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 480;

#if WINDOWS_PHONE
            this.graphics.IsFullScreen = true;
#endif
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.graphics.PreferMultiSampling = true;
            this.graphics.ApplyChanges();
        }

        private void RenderFaceWP7(Face face, Shader shader, BasicEffect shaderEffect, GraphicsDevice device, GameTime time)
        {
            foreach (ShaderLayer layer in shader.Layers)
            {
                shaderEffect.Texture = (Texture2D)layer.pTexture.TextureId;

                float[,] tcBackup = null;

                if (layer.TextureCoordModification != ShaderTcMod.None)
                {
                    if ((layer.TextureCoordModification & ShaderTcMod.Rotate) > 0)
                    {
                        float angle = MathHelper.ToRadians((float)(layer.TextureCoordRotateAnglePerSecond * ThW.Utils.Timer.Time));

                        Matrix4x4 tc = Matrix4x4.Rotate(new ThW.Utils.Vector3(0.0f, 0.0f, 1.0f), angle);

                        if (null != tcBackup)
                        {
                            tcBackup = new float[2, face.Vertexes.Length];

                            for (int i = 0; i < face.Vertexes.Length; i++)
                            {
                                tcBackup[0, i] = face.Vertexes[i].TexS;
                                tcBackup[1, i] = face.Vertexes[i].TexT;
                            }
                        }

                        for (int i = 0; i < face.Vertexes.Length; i++)
                        {
                            float[] v1 = new float[4];
                            v1[0] = face.Vertexes[i].TexS - 0.5f;
                            v1[1] = face.Vertexes[i].TexT - 0.5f;
                            v1[2] = 0.0f;
                            v1[3] = 1.0f;

                            v1 = tc * v1;

                            face.Vertexes[i].TexS = v1[0] + 0.5f;
                            face.Vertexes[i].TexT = v1[1] + 0.5f;
                        }
                    }
                }

                foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, face.Vertexes, 0, face.NumVertexes, face.Indices, 0, face.Indices.Length / 3, this.vertexShaderDeclaration);
                }

                if (null != tcBackup)
                {
                    for (int i = 0; i < face.Vertexes.Length; i++)
                    {
                        face.Vertexes[i].TexS = tcBackup[0, i];
                        face.Vertexes[i].TexT = tcBackup[1, i];
                    }
                }
            }
        }

        private BasicEffect effect2d = null;
        private BasicEffect effect3d = null;
        private BasicEffect effect = null;
        private BlendState blendState2d = new BlendState();
        private BlendState blendState3d = new BlendState();
        private GraphicsDeviceManager graphics = null;
        private GraphicsDevice graphicsDevice = null;
        private VertexDeclaration vertexShaderDeclaration = null;
        private DepthStencilState depthStencilStateOn = new DepthStencilState();
        private RasterizerState rasterizerState = new RasterizerState();
        private RasterizerState rasterizerStateInverted = new RasterizerState();
        private DepthStencilState depthStencilStateOff = new DepthStencilState();
    }
}
