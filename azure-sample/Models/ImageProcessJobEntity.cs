namespace Models
{
    using Microsoft.WindowsAzure.Storage.Table;


    public class ImageProcessJobEntity : TableEntity
    {
        public ImageProcessJobEntity() { }

        // Define the PK and RK
        public ImageProcessJobEntity(string accountId, string orderId)
        {
            this.PartitionKey = accountId;
            this.RowKey = orderId;
        }

        public string ImageId { get; set; }
        public string Status { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
