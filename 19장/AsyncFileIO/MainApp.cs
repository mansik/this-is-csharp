﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsyncFileIO
{
    class MainApp
    {
        // 파일 복사 후 복사한 파일 용량 반환
        static async Task<long> CopyAsync(string fromPath, string toPath)
        {
            using (var fromStream = new FileStream(fromPath, FileMode.Open))
            {
                long totalCopied = 0;

                using (var toStream = new FileStream(toPath, FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int nRead = 0; // 버퍼 안으로 읽어들인 총 바이트 수, 다 읽으면 0을 반환

                    // ReadAsync() : 현재 스트림에서 바이트 시퀀스를 읽고 읽은 바이트 수만큼 스트림에서 위치를 비동기적으로 앞으로 이동합니다.
                    while ((nRead = await fromStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        await toStream.WriteAsync(buffer, 0, nRead);
                        totalCopied += nRead;
                    }
                }
                return totalCopied;
            }
        }

        static async void DoCopy(string fromPath, string toPath)
        {
            long totalCopied = await CopyAsync(fromPath, toPath);
            Console.WriteLine($"Copied Total {totalCopied} Bytes.");
        }

        static void Main(string[] args)
        {
            if (args.Length < 2) {
                Console.WriteLine("Usage : AsyncFileIO <Source> <Destination>");
                return;
            }

            DoCopy(args[0], args[1]);

            Console.ReadLine(); // 프로그램 종료 방지
        }


    }
}