using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchHashLab01
{
    class Program
    {
        static void Main(string[] args)
        {
            new HashManager().Hash();

            Console.WriteLine();
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
    }

    public class HashManager
    {
        private readonly List<string> _HashSources = new List<string> {
            "X-Hashcash: 1:8:170310152141:Hello World::MCI/Ng:Ww",
            "X-Hashcash: 1:9:170310152141:Hello World::MCI/Ng:ny",
            "X-Hashcash: 1:10:170310152141:Hello World::MCI/Ng:F2",
            "X-Hashcash: 1:11:170310152141:Hello World::MCI/Ng:QC",
            "X-Hashcash: 1:12:170310152141:Hello World::MCI/Ng:ii"
        };

        public void Hash()
        {
            foreach(var sourceValueString in _HashSources)
            {
                Console.WriteLine("Source:");
                Console.WriteLine(sourceValueString);
                var sourceValueBytes = Utilities.StringToBytes(sourceValueString);

                var sha1Mgr = new SHA1Managed();
                var hashedValueBytes = sha1Mgr.ComputeHash(sourceValueBytes);
                var hashedValueHexString = Utilities.BytesToHexString(hashedValueBytes);
                var hashedValueBinaryString = Utilities.BytesToBinaryString(hashedValueBytes);
                Console.WriteLine("SHA1:");
                Console.WriteLine(hashedValueHexString);
                Console.WriteLine(hashedValueBinaryString);

                var sha256Mgr = new SHA256Managed();
                var hashedValueBytes2 = sha256Mgr.ComputeHash(sourceValueBytes);
                var hashedValueHexString2 = Utilities.BytesToHexString(hashedValueBytes2);
                Console.WriteLine("SHA256:");
                Console.WriteLine(hashedValueHexString2);

                MainAsync(sourceValueBytes);

                Console.WriteLine();
            }
        }

        public async void MainAsync(byte[] sourceValueBytes)
        {
            var sha256AsyncMgr = new SHA256ManagedAsynch();
            var cancelSource = new CancellationTokenSource();
            var hashedValueBytes2A = await sha256AsyncMgr.ComputeHashAsync(sourceValueBytes, cancelSource.Token);
            var hashedValueHexString2A = Utilities.BytesToHexString(hashedValueBytes2A);
            Console.WriteLine("SHA256 (computed async):");
            Console.WriteLine(hashedValueHexString2A);
        }
    }

    public class SHA256ManagedAsynch : SHA256Managed
    {

        public async Task<byte[]> ComputeHashAsync(byte[] inputBytes, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(inputBytes);
            return await ComputeHashAsync(stream, cancellationToken);
        }

        public async Task<byte[]> ComputeHashAsync(Stream inputStream, CancellationToken cancellationToken)
        {
            byte[] numArray1 = new byte[4096];
            int cbSize;
            do
            {
                cbSize = await inputStream.ReadAsync(numArray1, 0, 4096, cancellationToken).ConfigureAwait(false);
                if (cbSize > 0)
                {
                    HashCore(numArray1, 0, cbSize);
                }
            }
            while (cbSize > 0);
            HashValue = HashFinal();
            var numArray2 = (byte[]) HashValue.Clone();
            Initialize();
            return numArray2;
        }
    }

    public class Utilities
    {
        public static byte[] StringToBytes(string inString)
        {
            return Encoding.UTF8.GetBytes(inString);
        }

        public static string BytesToHexString(byte[] inBytes)
        {
            return BitConverter.ToString(inBytes).ToLower().Replace("-", string.Empty);
        }

        public static Stream BytesToStream(byte[] inBytes)
        {
            return new MemoryStream(inBytes);
        }

        public static string BytesToBinaryString(byte[] inBytes)
        {
            return string.Join(string.Empty, inBytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
        }
    }
}
