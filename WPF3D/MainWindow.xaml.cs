using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Runtime.InteropServices;

using System.Windows.Media.Media3D;
using System.Diagnostics;



namespace WPF3D
{
	internal sealed class Win32
	{
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public static void HideConsoleWindow()
		{
			IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

			if (hWnd != IntPtr.Zero)
			{
				ShowWindow(hWnd, 0); // 0 = SW_HIDE
			}
		}
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		PerspectiveCamera camera;

		public MainWindow()
		{
			InitializeComponent();

			

			test2();
		}



		void test2()
		{
			// Defines the camera used to view the 3D object. In order to view the 3D object, 
			// the camera must be positioned and pointed such that the object is within view  
			// of the camera.
			camera = new PerspectiveCamera();

			// Specify where in the 3D scene the camera is.
			camera.Position = new Point3D(-10, -10, 10);

			// Specify the direction that the camera is pointing.
			camera.LookDirection = new Vector3D(1, 1, -1);

			camera.UpDirection = new Vector3D(0, 0, 1);
		

			// Define camera's horizontal field of view in degrees.
			camera.FieldOfView = 80;

			// Asign the camera to the viewport
			myViewport3D.Camera = camera;

			myViewport3D.Children.Add(new ModelVisual3D() { Content = new AmbientLight(Colors.White) });


			drawTriangle(new Point3D(0, 0, 0), new Point3D(1, 0, 0), new Point3D(0, 1, 0));
			drawRectangle(new Point3D(-1, -1, 0), new Point3D(1, -1, 0), new Point3D(1, 1, 0), new Point3D(-1, 1, 0));

	
			


			double lineThickness = 0.04;
			double skyHeight = 50;
			for (int x = -100; x <= 100; x++)
			{
				drawRectangle(new Point3D(x - lineThickness, -100, 0), new Point3D(x + lineThickness, -100, 0), new Point3D(x + lineThickness, 100, 0), new Point3D(x - lineThickness, 100, 0), new SolidColorBrush(Colors.Aqua));
				drawRectangle(new Point3D(x + lineThickness, -100, skyHeight), new Point3D(x - lineThickness, -100, skyHeight), new Point3D(x - lineThickness, 100, skyHeight), new Point3D(x + lineThickness, 100, skyHeight), new SolidColorBrush(Colors.Aqua));
			}

			for (int y = -100; y <= 100; y++)
			{
				drawRectangle(new Point3D(-100, y + lineThickness, 0), new Point3D(-100, y - lineThickness, 0), new Point3D(100, y - lineThickness, 0), new Point3D(100, y + lineThickness, 0), new SolidColorBrush(Colors.LightGreen));
				drawRectangle(new Point3D(-100, y - lineThickness, skyHeight), new Point3D(-100, y + lineThickness, skyHeight), new Point3D(100, y + lineThickness, skyHeight), new Point3D(100, y - lineThickness, skyHeight), new SolidColorBrush(Colors.LightGreen));
			}

			ImageBrush imageBrushH = new ImageBrush();
			ImageBrush imageBrushV = new ImageBrush();
			ImageBrush imageBrushT = new ImageBrush();

			imageBrushH.ImageSource = new BitmapImage(new Uri("h.png", UriKind.RelativeOrAbsolute));
			imageBrushV.ImageSource = new BitmapImage(new Uri("v.png", UriKind.RelativeOrAbsolute));
			imageBrushT.ImageSource = new BitmapImage(new Uri("t.png", UriKind.RelativeOrAbsolute));

			Point3D p0 = new Point3D(0, 1, 1);
			Point3D p1 = new Point3D(0, 1, 0);
			Point3D p2 = new Point3D(0, 0, 1);
			Point3D p3 = new Point3D(0, 0, 0);
			Point3D p4 = new Point3D(1, 0, 1);
			Point3D p5 = new Point3D(1, 0, 0);
			Point3D p6 = new Point3D(1, 1, 1);

			drawRectangleTexture(p0, p2, p3, p1, imageBrushH);
			drawRectangleTexture(p2, p4, p5, p3, imageBrushV);
			drawRectangleTexture(p6, p4, p2, p0, imageBrushT);
		}


	
		

