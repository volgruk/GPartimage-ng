
// This file has been generated by the GUI designer. Do not modify.
namespace gpartimageng
{
	public partial class AboutWindow
	{
		private global::Gtk.Fixed fixed2;
		private global::Gtk.Label label1;
		private global::Gtk.Label label2;
		private global::Gtk.Label label3;
		private global::Gtk.Entry entry1;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget gpartimageng.AboutWindow
			this.Name = "gpartimageng.AboutWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("AboutWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Modal = true;
			this.DestroyWithParent = true;
			// Container child gpartimageng.AboutWindow.Gtk.Container+ContainerChild
			this.fixed2 = new global::Gtk.Fixed ();
			this.fixed2.Name = "fixed2";
			this.fixed2.HasWindow = false;
			// Container child fixed2.Gtk.Fixed+FixedChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("GPartimage-ng - это графическая оболочка для приложения \npartimage-ng, написанная на C# (Mono).");
			this.label1.Justify = ((global::Gtk.Justification)(2));
			this.fixed2.Add (this.label1);
			global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed2 [this.label1]));
			w1.X = 19;
			w1.Y = 15;
			// Container child fixed2.Gtk.Fixed+FixedChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Версия: 1.0 beta");
			this.fixed2.Add (this.label2);
			global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed2 [this.label2]));
			w2.X = 18;
			w2.Y = 77;
			// Container child fixed2.Gtk.Fixed+FixedChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Адрес git-репозитория:");
			this.fixed2.Add (this.label3);
			global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed2 [this.label3]));
			w3.X = 17;
			w3.Y = 128;
			// Container child fixed2.Gtk.Fixed+FixedChild
			this.entry1 = new global::Gtk.Entry ();
			this.entry1.WidthRequest = 370;
			this.entry1.CanFocus = true;
			this.entry1.Name = "entry1";
			this.entry1.Text = global::Mono.Unix.Catalog.GetString ("git://github.com/volgruk/GPartimage-ng.git");
			this.entry1.IsEditable = false;
			this.entry1.InvisibleChar = '•';
			this.fixed2.Add (this.entry1);
			global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed2 [this.entry1]));
			w4.X = 49;
			w4.Y = 154;
			this.Add (this.fixed2);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 462;
			this.DefaultHeight = 222;
			this.Show ();
		}
	}
}