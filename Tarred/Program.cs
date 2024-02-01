// See https://aka.ms/new-console-template for more information

using System.Formats.Tar;
using System.IO.Compression;

if (args.Length == 0)
{
    Console.WriteLine("No files or directories specified for compression.");
    Environment.Exit(1);
    return;
}

string envDir = Environment.CurrentDirectory;
string exeDir = AppDomain.CurrentDomain.BaseDirectory;
string outputFilePath = Path.Combine(string.IsNullOrEmpty(envDir) ? "." : envDir, $"{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.tar.gz");

await using FileStream outputFileStream = new(outputFilePath, FileMode.OpenOrCreate);
await using GZipStream gzStream = new GZipStream(outputFileStream, CompressionMode.Compress);
await using TarWriter tarWriter = new TarWriter(gzStream);

foreach (string arg in args)
{
    if (string.IsNullOrEmpty(arg))
    {
        continue;
    }
    
    try
    {
        FileInfo fileToCompress = new(arg);
        DirectoryInfo directoryToCompress = new(arg);

        if (directoryToCompress.Exists)
        {
            TarEntry tarEntry = new GnuTarEntry(TarEntryType.Directory, directoryToCompress.Name);
            
            await tarWriter.WriteEntryAsync(tarEntry);

            foreach (FileInfo file in directoryToCompress.EnumerateFiles())
            {
                await WriteFileIntoTar(file, Path.Combine(directoryToCompress.Name, file.Name).Replace('\\', '/'), tarWriter);
            }

            foreach (DirectoryInfo subDirectory in directoryToCompress.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                string tarDirPath = subDirectory.FullName
                    .Replace(directoryToCompress.Parent?.FullName ?? "?", string.Empty)
                    .Replace('\\', '/')
                    .TrimStart('/')
                    .TrimEnd('/');
                
                TarEntry subDirTarEntry = new GnuTarEntry(TarEntryType.Directory, tarDirPath);
            
                await tarWriter.WriteEntryAsync(subDirTarEntry);

                foreach (FileInfo nestedFileToCompress in subDirectory.EnumerateFiles())
                {
                    string nestedFileTarPath = nestedFileToCompress.FullName
                        .Replace(directoryToCompress.Parent?.FullName ?? "?", string.Empty)
                        .Replace('\\', '/')
                        .TrimStart('/')
                        .TrimEnd('/');

                    await WriteFileIntoTar(nestedFileToCompress, nestedFileTarPath, tarWriter);
                }
            }
        }
        else if (fileToCompress.Exists)
        {
            await WriteFileIntoTar(fileToCompress, fileToCompress.Name, tarWriter);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"{Environment.NewLine} Compression procedure failed. Thrown exception: {e.ToString()}");
        
        outputFileStream.Close();
        gzStream.Close();

        if (File.Exists(outputFilePath))
        {
            File.Delete(outputFilePath);
        }
        
        Console.ReadLine();
        Environment.Exit(2);
    }
}

return;

async Task WriteFileIntoTar(FileInfo fileInfo, string tarFilePath, TarWriter destinationTarWriter)
{
    Console.WriteLine($"Compressing {fileInfo.FullName}");
    
    TarEntry tarEntry = new GnuTarEntry(TarEntryType.RegularFile, tarFilePath);

    tarEntry.DataStream = fileInfo.OpenRead();
            
    await destinationTarWriter.WriteEntryAsync(tarEntry);

    tarEntry.DataStream?.Dispose();
}