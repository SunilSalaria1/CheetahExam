namespace CheetahExam.WebUI.Shared.Common.Models;

public class FileDetailDto
{
    public string Path { get; set; } = null!;
    public FileMetaData MetaData { get; set; } = new();
}

public class FileMetaData
{
    public byte[] FileBytes { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public long FileSize { get; set; }
}
