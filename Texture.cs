using static OpenGL.GL;
using System.Drawing;
using System;

namespace RetroShooter
{
    class Texture
    {
        private uint GlID { get; set; }

        public static Texture Load(string path)
        {
            Texture texture = new Texture();
            texture.Load(new Bitmap(path));
            return texture;
        }

        void Load(Bitmap bitmap)
        {

            GlID = glGenTexture();
            Bind();

            byte[] pixels = new byte[4 * bitmap.Width * bitmap.Height];
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    int offset = (y * bitmap.Width + x) * 4;
                    pixels[offset + 0] = c.R;
                    pixels[offset + 1] = c.G;
                    pixels[offset + 2] = c.B;
                    pixels[offset + 3] = c.A;
                }
            }

            unsafe
            {
                fixed(void* pixelsPtr = &pixels[0])
                {
                    glTexImage2D(
                        GL_TEXTURE_2D,
                        0,
                        GL_RGBA,
                        bitmap.Width,
                        bitmap.Height,
                        0,
                        GL_RGBA,
                        GL_UNSIGNED_BYTE,
                        pixelsPtr
                        );
                }
            }
            
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

            glBindTexture(GL_TEXTURE_2D, 0);
        }

        public void Bind()
        {
            glBindTexture(GL_TEXTURE_2D, GlID);
        }
    }
}
