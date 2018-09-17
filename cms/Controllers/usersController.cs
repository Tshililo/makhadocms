
using DevExpress.Web.Mvc;
using cms;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using cms.Models;

namespace cms.Controllers
{
	//   [Authorize]
	public class usersController : BaseController
    {
		// GET: users
		public ActionResult Index()
		{
			return View();
		}


		#region list users
		//  edit method called on grid view edittemplatecontent - will create new object or get existing object
		public ActionResult EditHeaderFormPartial(Guid ObjId)
		{

			var user = db.Users.Where(c => c.Id == ObjId).SingleOrDefault();

			if (user == null)
			{
				ViewData["isEdit"] = false;
				return PartialView("UsersEditPartialFormView", new User() { TempPassword = null });
			}



			ViewData["isEdit"] = true;
			return PartialView("UsersEditPartialFormView", user);

		}
		public ActionResult ResetPassword()
		{

			return PartialView("ResetPassword");

		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword(string Email, string Password, string ConfirmPassword)
		{
			//   get user with the userId

			var user = db.Users.Where(c => c.Email == Email).SingleOrDefault();
			if (user == null)
			{
				ModelState.AddModelError("Email", "No user found");
				return PartialView("Error");
			}

			if (user != null)
			{
				var keyNew = ConfirmPassword;

				// Encrypt the password from the UI
				var password = PasswordHelper.Encrypt(ConfirmPassword, keyNew);

				// Assign the password to PasswordHash Field
				user.Password = password;

				this.UpdateModel(user);
				db.SaveChanges();

				return PartialView("ResetPasswordConfirmation", "users");
			}

			return PartialView("ResetPasswordConfirmation");
		}

		//public ActionResult ForgotPassword(Guid userId)
		//{
		//	var model = db.Users;
		//	try
		//	{
		//		// Create a token of a type guid
		//		string UserToken = Guid.NewGuid().ToString();

		//		//  get user with the userId

		//		var user = db.Users.Where(c => c.Id == userId).SingleOrDefault();

		//		//  Get email for that user


		//		string EmailId = user.Email;

		//		//create url with above token
		//		var resetLink = "<a href='" + Url.Action("ResetPassword", "users", new { un = userId, rt = UserToken }, "http") + "'>Reset Password</a>";

		//		// send mail
		//		string subject = "Palbroker Password Reset Token";
		//		string body = "Dear " + user.UserName + " <br/>" + " You recently requested to reset your Password for Palbroker <br/>"
		//		+ "Please find the Password Reset Token " + resetLink; //edit it
		//		try
		//		{
		//			SendEMail(EmailId, subject, body);
		//			TempData["Message"] = "Mail Sent.";
		//		}
		//		catch (Exception ex)
		//		{
		//			TempData["Message"] = "Error occured while sending email." + ex.Message;
		//		}
		//		// only for testing
		//		TempData["Message"] = resetLink;
		//	}
		//	catch { }

		//	return null;
		//}
		[HttpPost]

		private void SendEMail(string emailid, string subject, string body)
		{
			System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
			client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
			client.EnableSsl = true;
			client.Host = "smtp.gmail.com";
			client.Port = 587;


			System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("prsappreply@gmail.com", "Mutheiwana27");
			client.UseDefaultCredentials = false;
			client.Credentials = credentials;

			System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
			msg.From = new MailAddress("prsappreply@gmail.com");
			msg.To.Add(new MailAddress(emailid));

			msg.Subject = subject;
			msg.IsBodyHtml = true;
			msg.Body = body;

			client.Send(msg);
		}
		public ActionResult GridViewPartial()
		{
			var model = db.Users;
			return PartialView("_GridViewPartial", model.ToList());
		}

		[HttpPost, ValidateInput(true)]
		public ActionResult GridViewPartialAddNew(cms.User item)
		{
			var model = db.Users;

			if (ModelState.IsValid)
			{
				try
				{


					//  check on the database if Username and email already exist
					var currentUserToAddExists = db.Users.Where(c => c.UserName == item.UserName && c.Email == item.Email).Any();

					// if username and email don't exist then save new entry
					if (currentUserToAddExists == false)
					{
						if (string.IsNullOrWhiteSpace(item.TempPassword))
							ViewData["EditError"] = "Password may not be Empty when Creating a User. ";


						else
						{
							item.Id = Guid.NewGuid();

							//  Assign the password to PasswordHash Field
							item.Password = item.TempPassword;

							//Empty TempPassword so it's not stored in the database
							item.TempPassword = null;

							model.Add(item);
							db.SaveChanges();
						}
					}
					else
						ViewData["EditError"] = "User Already Exist. ";

				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";


			return PartialView("_GridViewPartial", model.ToList());
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult GridViewPartialUpdate(cms.User item)
		{
			var model = db.Users;

			if (ModelState.IsValid)
			{
				try
				{
					//  if password is null then assign existing password from databse to empty field and save
					if (item.TempPassword == null)
					{

						var modelItem = model.FirstOrDefault(it => it.Id == item.Id);

						//assign existing password from databse to empty field
						item.Password = modelItem.Password;


						if (modelItem != null)
						{
							this.UpdateModel(modelItem);
							db.SaveChanges();
						}
					}

					else

					// if password change is true then enrypt
					{
						// get password from the UI

						var keyNew = item.TempPassword;

						//  Encrypt the password from the UI
						var password = PasswordHelper.Encrypt(item.TempPassword, keyNew);

						// Assign the password to PasswordHash Field
						item.Password = password;

						//Empty TempPassword so it's not stored in the database
						item.TempPassword = "";

						var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
						if (modelItem != null)
						{
							this.UpdateModel(model);
							db.SaveChanges();
						}
					}

				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_GridViewPartial", model.ToList());
		}

		[HttpPost, ValidateInput(false)]
		public ActionResult GridViewPartialDelete(System.Guid Id)
		{
			var model = db.Users;
			if (Id != null)
			{
				try
				{
					var item = model.FirstOrDefault(it => it.Id == Id);
					if (item != null)
						model.Remove(item);
					db.SaveChanges();
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_GridViewPartial", model.ToList());
		}

		#endregion

		#region userroles
		[ValidateInput(false)]
		public ActionResult UserRolesPartial(string UserId)
		{
			var model = GetUserRolesDto(UserId);
			return PartialView("_UserRoles", model.ToList());
		}
		private List<UserRolesDto> GetUserRolesDto(string userName)
		{
			var model = (from ur in db.UserRoles.Where(c => c.UserId.ToString() == userName)
						 from roles in db.Roles.Where(c => c.Id.ToString() == ur.RoleId)
						 from user in db.Users.Where(c => c.Id.ToString() == ur.UserId)
						 select new UserRolesDto
						 {
							 ObjId = ur.ObjId,
							 RoleName = roles.Name,
							 RoleId = ur.RoleId,
							 UserId = user.Id.ToString()
						 });

			return model.ToList();
		}

		public ActionResult RolesPartial()
		{

			return PartialView(new Role());
		}

		//Function to get a list of roles 
		private List<Role> GetRoles()
		{
			return db.Roles.ToList();
		}
		public ActionResult UserRolesEditHeaderFormPartial(string ObjId)
		{
			var userRoleModal = db.UserRoles.Where(c => c.UserId.ToString() == ObjId).SingleOrDefault();

			//this will pass a list of roles to the front end
			ViewData["Roles"] = GetRoles();
			if (userRoleModal == null)
			{
				return PartialView("RolesEditPartialFormView", new UserRolesDto());
			}

			var model = from ur in db.UserRoles.Where(c => c.UserId.ToString() == ObjId)
						from roles in db.Roles.Where(c => c.Id.ToString() == ur.RoleId)
						from user in db.Users.Where(c => c.Id.ToString() == ur.UserId)
						select new UserRolesDto
						{
							ObjId = Guid.NewGuid(),
							RoleName = roles.Name,
							RoleId = ur.RoleId,
							UserId = user.Id.ToString()
						};

			return PartialView("RolesEditPartialFormView", model);

		}


		[HttpPost, ValidateInput(true)]
		public ActionResult UserRolesPartialAddNew(UserRolesDto item)
		{
			var model = db.UserRoles;

			if (ModelState.IsValid)
			{
				try
				{
					// double check to ensure this new Id generated does not exist
					var userRole = db.UserRoles.Where(c => c.ObjId == item.ObjId).SingleOrDefault();

					// 1 get roleId, userid we want to insert
					// 2 query the userroles on the 2 values
					// only if we get nothing - .Any() is false, then can we insert.
					// if we find a record, return ViewData["EditError"] = "Role Already exists for User";

					var currentUserRoleToAddExists = db.UserRoles.Where(c => c.RoleId == item.RoleId && c.UserId == item.UserId).Any();

					if (userRole == null && currentUserRoleToAddExists == false)
					{
						var userR = new UserRole()
						{
							ObjId = item.ObjId,
							UserId = item.UserId,
							RoleId = item.RoleId
						}; ;

						userR.UserId = item.UserId;
						userR.RoleId = item.RoleId;
						model.Add(userR);
						db.SaveChanges();
					}
					else
					{
						if (currentUserRoleToAddExists == false)
							ViewData["EditError"] = "Role Already exists for User";
						else
							ViewData["EditError"] = "User Role Id Already Exist. This is not allowed on a New record. Please Contact Support.";
						//userRole.RoleId = item.RoleId;
						//model.Attach(userRole);
						//db.SaveChanges();
					}

				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			var modelReturn = GetUserRolesDto(item.UserId.ToString());
			return PartialView("_UserRoles", modelReturn);

		}

		[HttpPost, ValidateInput(true)]
		public ActionResult UserRolesGridViewPartialUpdate(UserRolesDto item)
		{
			var model = db.UserRoles;
			if (ModelState.IsValid)
			{
				try
				{
					var userRole = db.UserRoles.Where(c => c.ObjId == item.ObjId).SingleOrDefault();

					if (userRole != null)
					{
						this.UpdateModel(userRole);
						db.SaveChanges();
					}

					else
					{
						userRole.RoleId = item.RoleId;
						model.Attach(userRole);
						db.SaveChanges();
						;
					}
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			var modelReturn = GetUserRolesDto(item.UserId.ToString());
			return PartialView("_UserRoles", modelReturn);
		}

		public ActionResult UserRolesGridViewPartialDelete(UserRolesDto item)
		{
			var model = db.UserRoles;

			if (item.ObjId != null)
			{
				try
				{
					var userRole = db.UserRoles.Where(c => c.ObjId == item.ObjId).SingleOrDefault();
					if (userRole != null)
						model.Remove(userRole);
					db.SaveChanges();
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}

			var modelReturn = GetUserRolesDto(item.UserId.ToString());
			return PartialView("_UserRoles", modelReturn);
		}
		#endregion

		public ActionResult ForgotPassword(string userId)
		{
			var model = db.Users;
			try
			{
				// Create a token of a type guid
				string UserToken = Guid.NewGuid().ToString();

				//  get user with the userId

				var user = db.Users.Where(c => c.Id.ToString() == userId).SingleOrDefault();

				//  Get email for that user


				string EmailId = user.Email;

				//create url with above token
				var resetLink = "<a href='" + Url.Action("ResetPassword", "users", new { un = userId, rt = UserToken }, "http") + "'>Reset Password</a>";

				// send mail
				string subject = "CMS Password Reset Token";
				string body = "Dear " + user.UserName + " <br/>" + " You recently requested to reset your Password for CMS <br/>"
                + "Please find the Password Reset Token " + resetLink; //edit it
				try
				{
					SendEMail(EmailId, subject, body);
					TempData["Message"] = "Mail Sent.";
				}
				catch (Exception ex)
				{
					TempData["Message"] = "Error occured while sending email." + ex.Message;
				}
				// only for testing
				TempData["Message"] = resetLink;
			}
			catch { }

			return null;
		}

	}



}