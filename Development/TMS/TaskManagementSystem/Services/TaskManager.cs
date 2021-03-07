using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ListSearchModels;
using TaxationQuerySystem.HttpClient;

namespace TaskManagementSystem.Services
{
    public class TaskManager
    {
        private readonly string ApiUrl = null;

        public IConfiguration _configuration { get; }

        public TaskManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.ApiUrl = string.Concat(_configuration.GetValue<string>("TMSAPI:Url"), "/api");
            this.ApiUrl = $"{HttpRequestFactory.apihost}/api";
        }

        #region Tasks

        public async System.Threading.Tasks.Task<List<Task>> GetTasks(TaskSearchModel model)
        {
            var resp = await HttpRequestFactory.Post($"{this.ApiUrl}/Tasks/gettasks", model);
            return resp.ContentAsType<List<Task>>();
        }

        public async System.Threading.Tasks.Task<Task> GetTask(long id)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Tasks/{id}");
            return resp.ContentAsType<Task>();
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PutTask(long id, Task task)
        {
            task.documents = this.getDocBase64(task.documents);
            return await HttpRequestFactory.Put($"{this.ApiUrl}/Tasks/{id}", task);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostTask(Task task)
        {
            task.documents = this.getDocBase64(task.documents);
            return await HttpRequestFactory.Post($"{this.ApiUrl}/Tasks/", task);
        }

        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> DeleteTask(long id)
        {
            return await HttpRequestFactory.Delete($"{this.ApiUrl}/Tasks/{id}");
        }
        #endregion

        private List<Document> getDocBase64(List<Document> documents)
        {

            List<Document> docBase64 = new List<Document>();

            documents?.ForEach(doc =>
             {
                 docBase64.Add(new Document
                 {
                     formFile = null,
                     ChangeState = doc.ChangeState,
                     DocumentComments = doc.DocumentComments,
                     DocumentDate = doc.DocumentDate,
                     DocumentId = doc.DocumentId,
                     DocumentKeywordId = doc.DocumentKeywordId,
                     DocumentOwnerId = doc.DocumentOwnerId,
                     DocumentPhysicalPath = doc.DocumentPhysicalPath,
                     DocumentTaskId = doc.DocumentTaskId,
                     file = doc.formFile != null ? doc.formFile.getBase64String() : null,
                     filename = doc.filename
                 });
             });

            return docBase64;
        }

        public async System.Threading.Tasks.Task<string> DownloadFile(long id)
        {
            var resp = await HttpRequestFactory.Get($"{this.ApiUrl}/Tasks/downloadfile?id={id}");
            return resp.ContentAsType<string>();
        }
    }
}
