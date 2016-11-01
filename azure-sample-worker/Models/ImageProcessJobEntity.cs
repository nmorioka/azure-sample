namespace Models
{
    using Microsoft.WindowsAzure.Storage.Table;


    public class ImageProcessJobEntity : TableEntity
    {
        public ImageProcessJobEntity() { }

        // Define the PK and RK
        public ImageProcessJobEntity(string orderId, string imageId)
        {
            this.PartitionKey = orderId;
            this.RowKey = imageId;
        }

        public string Status { get; set; }
        public string ResultId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
