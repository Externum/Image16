using System.Runtime.InteropServices;

namespace Image16
{
    [StructLayout(LayoutKind.Sequential)]
    public class ImageParamsModel
    {
        public int Signature { get; set; }

        public int HeaderBytesLength { get; set; }

        public int ImageOffset { get; set; }

        public int Reserved { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public int PixelBitCount { get; set; }

        public int ByteLineLength { get; set; }
    }
}
