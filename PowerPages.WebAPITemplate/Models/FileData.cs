using Azure.Storage.Blobs.Models;

namespace PowerPages.WebAPITemplate.Models
{
    public class BlobMetadata
    {
        /// <summary>
        /// Gets and sets the name of the blob item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the content length of the blob item.
        /// </summary>
        public long? SizeInBytes { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="DateTimeOffset"/> for when the blob was created.
        /// </summary>
        public DateTimeOffset? CreatedOn { get; set; }

        public BlobMetadata(BlobItem blob)
        {
            Name = blob.Name;
            SizeInBytes = blob.Properties.ContentLength;
            CreatedOn = blob.Properties.CreatedOn;
        }

    }
}
