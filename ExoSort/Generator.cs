using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoSort
{
    class Generator
    {
        private uint length;
        private ulong count;
        public uint Length
        {
            get { return length; }
            
            set 
            {
                if (value > 1000000000)
                    length = 1000000000;
                else if (value < 1)
                    length = 1;
                else
                    length = value;
            }
        }
        public ulong Count 
        {
            get { return count; }
                 
            set
            {
                if (value < 1)
                    count = 1;
                else
                    count = value;
              
            }
        }


        public Generator()
        {
            Length = 255;
            Count = 1000000000;

        }
        public Generator(ulong count)
        {
            Length = 255;
            Count = count;
        }
        public Generator(uint length, ulong count)
        {
            Length = length;
            Count = count;
        }

        public async Task GetFile(string path)
        {
            string vocabulary = "qQwWeErRtTyYuUiIoOpPaAsSdDfFgGhHjJkKlLzZxXcCvVbBnNmM1234567890";
            int vocLength = vocabulary.Length;
            Random r = new Random();
            
            File.Create(path).Dispose();
            Char[] text = new Char[Length];
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                {
                    for (ulong i = 0; i < Count; i++)
                    {
                        for (uint j = 0; j < Length; j++)
                        {
                            text[j] = vocabulary[r.Next(vocLength)];
                        }

                        await sw.WriteLineAsync(text);
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
}
