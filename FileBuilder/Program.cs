using System;
using System.IO;

namespace FileBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string main_ref = "";

            string folder_destination = "";

            if (File.Exists(main_ref))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(main_ref, FileMode.Open)))
                {
                    string file_name = reader.ReadString();
                    int file_size = reader.ReadInt32();
                    int number_of_chunks = reader.ReadInt32();
                    int chunk_size = reader.ReadInt32();
                    int last_chunk_size = reader.ReadInt32();

                    string[] files_location = new string[number_of_chunks];

                    for (int i = 0; i < number_of_chunks; i++)
                    {
                        files_location[i] = reader.ReadString();
                        reader.ReadString(); // hash
                    }

                  
                    byte[][] file = new byte[number_of_chunks][];

                    if (last_chunk_size < chunk_size)
                    {
                        number_of_chunks--;
                    }

                    for(int i = 0; i < number_of_chunks; i++)
                    {
                        using (BinaryReader r = new BinaryReader(File.Open(files_location[i], FileMode.Open)))
                        {
                            file[i] = r.ReadBytes(chunk_size);
                        }
                    }

                    if (last_chunk_size < chunk_size)
                    {
                        using (BinaryReader r = new BinaryReader(File.Open(files_location[files_location.Length - 1], FileMode.Open)))
                        {
                            file[file.Length - 1] = reader.ReadBytes(last_chunk_size);
                        }
                    }

                    using (BinaryWriter writer = new BinaryWriter(File.Open(folder_destination + file_name, FileMode.Create)))
                    {
                        for (int i = 0; i < file.Length; i++)
                        {
                            writer.Write(file[i]);
                        }
                    }
                }
            }

        }
    }
}
