using System.IO;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models.UiModels
{
    public class ImageModel : NotificationModel
    {
        private byte[] _imageData;
        private ImageSource _imageSource;

        public byte[] ImageData
        {
            get => _imageData;
            set => this.SetValue(ref _imageData, value, nameof(ImageData));
        }
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => this.SetValue(ref _imageSource, value, nameof(ImageSource));
        }

        public ImageModel(byte[] data = null)
        {
            this.InitilizeSourceAsync(data).GetAwaiter();
        }

        public async Task InitilizeSourceAsync(byte[] data)
        {
            await Task.Run(() =>
            {
                if (data != null)
                {
                    ImageData = data;

                    using (var stream = new MemoryStream(ImageData))
                    {
                        ImageSource = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                }
            });
        }
    }
}