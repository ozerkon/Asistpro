using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SgkAssistant.Helpers
{
	/// <summary>
	/// Summary description for ImageMap.
	/// </summary>
	[ToolboxBitmap(typeof(ImageMap))]
	public class ImageMap : System.Windows.Forms.UserControl
	{
		private System.Drawing.Drawing2D.GraphicsPath pathData;
		private int activeIndex = -1;
		private ArrayList pathsArray;
		private ToolTip toolTip;
		private Graphics graphics;

		private System.Windows.Forms.PictureBox pictureBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public delegate void RegionClickDelegate(int index, string key);
		[Category("Action")]
		public event RegionClickDelegate RegionClick;

		public ImageMap()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			this.pathsArray = new ArrayList();
			this.pathData = new System.Drawing.Drawing2D.GraphicsPath();
			this.pathData.FillMode = System.Drawing.Drawing2D.FillMode.Winding;

			this.components = new Container();
			this.toolTip = new ToolTip(this.components);
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 1000;
			this.toolTip.ReshowDelay = 500;

			this.graphics = Graphics.FromHwnd(this.pictureBox.Handle);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.InitialImage = null;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            // 
            // ImageMap
            // 
            this.Controls.Add(this.pictureBox);
            this.Name = "ImageMap";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		[Category("Appearance")]
		public Image Image
		{
			get
			{
				return this.pictureBox.Image;
			}
			set
			{
				this.pictureBox.Image = value;
			}
		}

		public int AddElipse(string key, Point center, int radius)
		{
			return this.AddElipse(key, center.X, center.Y, radius);
		}

		public int AddElipse(string key, int x, int y, int radius)
		{
			if (this.pathsArray.Count > 0)
				this.pathData.SetMarkers();
			this.pathData.AddEllipse(x - radius, y - radius, radius * 2, radius * 2);
			return this.pathsArray.Add(key);
		}

		public int AddRectangle(string key, int x1, int y1, int x2, int y2)
		{
			return this.AddRectangle(key, new Rectangle(x1, y1, (x2 - x1), (y2 - y1)));
		}

		public int AddRectangle(string key, Rectangle rectangle)
		{
			if (this.pathsArray.Count > 0)
				this.pathData.SetMarkers();
			this.pathData.AddRectangle(rectangle);
			return this.pathsArray.Add(key);
		}

		public int AddPolygon(string key, Point[] points)
		{
			if (this.pathsArray.Count > 0)
				this.pathData.SetMarkers();
			this.pathData.AddPolygon(points);
			return this.pathsArray.Add(key);
		}

		private void pictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int newIndex = this.GetActiveIndexAtPoint(new Point(e.X, e.Y));
			if (newIndex > -1)
			{
				pictureBox.Cursor = Cursors.Hand;
				if (this.activeIndex != newIndex)
					this.toolTip.SetToolTip(this.pictureBox, this.pathsArray[newIndex].ToString());
			}
			else
			{
				pictureBox.Cursor = Cursors.Default;
				this.toolTip.RemoveAll();
			}
			this.activeIndex = newIndex;
		}

		private void pictureBox_MouseLeave(object sender, System.EventArgs e)
		{
			this.activeIndex = -1;
			this.Cursor = Cursors.Default;
		}

		private void pictureBox_Click(object sender, System.EventArgs e)
		{
			Point p = this.PointToClient(Cursor.Position);
			if (this.activeIndex == -1)
				this.GetActiveIndexAtPoint(p);
			if (this.activeIndex > -1 && this.RegionClick != null)
				this.RegionClick(this.activeIndex, this.pathsArray[this.activeIndex].ToString());
		}

		private int GetActiveIndexAtPoint(Point point)
		{
			System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
			System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(pathData);
			iterator.Rewind();
			for (int current = 0; current < iterator.SubpathCount; current++)
			{
				iterator.NextMarker(path);
				if (path.IsVisible(point, this.graphics))
					return current;
			}
			return -1;
		}

		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}
	}
}