		void drawTriangle(Point3D p0, Point3D p1, Point3D p2, Brush brush = null)
		{
			MeshGeometry3D triangleMesh = new MeshGeometry3D();

			triangleMesh.Positions.Add(p0);
			triangleMesh.Positions.Add(p1);
			triangleMesh.Positions.Add(p2);

			int n0 = 0;
			int n1 = 1;
			int n2 = 2;

			triangleMesh.TriangleIndices.Add(n0);
			triangleMesh.TriangleIndices.Add(n1);
			triangleMesh.TriangleIndices.Add(n2);


			if (brush == null)
			{
				brush = new SolidColorBrush(Colors.AliceBlue);
			}
			System.Windows.Media.Media3D.Material frontMaterial =
			new DiffuseMaterial(brush);

			System.Windows.Media.Media3D.GeometryModel3D triangleModel =
			new GeometryModel3D(triangleMesh, frontMaterial);

			//triangleModel.Transform = new Transform3DGroup();

			System.Windows.Media.Media3D.ModelVisual3D visualModel = new ModelVisual3D();
			visualModel.Content = triangleModel;

			myViewport3D.Children.Add(visualModel);
		}

		void drawRectangle(Point3D p0, Point3D p1, Point3D p2, Point3D p3, Brush brush = null)
		{
			drawTriangle(p0, p1, p2, brush);
			drawTriangle(p2, p3, p0, brush);
		}



		void drawTriangleRT(Point3D p0, Point3D p1, Point3D p2, ImageBrush imageBrush)
		{
			MeshGeometry3D triangleMesh = new MeshGeometry3D();

			triangleMesh.Positions.Add(p0);
			triangleMesh.Positions.Add(p1);
			triangleMesh.Positions.Add(p2);

			triangleMesh.TextureCoordinates.Add(new Point(0, 0));
			triangleMesh.TextureCoordinates.Add(new Point(1, 0));
			triangleMesh.TextureCoordinates.Add(new Point(1, 1));

			int n0 = 0;
			int n1 = 1;
			int n2 = 2;

			triangleMesh.TriangleIndices.Add(n0);
			triangleMesh.TriangleIndices.Add(n1);
			triangleMesh.TriangleIndices.Add(n2);



			System.Windows.Media.Media3D.Material frontMaterial =
			new DiffuseMaterial(imageBrush);

			System.Windows.Media.Media3D.GeometryModel3D triangleModel =
			new GeometryModel3D(triangleMesh, frontMaterial);

			triangleModel.BackMaterial = frontMaterial;

			System.Windows.Media.Media3D.ModelVisual3D visualModel = new ModelVisual3D();
			visualModel.Content = triangleModel;

			myViewport3D.Children.Add(visualModel);
		}

		void drawTriangleLB(Point3D p0, Point3D p1, Point3D p2, ImageBrush imageBrush)
		{
			MeshGeometry3D triangleMesh = new MeshGeometry3D();

			triangleMesh.Positions.Add(p0);
			triangleMesh.Positions.Add(p1);
			triangleMesh.Positions.Add(p2);

			triangleMesh.TextureCoordinates.Add(new Point(1, 1));
			triangleMesh.TextureCoordinates.Add(new Point(0, 1));
			triangleMesh.TextureCoordinates.Add(new Point(0, 0));

			int n0 = 0;
			int n1 = 1;
			int n2 = 2;

			triangleMesh.TriangleIndices.Add(n0);
			triangleMesh.TriangleIndices.Add(n1);
			triangleMesh.TriangleIndices.Add(n2);



			System.Windows.Media.Media3D.Material frontMaterial =
			new DiffuseMaterial(imageBrush);

			System.Windows.Media.Media3D.GeometryModel3D triangleModel =
			new GeometryModel3D(triangleMesh, frontMaterial);

			triangleModel.BackMaterial = frontMaterial;

			System.Windows.Media.Media3D.ModelVisual3D visualModel = new ModelVisual3D();
			visualModel.Content = triangleModel;

			myViewport3D.Children.Add(visualModel);
		}


		void drawRectangleTexture(Point3D p0, Point3D p1, Point3D p2, Point3D p3, ImageBrush imageBrush)
		{
			drawTriangleRT(p0, p1, p2, imageBrush);
			drawTriangleLB(p2, p3, p0, imageBrush);

			
			//MeshGeometry3D mesh = new MeshGeometry3D();

			//mesh.Positions.Add(p0);
			//mesh.Positions.Add(p1);
			//mesh.Positions.Add(p2);
			//mesh.Positions.Add(p3);

			//mesh.TriangleIndices.Add(0);
			//mesh.TriangleIndices.Add(1);
			//mesh.TriangleIndices.Add(2);

			//mesh.TriangleIndices.Add(2);
			//mesh.TriangleIndices.Add(3);
			//mesh.TriangleIndices.Add(0);


			//mesh.TextureCoordinates.Add(new Point(0, 0));
			//mesh.TextureCoordinates.Add(new Point(0, 1));
			//mesh.TextureCoordinates.Add(new Point(1, 1));
			//mesh.TextureCoordinates.Add(new Point(1, 0));



			////GeometryModel3D geometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Green));  // this line works without the TextureCoordinates

			//GeometryModel3D geometry = new GeometryModel3D(mesh, new DiffuseMaterial(imageBrush));

			//geometry.Transform = new Transform3DGroup();

			//System.Windows.Media.Media3D.ModelVisual3D visualModel = new ModelVisual3D();
			//visualModel.Content = geometry;

			//myViewport3D.Children.Add(visualModel);
		}
	

