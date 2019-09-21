using System.Windows.Forms;

namespace PassiveWallpaperF
{
    public partial class PassiveWallpaper
    {
        public class BackgroundForm
        {
            public bool Paused { get; set; } = false;
            public Form Form { get; set; } = new Form();
        }
    }
}
