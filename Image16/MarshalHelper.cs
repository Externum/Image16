using System.Runtime.InteropServices;

namespace Image16
{
    public static class MarshalHelper
    {
        public static T UnpackStruct<T>(byte[] data)
        {
            if (data.Length == 0)
            {
                return default;
            }

            var unmanagedPointer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, unmanagedPointer, data.Length);
            var result = (T)Marshal.PtrToStructure(unmanagedPointer, typeof(T));
            Marshal.FreeHGlobal(unmanagedPointer);
            return result;
        }

        public static byte[] PackStruct<T>(T dataStruct)
        {
            if (dataStruct == null)
            {
                return new byte[0];
            }

            var size = Marshal.SizeOf(dataStruct);
            var byteArray = new byte[size];
            var unmanagedPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(dataStruct, unmanagedPointer, false);
            Marshal.Copy(unmanagedPointer, byteArray, 0, size);
            Marshal.FreeHGlobal(unmanagedPointer);

            return byteArray;
        }
    }
}
