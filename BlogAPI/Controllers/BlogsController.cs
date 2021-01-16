
using Authentication;
using BlogDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BlogAPI.Controllers
{

	[BasicAuthentication]
	public class BlogsController : ApiController
    {
		[HttpGet]
		public IEnumerable<BlogTable> GetAllBlogs() {
			
				using (BLogDatabaseEntities context = new BLogDatabaseEntities())
				{
					return context.BlogTables.ToList();
				}
			
        }

		
		[HttpPost]
		public HttpResponseMessage PostNewBlog([FromBody] BlogTable obj)
		{
			using (BLogDatabaseEntities context = new BLogDatabaseEntities())
			{
				try
				{


					context.BlogTables.Add(obj);
					context.SaveChanges();
					return Request.CreateResponse(HttpStatusCode.Created, "Added Blog_ID : " + obj.BlogId + " Blog_Title : " + obj.BlogTitle);


				}
				catch (Exception ex)
				{

					ErrorLog eo = new ErrorLog() { ErrorMessage = "Adding Blog Failed with error -  " + ex, Severity = "low" };
					context.ErrorLogs.Add(eo);
					context.SaveChanges();

					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please provide blog details to add.");
				}
			}
		}

		
		[HttpDelete]
		public HttpResponseMessage DeleteBlog(int BlogId)
		{
			using (BLogDatabaseEntities context = new BLogDatabaseEntities())
			{
				try
				{


					var entity = context.BlogTables.FirstOrDefault(x => x.BlogId == BlogId);
					if (entity == null)
					{
						ErrorLog eo = new ErrorLog() { ErrorMessage = "Requested Blog_id " + BlogId + " not found!", Severity = "medium" };
						context.ErrorLogs.Add(eo);
						context.SaveChanges();
						return Request.CreateResponse(HttpStatusCode.NotFound, "Blog_ID : " + BlogId + "Not Found");
					}
					else
					{
						context.BlogTables.Remove(entity);
						context.SaveChanges();
						return Request.CreateResponse(HttpStatusCode.OK, "Blog_ID : " + BlogId + " Deleted");
					}



				}
				catch (Exception ex)
				{
					
						ErrorLog eo = new ErrorLog() { ErrorMessage = "Deleting Blog Failed with error -  " + ex, Severity = "low" };
						context.ErrorLogs.Add(eo);
						context.SaveChanges();
					
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
				}
			}

		}

		
		[HttpPut]
		public HttpResponseMessage PutUpdateBlog([FromBody] BlogTable obj)
		{
			using (BLogDatabaseEntities context = new BLogDatabaseEntities())
			{
				try
				{


					var entity = context.BlogTables.FirstOrDefault(x => x.BlogId == obj.BlogId);
					if (entity == null)
					{
						ErrorLog eo = new ErrorLog() { ErrorMessage = "Edit - Requested Blog_id " + obj.BlogId + " not found!", Severity = "medium" };
						context.ErrorLogs.Add(eo);
						context.SaveChanges();
						return Request.CreateResponse(HttpStatusCode.NotFound, "Blog_ID : " + obj.BlogId + " Not Found");
					}
					else
					{
						entity.BlogTitle = obj.BlogTitle;
						entity.Content = obj.Content;
						entity.LastModified = DateTime.Now;
						context.SaveChanges();
						return Request.CreateResponse(HttpStatusCode.OK, "Blog_ID : " + obj.BlogId + " Updated");
					}



				}
				catch (Exception ex)
				{

					ErrorLog eo = new ErrorLog() { ErrorMessage = "Editing Blog Failed with error -  " + ex, Severity = "low" };
					context.ErrorLogs.Add(eo);
					context.SaveChanges();

					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
				}
			}
		}


	}
}
