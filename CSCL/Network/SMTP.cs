using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CSCL.Network
{
	/// <summary>
	/// The .Net Framework 2.0 supposedly has twice the number of classes as .NET Framework 1.1. 
	/// Many of these are completely new and will require considerable study. Some are "enhanced" 
	/// versions of old classes added to existing namespaces, and some, like System.Net.Mail, are 
	/// enhanced versions of pre-existing classes that have been moved to a new namespace.
	/// 
	/// In the case of System.Net.Mail, that's a good thing. What the hell it was ever doing in 
	/// System.Web, I'll never know! At any rate, it has some quality enhancements, different methods, 
	/// and asynchronous support out of the box, and that's good. Although no longer explicitly dependent 
	/// on CDO.SYS, it is still dependent on COM+. Let's get down to the basics of sending email either 
	/// from Windows Forms or ASP.NET with a handy helper class. Now this is not the Data Access 
	/// Application Block "EmailHelper" class by a long shot. It just gets some basic concepts going. 
	/// Look through the code I've cobbled together first, and then I'll explain:
	/// 
	/// You can see I have two overloads of a SendMailMessage static method, and a SendMailMessageAsync 
	/// method with a matching SendCompletedCallback method. I've also added a public event, NotifyCaller, 
	/// with a matching target method, OnNotifyCaller. I could have used the SendCompletedCallback to do 
	/// this, but sometimes its cleaner to actually fire a separate, public event from within a callback 
	/// which may really be more involved in performing internal business logic.
	/// 
	/// We can see that in terms of the "Chunky" method signature, we need an SMTPServer, a fromAddress, 
	/// a fromName, a toAddress, a toName, a msgSubject, and a msgBody to get 100% to first base with a 
	/// single static method call.
	/// 
	/// Having gotten all the goodies assembled in our method body, we need to create an instance of the 
	/// SMTPClient class around our provided SMTPHost. We then need to create two MailAddress instances, 
	/// one for the "To" and one for the "From" address. Finally, we create a new MailMessage instance, 
	/// passing in our From and To MailAddress objects, and we can set the Subject, Body and call the 
	/// Send method. That's why I like helper classes with public static "Chunky" methods. You give me 
	/// all the stuff in one shot, I take care of the rest. Easy!
	/// 
	/// In the Async Method, we set up our SendCompletedCallback:
	/// 
	/// client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
	/// 
	/// and call
	/// 
	/// client.SendAsync(message, message.To.ToString());
	/// 
	/// Note that the second parameter is the typical Async State object; in this case the "To" address 
	/// string is sufficient to identify which email is the one being handled by my callback. Since this 
	/// is an object, I can send a whole class in there if I need it, and cast it back to its type within 
	/// my callback and use it for whatever business logic. For example, I might have a MailStatus class 
	/// that holds, among other items, a collection of indexed emails I need to send, each of which has a 
	/// "Sent" status that I can update in the callback.
	/// 
	/// There is a lot more to the System.Net.Mail namespace, but for most developers, this is enough. 
	/// Oh, yes -- almost forgot-- attachments.
	/// 
	/// Attachments are easy, and there is enough flexibility to load one from a file or from a stream:
	/// 
	/// Attachment at = new Attachment(this.attachmentPath);
	/// msgWithAttachment.Attachments.Add(at);
	/// Utils.Mail.MailSender.SendMailMessageAsync(this.txtSMTPHost.Text, msgWithAttachment);
	/// 
	/// As can be seen, our AttachmentCollection is already a part of the MailMessage class, making 
	/// handling these boogers a cinch.
	/// 
	/// When you download the Visual Studio.Net 2005 Solution below, you will find the MailSender 
	/// utility class, a Windows Forms Tester that lets you browse for an attachment, and an ASP.NET 2.0 
	/// Web application that lets you do the same with a FileUpload control that handles the email and 
	/// the attachment from the server all in one fell swoop with a bunch of additional code. The web 
	/// project is the new Cassini built-in file type, so you don't even need to think about IIS and 
	/// virtual directories -- just unzip it wherever you want and it will all work great.
	/// 
	/// There's a lot to ASP.NET 2.0. If you haven't discovered the QuickStart Web Application on 
	/// your hard drive, that's an excellent place to start. It's well designed, and I'm studying it. 
	/// If you didn't install it, look here:
	/// 
	/// C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\QuickStart
	/// 
	/// If the folder and its files / subfolders are there (you installed the SDK), just fire up IIS, 
	/// create a new "Virtual Directory", and point it to the above path. Name it "quickstart"
	/// 
	/// Set the ASP.NET Tab on IIS to use the 2.XXXX Framework, and request "Http://localhost/quickstart", 
	/// and you will be on your way.
	/// 
	/// By the way, the "sourceview" element in the web.config is mistakenly left to the settings of the 
	/// last Microsoft developer responsible for it before the build (most likely NOT where yours will be), 
	/// so change it to :
	/// 
	/// <sourceview>
	/// <add key="root" value="C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\QuickStart" />
	/// </sourceview>
	/// </summary>
	public class SMTP
	{
		public static string mailStatus=String.Empty;
		public static event EventHandler NotifyCaller;
		protected static void OnNotifyCaller()
		{
			if(NotifyCaller!=null) NotifyCaller(mailStatus, EventArgs.Empty);
		}

		public static bool SendMailMessage(string SMTPServer, string fromAddress,
		 string fromName, string toAddress, string toName, string msgSubject, string msgBody)
		{
			try
			{
				SmtpClient client=new SmtpClient(SMTPServer);
				MailAddress from=new MailAddress(fromAddress, fromName);
				MailAddress to=new MailAddress(toAddress, toName);
				MailMessage message=new MailMessage(from, to);
				message.Subject=msgSubject;
				message.Body=msgBody;
				client.Send(message);
			}
			catch(System.Net.Mail.SmtpException)
			{
				throw;
			}
			catch(Exception)
			{
				throw;
			}
			return true;
		}

		public static void SendMailMessageAsync(string SMTPServer, MailMessage message)
		{
			try
			{
				SmtpClient client=new SmtpClient(SMTPServer);
				client.SendCompleted+=new SendCompletedEventHandler(SendCompletedCallback);
				client.SendAsync(message, message.To.ToString());
			}
			catch(System.Net.Mail.SmtpException)
			{
				throw;
			}
			catch(Exception)
			{
				throw;
			}
		}

		public static bool SendMailMessage(string SMTPServer, MailMessage message)
		{
			try
			{
				SmtpClient client=new SmtpClient(SMTPServer);
				client.Send(message);
			}
			catch(System.Net.Mail.SmtpException)
			{
				throw;
			}
			catch(Exception)
			{
				throw;
			}
			return true;
		}

		public static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			String token=(string)e.UserState;
			if(e.Cancelled)
			{
				mailStatus=token+" Send canceled.";
			}
			if(e.Error!=null)
			{
				mailStatus="Error on "+token+": "+e.Error.ToString();
			}
			else
			{
				mailStatus=token+" mail sent.";
			}
			OnNotifyCaller();
		}

		/// <summary>
		/// Ermittelt ob es sich um eine echte Mail Adresse handelt
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public static bool IsValidMailAddress(string address)
		{
			//TODO: Durch IsMatch ersetzen
			MatchCollection regexMatchCollection=Regex.Matches(address, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
			if(regexMatchCollection.Count!=0) return true;
			else return false;
		}
	}
}
