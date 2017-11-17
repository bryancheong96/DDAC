using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UIA.Controllers
{
	public class ImageService
	{
		public async Task<string> UploadImageAsync(HttpPostedFileBase imageToUpload)
		{
			string imageFullPath = null;
			if (imageToUpload == null || imageToUpload.ContentLength == 0)
			{
				return null;
			}
			try
			{
				CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
				CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("profile");
				string uniqueBlobName = string.Format("profileimages/image_{0}{1}",Guid.NewGuid().ToString(), 
					Path.GetExtension(imageToUpload.FileName));
				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(uniqueBlobName);
				cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
				cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);
				imageFullPath = cloudBlockBlob.Uri.ToString();
			}
			catch (Exception ex)
			{

			}
			return imageFullPath;
		}
	}
}