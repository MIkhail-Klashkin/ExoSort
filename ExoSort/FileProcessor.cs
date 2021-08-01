using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoSort
{
    class FileProcessor
    {
        static string[] Merge_Sort(string[] hip)
        {
            if (hip.Length == 1)
                return hip;
            int mid_point = hip.Length / 2;
            return Merge(Merge_Sort(hip.Take(mid_point).ToArray()), Merge_Sort(hip.Skip(mid_point).ToArray()));
        }
        static string[] Merge(string[] hip1, string[] hip2)
        {
            int a = 0, b = 0;
            string[] merged = new string[hip1.Length + hip2.Length];
            for (int i = 0; i < hip1.Length + hip2.Length; i++)
            {
                if (b.CompareTo(hip2.Length) < 0 && a.CompareTo(hip1.Length) < 0)
                    if (hip1[a].CompareTo(hip2[b]) > 0)
                        merged[i] = hip2[b++];
                    else
                        merged[i] = hip1[a++];
                else
                    if (b < hip2.Length)
                    merged[i] = hip2[b++];
                else
                    merged[i] = hip1[a++];
            }
            return merged;
        }
        static async Task Merge_Files(string dir, ulong i)
        {
            string path, f1, f2;
            f1 = $@"{dir}\file{i}.txt";
            if (i % 2 == 1)
            {
                path = $@"{dir}\fileA.txt";
                f2 = $@"{dir}\fileB.txt";
            }
            else
            {
                path = $@"{dir}\fileB.txt";
                f2 = $@"{dir}\fileA.txt";
            }
            string s1,s2;
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                using (StreamReader sr1 = new StreamReader(f1, Encoding.Default))
                {
                    using (StreamReader sr2 = new StreamReader(f2, Encoding.Default))
                    {
                        s1 = await sr1.ReadLineAsync();
                        s2 = await sr2.ReadLineAsync();
                        while (true)
                        {

                            if (s1 == null)
                            {
                                if (s2 == null)
                                    break;
                                await sw.WriteLineAsync(s2);
                                while ((s2 = await sr2.ReadLineAsync()) != null)
                                    await sw.WriteLineAsync(s2);
                            }
                            else
                            {
                                if (s2 == null)
                                {
                                    await sw.WriteLineAsync(s1);
                                    while ((s1 = await sr1.ReadLineAsync()) != null)
                                        await sw.WriteLineAsync(s1);
                                }
                                else
                                {
                                    if (s1.CompareTo(s2) > 0)
                                    {
                                        await sw.WriteLineAsync(s2);
                                        s2 = await sr2.ReadLineAsync();
                                    }
                                    else
                                    {
                                        await sw.WriteLineAsync(s1);
                                        s1 = await sr1.ReadLineAsync();
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        public static async Task Sort(string path, ulong count)
        {
            string fout = $@"{Directory.GetParent(path).FullName}\tempdir";
            ulong hipCount = count / 4;
            string[] hip;
            int j;
            Directory.CreateDirectory(fout);
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    for (ulong i = 0; i < hipCount; i++)
                    {
                        hip = new string[4];
                        j = 0;
                    while (j < 4 && (hip[j] = await sr.ReadLineAsync()) != null)
                        j++;
                    File.Create($@"{fout}\file{i}.txt").Dispose();
                    File.WriteAllLines($@"{fout}\file{i}.txt", Merge_Sort(hip));
                    }
                    if (count % 4 > 0)
                    {
                        hip = new string[count % 4];
                        j = 0;
                        while ((j < (int)(count % 4)) && (hip[j] = await sr.ReadLineAsync()) != null)
                            j++;
                        File.Create($@"{fout}\file{hipCount}.txt").Dispose();
                        File.WriteAllLines($@"{fout}\file{hipCount}.txt", Merge_Sort(hip));
                        hipCount++;
                    }
                }
                File.Create($@"{fout}\fileA.txt").Dispose();
                File.Create($@"{fout}\fileB.txt").Dispose();
                for (ulong i = 0; i < hipCount; i++)
                {
                    await Merge_Files(fout, i);
                }
                File.Delete(path);
                if (hipCount % 2 == 0)
                    File.Move($@"{fout}\fileA.txt", path);
                else
                    File.Move($@"{fout}\fileB.txt", path);
                Directory.Delete(fout,true);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}

