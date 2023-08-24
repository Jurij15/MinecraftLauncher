using Microsoft.UI.Xaml.Media.Imaging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MinecraftLauncherUniversal.Helpers
{
    public class ImageHelper
    {
        public static async Task<BitmapImage> GetBitmapAsync(byte[] data)
        {
            var bitmapImage = new BitmapImage();

			try
			{
                using (var stream = new InMemoryRandomAccessStream())
                {
                    using (var writer = new DataWriter(stream))
                    {
                        writer.WriteBytes(data);
                        await writer.StoreAsync();
                        await writer.FlushAsync();
                        writer.DetachStream();
                    }

                    stream.Seek(0);
                    await bitmapImage.SetSourceAsync(stream);
                }

                return bitmapImage;
            }
			catch (Exception ex)
			{
                Log.Error("Failed to load image with error " + ex.Message);

                return null;
			}
        }
    }
}
