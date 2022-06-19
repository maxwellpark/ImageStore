namespace ImageStore.Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Caption { get; set; }
        public string CreationDate { get; set; }
        public string Tags { get; set; }

        public Image() { }

        public Image(string name, string filePath, string caption, string creationDate)
        {
            Name = name;
            FilePath = filePath;
            Caption = caption;
            CreationDate = creationDate;
        }

        public Image(string name, string filePath, string caption, string creationDate, string tags)
        {
            Name = name;
            FilePath = filePath;
            Caption = caption;
            CreationDate = creationDate;
            Tags = tags;
        }
    }
}
