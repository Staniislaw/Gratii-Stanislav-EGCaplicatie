

using OpenTK;
using OpenTK.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenTK3_StandardTemplate_WinForms
{
    internal class Alien
    {
        private List<Vector3> Listcoordonate;


        // ...
        private float rotationSpeed = 0.05f; // Adăugați această linie


        private float rotationAngle; // Angle in radians
        /// <summary>
        /// 'Initializari
        /// </summary>

        public Alien()
        {
         
            Listcoordonate = new List<Vector3>();
            //triunghiuri
            Listcoordonate.Add(new Vector3(0, 0, 0));
            Listcoordonate.Add(new Vector3(5, 0, 0));
            Listcoordonate.Add(new Vector3(2.5f, 5, 0));

            Listcoordonate.Add(new Vector3(5, 0, 0));
            Listcoordonate.Add(new Vector3(10, 0, 0));
            Listcoordonate.Add(new Vector3(7.5f, 5, 0));

            Listcoordonate.Add(new Vector3(7.5f, 5, 0));
            Listcoordonate.Add(new Vector3(2.5f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 10, 0));

            //unitatea jos 
            Listcoordonate.Add(new Vector3(3.75f, 5, 0));
            Listcoordonate.Add(new Vector3(6.25f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, 3));

            Listcoordonate.Add(new Vector3(3.75f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(6.25f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, 3));

            Listcoordonate.Add(new Vector3(3.75f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(3.75f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, 3));

            Listcoordonate.Add(new Vector3(6.25f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(6.25f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, 3));

            //unitatea sus
            Listcoordonate.Add(new Vector3(3.75f, 5, 0));
            Listcoordonate.Add(new Vector3(6.25f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, -3));

            Listcoordonate.Add(new Vector3(3.75f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(6.25f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, -3));

            Listcoordonate.Add(new Vector3(3.75f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(3.75f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, -3));

            Listcoordonate.Add(new Vector3(6.25f, 2.5f, 0));
            Listcoordonate.Add(new Vector3(6.25f, 5, 0));
            Listcoordonate.Add(new Vector3(5, 3.75f, -3));

           
            ListTextureCoordinates = new List<Vector2>();
            for (int i = 0; i < Listcoordonate.Count; i++)
            {
                float textureCoordX = Listcoordonate[i].X / 5;
                float textureCoordY = Listcoordonate[i].Y / 5;
                ListTextureCoordinates.Add(new Vector2(textureCoordX, textureCoordY));
            }
        }

        private List<Vector2> ListTextureCoordinates;

        public void DrawAlien()
        {
            GL.PushMatrix();

            float rotationAngleDegrees = MathHelper.RadiansToDegrees(rotationAngle);
            double AlienRotationRadius = 25;
            double Alienspeed = 0.5f;
            double AlienAngle = rotationAngleDegrees * Alienspeed;

            GL.Translate(AlienRotationRadius * Math.Cos(MathHelper.DegreesToRadians(AlienAngle)),
                         0.0,
                         AlienRotationRadius * Math.Sin(MathHelper.DegreesToRadians(AlienAngle)));

            GL.Rotate(rotationAngleDegrees, Vector3.UnitY);

            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.BindTexture(TextureTarget.Texture2D, textures[0]);

            for (int i = 0; i < Listcoordonate.Count; i += 3)
            {
                GL.Color3(GetColor(i / 3));
                GL.Begin(PrimitiveType.Triangles);

                for (int j = i; j < i + 3; j++)
                {
                    GL.TexCoord2(ListTextureCoordinates[j].X, ListTextureCoordinates[j].Y);
                    GL.Vertex3(Listcoordonate[j]);
                }

                GL.End();
            }
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        public void UpdatePosition()
        {
            for (int i = 0; i < Listcoordonate.Count; i++)
            {
                Listcoordonate[i] = new Vector3(Listcoordonate[i].X, Listcoordonate[i].Y + 0.05f, Listcoordonate[i].Z);
            }
        }
        public void UpdateMarime()
        {
            for (int i = 0; i < Listcoordonate.Count; i++)
            {
                Listcoordonate[i] = new Vector3(Listcoordonate[i].X * 10, Listcoordonate[i].Y * 10, Listcoordonate[i].Z * 10);
            }
        }

        public void UpdateRotation()
        {
            // Actualizează unghiul de rotație în grade
            rotationAngle += rotationSpeed;

            // Dacă doriți o rotație continuă, evitați depășirea limitei
            if (rotationAngle >= 360f)
            {
                rotationAngle = 0f;
            }
        }
        public void Update()
        {
            // Calculați unghiul de rotație în jurul umanului
            UpdateRotation();
            UpdatePosition();
        }
        private Color GetColor(int partIndex)
        {
            Color[] colors = { Color.White };
            return colors[partIndex % colors.Length];
        }
        private int[] textures = new int[2];

        public void LoadTextures()
        {
            GL.GenTextures(textures.Length, textures);
            LoadTexture(textures[1], "AlienTexture.png");
            LoadTexture(textures[0], "Alien.jpg");
         
        }

        private void LoadTexture(int textureId, string filename)
        {
            Bitmap bmp = new Bitmap(filename);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
        }

    }
}