		private void myViewport3D_KeyDown(object sender, KeyEventArgs e)
		{
			
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.A)
			{
				cameraLeft(0.2);
			}
			else if (e.Key == Key.W)
			{
				cameraForward(0.2);
			}
			else if (e.Key == Key.S)
			{
				cameraForward(-0.2);
			}
			else if (e.Key == Key.D)
			{
				cameraLeft(-0.2);
			}
			else if (e.Key == Key.Q)
			{
				camera.FieldOfView += 2;
				updateCameraInfo();
			}
			else if (e.Key == Key.E)
			{
				camera.FieldOfView -= 2;
				updateCameraInfo();
			}
			else if (e.Key == Key.Space)
			{
				mouseMove = !mouseMove;
				bef = new Point(-1, -1);

				if (mouseMove)
				{
					this.Cursor = Cursors.None;
				}
				else
				{
					this.Cursor = Cursors.Arrow;
				}
			}

			
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{

		}


		bool mouseMove = false;

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			mouseMove = !mouseMove;
			bef = new Point(-1, -1);

			if (mouseMove)
			{
				this.Cursor = Cursors.None;
			}
			else
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{

		}

		Point bef = new Point(-1, -1);

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMove == true)
			{

				Point windowCenter = new Point(this.Width / 2, this.Height / 2);
				Point screenCenter = this.PointToScreen(windowCenter);
				System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)screenCenter.X, (int)screenCenter.Y);

				Point cur = e.GetPosition(this);

				if (bef.X != -1)
				{
					if (cur != windowCenter)
					{
						double dx = bef.X - cur.X;
						double dy = bef.Y - cur.Y;

						Console.WriteLine(cur);

						double dPhi = dx * 0.003;
						double dTheta = dy * 0.003;

						cameraPan(dPhi);
						cameraTilt(dTheta);
					}

					
				}

				bef = windowCenter;
				bef = e.GetPosition(myViewport3D);
			}
		}

		private void cameraTilt(double offset)
		{
			Vector3D vec = camera.LookDirection;

			double x = vec.X;
			double y = vec.Y;
			double z = vec.Z;

			double nuTilt = Math.Asin(z / 1) + offset;

			if (nuTilt > Math.PI / 2)
			{
				nuTilt = Math.PI / 2;
			}
			else if (nuTilt < -Math.PI / 2)
			{
				nuTilt = -Math.PI  / 2;
			}

			
			double nz = Math.Sqrt(x * x + y * y) * Math.Tan(nuTilt);

			double nx = x;
			double ny = y;

			double norm = Math.Sqrt(nx * nx + ny * ny + nz * nz);

			nx /= norm;
			ny /= norm;
			nz /= norm;

			camera.LookDirection = new Vector3D(nx, ny, nz);


			updateCameraInfo();
		}

		private void cameraPan(double pan)
		{
			Vector3D vec = camera.LookDirection;

			Vector3D mat1 = new Vector3D(Math.Cos(pan), -Math.Sin(pan), 0);
			Vector3D mat2 = new Vector3D(Math.Sin(pan), Math.Cos(pan), 0);
			Vector3D mat3 = new Vector3D(0, 0, 1);


			double nx = Vector3D.DotProduct(vec, mat1);
			double ny = Vector3D.DotProduct(vec, mat2);
			double nz = Vector3D.DotProduct(vec, mat3);

			double norm = Math.Sqrt(nx * nx + ny * ny + nz * nz);
			nx /= norm;
			ny /= norm;
			nz /= norm;

			camera.LookDirection = new Vector3D(nx, ny, nz);

			updateCameraInfo();
		}

		private void cameraForward(double dist)
		{
			camera.Position += camera.LookDirection * dist;

			updateCameraInfo();
		}

		private void cameraLeft(double dist)
		{
			Vector3D vec = camera.LookDirection;
			Vector3D top = new Vector3D(0, 0, 1);
			Vector3D mov = Vector3D.CrossProduct(top, vec);
			mov = Vector3D.Divide(mov, mov.Length);

			mov = Vector3D.Multiply(mov, dist);

			camera.Position += mov;

			Console.WriteLine(mov);

			updateCameraInfo();
		}

		private void updateCameraInfo()
		{
			textBlockCameraLookDirection.Text = camera.LookDirection.ToString();
			textBlockCameraPosition.Text = camera.Position.ToString();
			textBlockCameraFieldOfView.Text = camera.FieldOfView.ToString();
		}

		
	}
}
