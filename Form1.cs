using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;

using OpenTK3_StandardTemplate_WinForms.helpers;
using OpenTK3_StandardTemplate_WinForms.objects;

namespace OpenTK3_StandardTemplate_WinForms
{
    public partial class MainForm : Form
    {
        private Axes mainAxis;
        private Rectangles re;
        private Camera cam;
        private Scene scene;

        private Point mousePosition;
       
        public MainForm()
        {
            // general init
            InitializeComponent();
            
            // init VIEWPORT
            scene = new Scene();

            scene.GetViewport().Load += new EventHandler(this.mainViewport_Load);
            scene.GetViewport().Paint += new PaintEventHandler(this.mainViewport_Paint);
            scene.GetViewport().MouseMove += new MouseEventHandler(this.mainViewport_MouseMove);
            

            this.Controls.Add(scene.GetViewport());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // init RNG
            Randomizer.Init();

            // init CAMERA/EYE
            cam = new Camera(scene.GetViewport());

            // init AXES
            mainAxis = new Axes(showAxes.Checked);
            re = new Rectangles();
          
        }
      
        private void showAxes_CheckedChanged(object sender, EventArgs e)
        {
            mainAxis.SetVisibility(showAxes.Checked);

            scene.Invalidate();
        }

        private void changeBackground_Click(object sender, EventArgs e)
        {
            GL.ClearColor(Randomizer.GetRandomColor());

            scene.Invalidate();
        }

        private void resetScene_Click(object sender, EventArgs e)
        {
            showAxes.Checked = true;
            mainAxis.SetVisibility(showAxes.Checked);
            scene.Reset();
            cam.Reset();

            scene.Invalidate();
        }

        private void mainViewport_Load(object sender, EventArgs e)
        {
            InitializeTextures();
            scene.Reset();
        }

        private void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = new Point(e.X, e.Y);
            scene.Invalidate();
        }
        private float elapsedTime = 0.0f;
        private Human human=new Human();
        private Alien alien = new Alien();
        private void mainViewport_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            scene.GetViewport().MakeCurrent();
            cam.SetView();

            if (enableRotation.Checked == true)
            {
                // Rotate the scene based on elapsed time
                GL.Rotate(elapsedTime * 30.0f, 1, 1, 1); // Adjust the rotation speed (30.0f is just an example)
            }

            // GRAPHICS PAYLOAD
            mainAxis.Draw();

            human.DrawHman();
            alien.DrawAlien();

            if (enableObjectRotation.Checked == true)
            {
                // Rotate the object based on elapsed time
                GL.PushMatrix();
                GL.Rotate(elapsedTime * 50.0f, 1, 1, 1); // Adjust the rotation speed (50.0f is just an example)
                re.Draw();
                GL.PopMatrix();
            }
            else
            {
                re.Draw();
            }
            if (chxRotare.Checked == true)
            {
              
            }
            scene.GetViewport().SwapBuffers();

            // Update the elapsed time for the next frame
            elapsedTime += 0.016f; // Assuming a frame time of 16 milliseconds (60 FPS), adjust accordingly
        }

        private void enableRotation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void enableObjectRotation_CheckedChanged(object sender, EventArgs e)
        {

        }
        private bool automaticRotation = false;
        private void chxRotare_CheckedChanged(object sender, EventArgs e)
        {
            automaticRotation = chxRotare.Checked;

            if (automaticRotation)
            {
                StartAutomaticRotation();
            }
        }
        private void StartAutomaticRotation()
        {
        
            Timer rotationTimer = new Timer();
            rotationTimer.Interval = 16;//60fps
            rotationTimer.Tick += (s, e) =>
            {
                // Actualizați rotația pentru obiectele dorite (alien și om)
                float rotationSpeed = 2.0f; // Puteți ajusta viteza de rotație
                alien.Update();
                human.UpdatePosition();

                // Redesenați scena
                scene.Invalidate();
            };

            rotationTimer.Start();
        }

        private void btnMarime_Click(object sender, EventArgs e)
        {
            alien.UpdateMarime();
            human.UpdateMarime();
        }
        private void InitializeTextures()
        {
            alien.LoadTextures();
            human.LoadTextures();
        }
    }

}
