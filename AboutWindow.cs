using System;

namespace gpartimageng
{
	public partial class AboutWindow : Gtk.Window
	{
		public AboutWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

