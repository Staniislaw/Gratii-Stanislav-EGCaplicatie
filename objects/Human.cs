using OpenTK;
using OpenTK.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenTK3_StandardTemplate_WinForms
{

    internal class Human
    {

        private List<Vector3> Listcoordonate;
        private const float Gravity_Offset =0.05f;

        /// <summary>
        /// 'Initializari
        /// </summary>
        private List<Vector2> ListTextureCoordinates;
        public Human()
        {


            Listcoordonate = new List<Vector3>();
            //corpul rachetei
            Listcoordonate.Add(new Vector3(0, 0, 0));
            Listcoordonate.Add(new Vector3(7, 0, 0));
            Listcoordonate.Add(new Vector3(7, 14, 0));
            Listcoordonate.Add(new Vector3(0, 14, 0));


            Listcoordonate.Add(new Vector3(7, 0, 0));
            Listcoordonate.Add(new Vector3(7, 0, -5));
            Listcoordonate.Add(new Vector3(7, 14, -5));
            Listcoordonate.Add(new Vector3(7, 14, 0));

            Listcoordonate.Add(new Vector3(0, 0, 0));
            Listcoordonate.Add(new Vector3(0, 0, -5));
            Listcoordonate.Add(new Vector3(0, 14, -5));
            Listcoordonate.Add(new Vector3(0, 14, 0));

            Listcoordonate.Add(new Vector3(0, 0, -5));
            Listcoordonate.Add(new Vector3(0, 14, -5));
            Listcoordonate.Add(new Vector3(7, 14, -5));
            Listcoordonate.Add(new Vector3(7, 0, -5));

            Listcoordonate.Add(new Vector3(0, 14, 0));
            Listcoordonate.Add(new Vector3(0, 14, -5));
            Listcoordonate.Add(new Vector3(7, 14, -5));
            Listcoordonate.Add(new Vector3(7, 14, 0));

            //varful rachetei
            Listcoordonate.Add(new Vector3(0, 14, 0));
            Listcoordonate.Add(new Vector3(7, 14, 0));
            Listcoordonate.Add(new Vector3(3.5f, 20, -2.5f));
            Listcoordonate.Add(new Vector3(0, 14, 0));

            Listcoordonate.Add(new Vector3(7, 14, 0));
            Listcoordonate.Add(new Vector3(7, 14, -5));
            Listcoordonate.Add(new Vector3(3.5f, 20, -2.5f));
            Listcoordonate.Add(new Vector3(7, 14, 0));


            Listcoordonate.Add(new Vector3(7, 14, -5));
            Listcoordonate.Add(new Vector3(0, 14, -5));
            Listcoordonate.Add(new Vector3(3.5f, 20, -2.5f));
            Listcoordonate.Add(new Vector3(7, 14, -5));

            Listcoordonate.Add(new Vector3(0, 14, -5));
            Listcoordonate.Add(new Vector3(0, 14, 0));
            Listcoordonate.Add(new Vector3(3.5f, 20, -2.5f));
            Listcoordonate.Add(new Vector3(0, 14, -5));

            //arip[ile
            Listcoordonate.Add(new Vector3(-5, 0, -2.5f));
            Listcoordonate.Add(new Vector3(0, 0, -2.5f));
            Listcoordonate.Add(new Vector3(0, 14, -2.5f));
            Listcoordonate.Add(new Vector3(-5, 0, -2.5f));

            Listcoordonate.Add(new Vector3(7, 0, -2.5f));
            Listcoordonate.Add(new Vector3(12, 0, -2.5f));
            Listcoordonate.Add(new Vector3(7, 14, -2.5f));
            Listcoordonate.Add(new Vector3(7, 0, -2.5f));
            ListTextureCoordinates = new List<Vector2>();
            for (int i = 0; i < Listcoordonate.Count; i++)
            {
                float textureCoordX = Listcoordonate[i].X / 5;
                float textureCoordY = Listcoordonate[i].Y / 5;
                ListTextureCoordinates.Add(new Vector2(textureCoordX, textureCoordY));
            }
        }
        public void UpdatePosition()
        {
            for (int i = 0; i < Listcoordonate.Count; i++)
            {
                Listcoordonate[i] = new Vector3(Listcoordonate[i].X, Listcoordonate[i].Y + Gravity_Offset, Listcoordonate[i].Z);
            }
        }
        public void UpdateMarime()
        {
            for (int i = 0; i < Listcoordonate.Count; i++)
            {
                Listcoordonate[i] = new Vector3(Listcoordonate[i].X*3, Listcoordonate[i].Y*3, Listcoordonate[i].Z*3);
            }
        }

        public void DrawHman()
        {
          
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.BindTexture(TextureTarget.Texture2D, textures[0]); // Folosiți indexul corect pentru textură

            for (int i = 0; i < Listcoordonate.Count; i += 4)
            {
                GL.Color3(GetColor(i / 4));
                GL.Begin(PrimitiveType.Quads);

                for (int j = i; j < i + 4; j++)
                {
                    GL.TexCoord2(ListTextureCoordinates[j].X, ListTextureCoordinates[j].Y);
                    GL.Vertex3(Listcoordonate[j]);
                }

                GL.End();
            }

            GL.Disable(EnableCap.Texture2D);
           
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
            LoadTexture(textures[0], "Avion.jpg");

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
