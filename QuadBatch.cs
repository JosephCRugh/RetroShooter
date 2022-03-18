using static OpenGL.GL;
using System.Collections.Generic;

namespace RetroShooter
{
    class QuadBatch
    {
        private uint vao;
        private uint vbo;

        private static readonly int ATTRIBUTE_STRIDE = 4;

        private float[] vBuffer;
        private int vPtr;

        private class Vertex
        {
            public float x, y;
            public float u, v;
        }

        private class Quad
        {
            public Vertex v1 = new Vertex();
            public Vertex v2 = new Vertex();
            public Vertex v3 = new Vertex();
            public Vertex v4 = new Vertex();
            public Texture texture;
        }

        List<Quad> quads = new List<Quad>();

        public void AddQuad(float x, float y, float width, float height,
                     float u0, float u1, float v0, float v1,
                     Texture texture)
        {
            Quad quad = new Quad();
            quad.v1.x = x;
            quad.v1.y = y;
            quad.v1.u = 0;
            quad.v1.v = 0;

            quad.v2.x = x + width;
            quad.v2.y = y;
            quad.v2.u = 1;
            quad.v2.v = 0;

            quad.v3.x = x;
            quad.v3.y = y + height;
            quad.v3.u = 0;
            quad.v3.v = 1;

            quad.v4.x = x + width;
            quad.v4.y = y + height;
            quad.v4.u = 1;
            quad.v4.v = 1;

            quad.texture = texture;

            quads.Add(quad);
        }

        public void Setup()
        {
            vao = glGenVertexArray();
            glBindVertexArray(vao);

            vbo = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            unsafe
            {
                glVertexAttribPointer(0, 2, GL_FLOAT, false, ATTRIBUTE_STRIDE * sizeof(float), null);
                glVertexAttribPointer(1, 2, GL_FLOAT, false, ATTRIBUTE_STRIDE * sizeof(float), (void*)(2 * sizeof(float)));
            }
        }

        public void Render()
        {
            vBuffer = new float[ATTRIBUTE_STRIDE * 6 * quads.Count];
            vPtr = 0;

            foreach (Quad quad in quads)
            {
                BufferQuad(quad);
            }

            unsafe
            {
                fixed (void* uploadData = &vBuffer[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vBuffer.Length, uploadData, GL_STATIC_DRAW);
                }
            }

            glActiveTexture(GL_TEXTURE0);

            int quadCount = 0;
            foreach (Quad quad in quads)
            {
                quad.texture.Bind();

                glDrawArrays(GL_TRIANGLES, quadCount*6, 6);
                ++quadCount;
            }

            quads.Clear();
        }

        private void BufferQuad(Quad quad)
        {
            BufferVertex(quad.v1);
            BufferVertex(quad.v2);
            BufferVertex(quad.v4);
            BufferVertex(quad.v1);
            BufferVertex(quad.v4);
            BufferVertex(quad.v3);
        }

        private void BufferVertex(Vertex vertex)
        {
            vBuffer[vPtr++] = vertex.x;
            vBuffer[vPtr++] = vertex.y;
            vBuffer[vPtr++] = vertex.u;
            vBuffer[vPtr++] = vertex.v;
        }
    }
}
