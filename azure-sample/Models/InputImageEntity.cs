namespace azure_sample.Models
{
    using Microsoft.WindowsAzure.Storage.Table;


    public class InputImageEntity : TableEntity
    {
        public InputImageEntity() { }

        // Define the PK and RK
        public InputImageEntity(string userName, string fileName)
        {
            this.PartitionKey = userName;
            this.RowKey = fileName;
        }

        //For any property that should be stored in the table service, the property must be a public property of a supported type that exposes both get and set.        
        public bool Valid { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
