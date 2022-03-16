using System;
using System.Collections.Generic;
using System.Text;
using static OpenGL.GL;

namespace RetroShooter
{
    class Graphics
    {
        public static unsafe uint CreateShaderProgram(uint[] shaders)
        {
            uint program = glCreateProgram();
            foreach (uint shader in shaders)
            {
                glAttachShader(program, shader);
            }
            
            glLinkProgram(program);

            int status;
            glGetProgramiv(program, GL_LINK_STATUS, &status);
            if (status != GL_TRUE)
            {
                throw new System.Exception(
                    "GLSL Link Error: " + glGetProgramInfoLog(program));
            }

            glGetProgramiv(program, GL_VALIDATE_STATUS, &status);
            if (status != GL_TRUE)
            {
                throw new System.Exception(
                    "GLSL Link Error: " + glGetProgramInfoLog(program));
            }
            
            return program;
        }

        public static unsafe uint CreateShader(int type, string source)
        {
            uint shader = glCreateShader(type);
            glShaderSource(shader, source);
            glCompileShader(shader);

            // Checking to see if the compilation failed.
            int status;
            glGetShaderiv(shader, GL_COMPILE_STATUS, &status);
            if (status != GL_TRUE)
            {
                throw new System.Exception(
                    "GLSL Compilation Error: " + glGetShaderInfoLog(shader));
            }
            
            return shader;
        }
    }
}
