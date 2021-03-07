using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class MediaManager : XmlFileHandler<List<Media>>
    {
        public MediaManager(IHostingEnvironment environment) : base(environment, "ArrayOfMedia", "media.xml")
        {

        }

            #region Media
        public void AddMedia(Media newMedia)
        {
            xmlObjects.Add(newMedia);
        }

        public void EditMedia(Media media)
        {
            int index = xmlObjects.FindIndex(m => m.Id == media.Id);
            Media[] mediaArr = xmlObjects.ToArray();
            mediaArr[index] = media;
            xmlObjects = mediaArr.ToList();
        }

        public void RemoveMedia(Media media)
        {
            xmlObjects.Remove(media);
        }
        public void AddFile(long mediaId, File file)
        {
            int mediaIndex = xmlObjects.FindIndex(m => m.Id == mediaId);
            Media[] MediaArr = xmlObjects.ToArray();
            Media parentMedia = MediaArr[mediaIndex];
            parentMedia.files.Add(file);
            MediaArr[mediaIndex] = parentMedia;
            xmlObjects = MediaArr.ToList();
        }

        public void RemoveFile(long mediaId, File file)
        {
            int mediaIndex = xmlObjects.FindIndex(m => m.Id == mediaId);
            Media[] MediaArr = xmlObjects.ToArray();
            Media media = MediaArr[mediaIndex];
            media.files.Remove(file);
            MediaArr[mediaIndex] = media;
            xmlObjects = MediaArr.ToList();
        }
        #endregion
    }
}
