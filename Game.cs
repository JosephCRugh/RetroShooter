using System;
using GLFW;
using static OpenGL.GL;

namespace RetroShooter {
    class Game {

        private static readonly String WINDOW_TITLE = "My Window";

        private static readonly int WINDOW_WIDTH  = 500;
        private static readonly int WINDOW_HEIGHT = 500;

        private static QuadBatch batch = new QuadBatch();

        private static uint programId;

        private static Window CreateWindow()
        {
            if (!Glfw.Init())
            {
                throw new System.Exception("Failed to initialize GLFW");
            }

            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Doublebuffer, true);
            
            Window window = Glfw.CreateWindow(
                WINDOW_WIDTH,
                WINDOW_HEIGHT,
                WINDOW_TITLE,
                GLFW.Monitor.None,
                Window.None);

            if (window == null)
            {
                Glfw.Terminate();
                throw new System.Exception("Failed to create GLFW window");
            }

            // Centering the window
            var screen = Glfw.PrimaryMonitor.WorkArea;
            int x = (screen.Width - WINDOW_WIDTH) / 2;
            int y = (screen.Height - WINDOW_HEIGHT) / 2;
            Glfw.SetWindowPosition(window, x, y);

            // Enable OpenGL for the window
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);

            return window;
        }
        

        static void Main(string[] args)
        {

            Window window = CreateWindow();

            uint vertex = Graphics.CreateShader(GL_VERTEX_SHADER, System.IO.File.ReadAllText("Resources/vertex.glsl"));
            uint fragment = Graphics.CreateShader(GL_FRAGMENT_SHADER, System.IO.File.ReadAllText("Resources/fragment.glsl"));
            programId = Graphics.CreateShaderProgram(new uint[] { vertex, fragment });
            glUseProgram(programId);

            batch.Setup();

            Texture testTexture = Texture.Load("Resources/test.png");

            glEnable(GL_BLEND);
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents();

                glClear(GL_COLOR_BUFFER_BIT);
                glClearColor(0.5f, 0.5f, 0.5f, 1);

                batch.AddQuad(0, 0, 1, 1, 0, 1, 0, 1, testTexture);
                batch.Render();

                Glfw.SwapBuffers(window);

            }

            Glfw.Terminate();
        }
    }
}
