namespace Models
{
    using Microsoft.WindowsAzure.Storage.Table;


    public class InputImageEntity : TableEntity
    {
        public InputImageEntity() { }

        // Define the PK and RK
        public InputImageEntity(string userId, string imageId)
        {
            this.PartitionKey = userId;
            this.RowKey = imageId;
        }

        public bool Valid { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
