using System;
using System.IO;
using System.Threading.Tasks;

namespace Image16
{
    public static class Process
    {
        public static void Run(string firstFilePath, string secondFilePath)
        {
            if (string.IsNullOrEmpty(firstFilePath) || string.IsNullOrEmpty(secondFilePath))
            {
                return;
            }

            if (!File.Exists(firstFilePath) || !File.Exists(secondFilePath))
            {
                return;
            }

            var firstFile = File.OpenRead(firstFilePath);
            var secondFile = File.OpenRead(secondFilePath);

            if (firstFile.Length != secondFile.Length)
            {
                return;
            }

            var filesParamsBytesLength = 32;
            var offset = 512;
            var filesImageBytesLength = (int)firstFile.Length - offset;

            var firstFileParamsBytes = new byte[filesParamsBytesLength];
            var secondFileParamsBytes = new byte[filesParamsBytesLength];
            ImageParamsModel firstFileParamsModel = null;
            ImageParamsModel secondFileParamsModel = null;
            var firstFileImageBytes = new byte[filesImageBytesLength];
            var secondFileImageBytes = new byte[filesImageBytesLength];
            var resultFileImageBytes = new byte[filesImageBytesLength];

            var cnt = 0;

            Parallel.Invoke(
                () =>
                {
                    firstFile.Read(firstFileParamsBytes, 0, firstFileParamsBytes.Length);
                    firstFileParamsModel = MarshalHelper.UnpackStruct<ImageParamsModel>(firstFileParamsBytes);
                    firstFile.Position = 512;
                    firstFile.Read(firstFileImageBytes, 0, firstFileImageBytes.Length);
                },
                () =>
                {
                    secondFile.Read(secondFileParamsBytes, 0, secondFileParamsBytes.Length);
                    secondFileParamsModel = MarshalHelper.UnpackStruct<ImageParamsModel>(secondFileParamsBytes);
                    secondFile.Position = 512;
                    secondFile.Read(secondFileImageBytes, 0, secondFileImageBytes.Length);
                });

            Parallel.For(cnt, filesImageBytesLength,
                _ =>
                {
                    var first = BitConverter.ToInt16(firstFileImageBytes, cnt);
                    var second = BitConverter.ToInt16(secondFileImageBytes, cnt);

                    var devide = BitConverter.GetBytes((short)(first / second));

                    resultFileImageBytes[cnt] = devide[0];
                    resultFileImageBytes[cnt + 1] = devide[1];

                    cnt += 2;
                });

            var resultFileBytes = new byte[offset + filesImageBytesLength];

            // Предположил, что для третьего файла у нас есть параметры, для заглушки просто скопировал параметры первого
            var resultFileParamsBytes = MarshalHelper.PackStruct(firstFileParamsModel);

            resultFileParamsBytes.CopyTo(resultFileBytes, 0);
            resultFileImageBytes.CopyTo(resultFileBytes, offset);

            // Какой-то путь для сохранения результата
            File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "result.ELI"), resultFileBytes);
        }
    }
}
